using System;
using System.Collections.Generic;
using HappinessIndex.Models;
using Xamarin.Forms;
using System.Linq;
using Syncfusion.SfChart.XForms;
using System.Threading.Tasks;
using HappinessIndex.Resx;
using Xamarin.Essentials;
using HappinessIndex.DependencyService;
using HappinessIndex.Helpers;

namespace HappinessIndex.ViewModels
{
    public class ReportPageViewModel : ViewModelBase
    {
        public Command GenerateRecordsCommand { get; set; }

        public List<Factor> Factors { get; set; }

        public User User { get; set; }

        private DateTime endDate = DateTime.Now;

        public DateTime EndDate
        {
            get => endDate;
            set
            {
                if (endDate == value) return;
                endDate = value;

                NotifyPropertyChanged();
            }
        }

        private DateTime startDate = DateTime.Now;

        public DateTime StartDate
        {
            get => startDate;
            set
            {
                if (startDate == value) return;
                startDate = value;

                NotifyPropertyChanged();
            }
        }

        private bool canDisplayChart;

        public bool CanDisplayChart
        {
            get => canDisplayChart;
            set
            {
                if (canDisplayChart == value) return;
                canDisplayChart = value;

                NotifyPropertyChanged();
            }
        }

        private List<OveralScore> overallSource;

        public List<OveralScore> OverallSource
        {
            get => overallSource;
            set
            {
                if (overallSource == value) return;
                overallSource = value;

                NotifyPropertyChanged();
            }
        }

        private List<Journal> chartSource;

        public List<Journal> ChartSource
        {
            get => chartSource;
            set
            {
                if (chartSource == value) return;
                chartSource = value;

                NotifyPropertyChanged();
            }
        }

        private ChartSeriesCollection inhibitorsLine = new ChartSeriesCollection();

        public ChartSeriesCollection InhibitorsLine
        {
            get => inhibitorsLine;
            set
            {
                if (inhibitorsLine == value) return;
                inhibitorsLine = value;

                NotifyPropertyChanged();
            }
        }

        private string resultDisplayText;

        public string ResultDisplayText
        {
            get => resultDisplayText;
            set
            {
                if (resultDisplayText == value) return;
                resultDisplayText = value;

                NotifyPropertyChanged();
            }
        }

        private ChartColorCollection colors;

        public ChartColorCollection Colors
        {
            get => colors;
            set
            {
                if (colors == value) return;
                colors = value;

                NotifyPropertyChanged();
            }
        }

        public ReportPageViewModel()
        {
            GenerateRecordsCommand = new Command(GenerateRecords);
        }

        bool autoRefresh = false;

        private void RefreshData()
        {
            ChartSource = null;
            OverallSource = null;
            ResultDisplayText = "";
            CanDisplayChart = false;

            autoRefresh = true;

            StartDate = AppSettings.JournalDate;
            EndDate = AppSettings.JournalDate;

            //if ((EndDate.Date - StartDate.Date).Days == 0)
            //{
            //    StartDate = Preferences.Get(AppSettings.StartDateKey, DateTime.Now.Date);
            //    EndDate = DateTime.Now;
            //}

            GenerateRecords();

            //IsBusy = true;

            //var data = 

            //IsBusy = false;
        }

        protected override void OnAppearing()
        {
            RefreshData();

            base.OnAppearing();
        }

        private async void GenerateRecords()
        {
            IsBusy = true;
            CanDisplayChart = false;

            User = await DataService.GetUserAsync(Preferences.Get(AppSettings.EmailKey, ""));

            Factors = await DataService.GetAllFactors(User);

            ValidateAndPrintDateRange();

            var chartSource = new List<Journal>();

            var records = await DataService.GetAllJounralsAsync(StartDate, EndDate);

            var isInhibitorsRecordsAvailable = await GenerateInhibitorsRecords();

            if (records.Count == 0 && !isInhibitorsRecordsAvailable)
            {
                if (autoRefresh)
                {
                    autoRefresh = false;
                    IsBusy = false;
                    return;
                }
                await Application.Current.MainPage.DisplayAlert("", AppResources.NoRecordsFound, AppResources.Ok);
                IsBusy = false;
                return;
            }
            Xamarin.Forms.DependencyService.Resolve<IAppRating>().RateApp();

            CanDisplayChart = true;

            var distinctRecords = records.GroupBy(item => item.FactorID);

            foreach (var record in distinctRecords)
            {
                var factor = record.FirstOrDefault();
                var total = record.Count();

                Journal journal = new Journal()
                {
                    FactorID = factor.FactorID,
                    ActualValue = record.Sum(item => item.ActualValue) / total,
                    Date = factor.Date,
                    UserID = factor.UserID,
                };

                chartSource.Add(journal);
            }

            GenerateColors(chartSource);

            await GenerateCommonData();

            ChartSource = chartSource;

            autoRefresh = false;
            IsBusy = false;
        }

        public List<NegativeFactor> InhibitorsChartSource { get; set; }

        private async Task<bool> GenerateInhibitorsRecords()
        {
            var data = await DataService.GetNegativeFactorAsync(StartDate, EndDate);
            InhibitorsChartSource = data;

            if(data != null && data.Count == 0)
            {
                return false;
            }

            var groupedData = data.GroupBy(item => item.Name);

            var series = new ChartSeriesCollection();

            App.GetShell().Resources.TryGetValue("InhibitorTooltipTemplate", out object tooltipTemplate);

            foreach (var group in groupedData)
            {
                SplineSeries lineSeries = new SplineSeries
                {
                    ItemsSource = group,
                    EnableAnimation = true,
                    XBindingPath = "Date",
                    YBindingPath = "Value",
                    SplineType = SplineType.Cardinal,
                    EnableTooltip = true,
                    TooltipTemplate = tooltipTemplate as DataTemplate
                };
                lineSeries.DataMarkerLabelCreated += LineSeries_DataMarkerLabelCreated;

                var item = group.FirstOrDefault();
                if (item != null)
                {
                    lineSeries.Label = item.DisplayName;
                }

                if (group.Sum(item => item.Value) == 0)
                {
                    lineSeries.IsVisible = false;
                }

                lineSeries.DataMarker = new ChartDataMarker
                {
                    ShowMarker = true,
                    ShowLabel = false
                };
                series.Add(lineSeries);
            }

            InhibitorsLine = series;
            return true;
        }

        private void LineSeries_DataMarkerLabelCreated(object sender, ChartDataMarkerLabelCreatedEventArgs e)
        {
            if (e.DataMarkerLabel.Index == 0)
            {
                e.DataMarkerLabel.MarkerType = DataMarkerType.Square;
                e.DataMarkerLabel.MarkerHeight = 8;
                e.DataMarkerLabel.MarkerWidth = 8;
            }
        }

        private void ValidateAndPrintDateRange()
        {
            var dateDiff = EndDate - StartDate;

            if (dateDiff.Days == 0)
            {
                ResultDisplayText = "{0} " + AppResources.ReportFor + StartDate.ToString("MMM-dd");
            }
            else
            {
                ResultDisplayText = "{0} " + AppResources.ReportGeneratedFrom + StartDate.ToString("MMM-dd") + AppResources.To1 +
                    EndDate.ToString("MMM-dd") + AppResources.AvgValues;
            }
        }

        private void GenerateColors(List<Journal> chartSource)
        {
            //var colors = new ChartColorCollection();
            //var realityColor = new ChartColorCollection();

            var noData = new List<Journal>();

            foreach (var journal in chartSource)
            {
                var factor = Factors.Where(item => item.ID == journal.FactorID).FirstOrDefault();

                if(factor != null)
                {
                    journal.Factor = factor;
                    //colors.Add(factor.Color);
                }
                else
                {
                    noData.Add(journal);
                }
            }

            foreach (var empty in noData)
            {
                chartSource.Remove(empty);
            }

            //Colors = colors;
            Colors = ColorHelper.DefaultColors;
        }

        private async Task GenerateCommonData()
        {
            var commonRecords = await DataService.GetOverallScoreAsync(StartDate, EndDate);

            var totalCommonRecords = commonRecords.Count();

            if (commonRecords != null)
            {
                OverallSource = new List<OveralScore> { new OveralScore{
                    ActualValue = commonRecords.Sum(item => item.ActualValue) / totalCommonRecords
                } };
            }
        }
    }
}