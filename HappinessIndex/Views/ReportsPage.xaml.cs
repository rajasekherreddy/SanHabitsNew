using HappinessIndex.ViewModels;
using Syncfusion.SfChart.XForms;
using Xamarin.Essentials;
using Xamarin.Forms;
using System;
using HappinessIndex.Resx;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System.Globalization;
using HappinessIndex.Models;

namespace HappinessIndex.Views
{
    public class DateTimeAxisExt : DateTimeAxis
    {
        public DateTime ActualMinimum { get; set; }

        public DateTime ActualMaximum { get; set; }

        protected override void OnCreateLabels()
        {
            base.OnCreateLabels();

            VisibleLabels.Clear();

            ReportPageViewModel viewModel = BindingContext as ReportPageViewModel;

            var source = viewModel.InhibitorsChartSource;

            var labelMinimumGap = (ActualMaximum - ActualMaximum).TotalHours / 7;

            if (source != null)
            {
                var list = source.OrderBy(item => item.Date);

                var previousTime = DateTime.MinValue;

                foreach (var data in list)
                {
                    if (data.Date - previousTime < TimeSpan.FromHours(labelMinimumGap))
                    {
                        continue;
                    }

                    VisibleLabels.Add(new ChartAxisLabel(data.Date.ToOADate(), data.Date.ToString("MMM-dd HH:mm")));
                    previousTime = data.Date;
                }
            }
        }
    }

    public class ChartTitleStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            value = value ?? "";
            if (ReportsPage.selectedIndex == 0)
            {
                return string.Format(value.ToString(), AppResources.EnablersReport); ;
            }
            else
            {
                return string.Format(value.ToString(), AppResources.InhibitorsReport);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public partial class ReportsPage : ContentPage
    {
        void CategoryAxis_LabelCreated_1(System.Object sender, Syncfusion.SfChart.XForms.ChartAxisLabelEventArgs e)
        {
            e.LabelContent = AppResources.OverallDaysScore;
        }

        public ReportsPage()
        {

            InitializeComponent();

            viewModel = BindingContext as ReportPageViewModel;

            if (DeviceInfo.Name.Replace(" ", "").ToLower() == "iphone7")
            {
                FromLabel.FontSize = 9;
                ToLabel.FontSize = 9;
            }
        }

        private void DateTimeCategoryAxis_ActualRangeChanged(object sender, ActualRangeChangedEventArgs e)
        {
            //if (Device.RuntimePlatform == "Android") return;

            var min = DateTime.FromOADate(double.Parse(e.ActualMinimum.ToString()));
            var max = DateTime.FromOADate(double.Parse(e.ActualMaximum.ToString()));

            var diff = max - min;

            if (diff.TotalHours < 1)
            {
                max = min.AddHours(1);
                e.ActualMaximum = max.ToOADate();
            }
            else
            {
                max = max.AddHours(1);
                e.ActualMaximum = max.ToOADate();
            }

            (sender as DateTimeAxisExt).ActualMinimum = min;
            (sender as DateTimeAxisExt).ActualMaximum = max;
        }

        void CategoryAxis_LabelCreated(System.Object sender, ChartAxisLabelEventArgs e)
        {
            //if (Device.RuntimePlatform == "Android") return;

            var viewModel = BindingContext as ReportPageViewModel;

            if (viewModel != null && viewModel.Colors != null && viewModel.Colors.Count > e.Position)
            {
                e.LabelStyle = new ChartAxisLabelStyle
                {
                    FontSize = 14,
                    TextColor = viewModel.Colors[(int)e.Position]
                };
            }
            else if (viewModel != null)
            {
                e.LabelStyle = new ChartAxisLabelStyle
                {
                    FontSize = 14,
                    TextColor = viewModel.Colors[0]
                };
            }
        }

        int previousDay;

        void InhibitorsAxisLabelCreated(object sender, ChartAxisLabelEventArgs e)
        {
            //if (Device.RuntimePlatform == "Android") return;

            //if (e.LabelContent != null)
            //{
            //    if (e.Position == 0)
            //    {
            //        previousDay = -1;
            //    }

            //    var date = DateTime.Parse(e.LabelContent);

            //    if (previousDay != date.Day || e.Position == 0)
            //    {
            //        e.LabelContent = date.ToString("MMM-dd hh:mm");
            //    }
            //    else
            //    {
            //        e.LabelContent = date.ToString("hh:mm");
            //    }
            //    previousDay = date.Day;
            //}
        }

        internal static int selectedIndex;

        void SfSegmentedControl_SelectionChanged(object sender, Syncfusion.XForms.Buttons.SelectionChangedEventArgs e)
        {
            switch (e.Index)
            {
                case 0:
                    selectedIndex = 0;
                    Inhibitors.IsVisible = false;
                    BarChart.IsVisible = true;
                    OverallChart.IsVisible = true;
                    OverallRow.Height = 100;
                    Title = AppResources.YourHappinessScore;
                    if (viewModel != null)
                    {
                        var text = viewModel.ResultDisplayText;
                        viewModel.ResultDisplayText = "";
                        viewModel.ResultDisplayText = text;
                    }
                    //Header.Text = string.Format(ReportPageViewModel.ReportText, AppResources.EnablersReport);
                    break;
                case 1:
                    //var result = Preferences.Get("IsFirstVisitToInhibitorReport", true);

                    //if (result)
                    //{
                    //    Xamarin.Forms.DependencyService.Resolve<IToast>().Show(AppResources.InhibitorTooltipForNewUser, 3.5);
                    //    Preferences.Set("IsFirstVisitToInhibitorReport", false);
                    //}
                    selectedIndex = 1;
                    Inhibitors.IsVisible = true;
                    BarChart.IsVisible = false;
                    OverallChart.IsVisible = false;
                    OverallRow.Height = 0;
                    Title = AppResources.YourInhibitorEffectScore;
                    //Header.Text = string.Format(ReportPageViewModel.ReportText, AppResources.InhibitorsReport);
                    if (viewModel != null)
                    {
                        var text = viewModel.ResultDisplayText;
                        viewModel.ResultDisplayText = "";
                        viewModel.ResultDisplayText = text;
                    }
                    break;
                default:
                    break;
            }
        }

        ReportPageViewModel viewModel;

        private async void SnedReport(object sender, EventArgs e)
        {
            if (!viewModel.CanDisplayChart) return;

            viewModel.IsBusy = true;

            Stream chartStream = null;
            string subject = "";

            if (BarChart.IsVisible)
            {
                BarChart.BackgroundColor = Color.White;
                chartStream = await BarChart.GetStreamAsync();
                BarChart.BackgroundColor = Color.Transparent;

                subject = "Enablers Report";
            }
            else if (Inhibitors.IsVisible)
            {
                Inhibitors.BackgroundColor = Color.White;
                chartStream = await Inhibitors.GetStreamAsync();
                Inhibitors.BackgroundColor = Color.Transparent;

                subject = "Inhibitors Report";
            }

            byte[] m_Bytes = ReadToEnd(chartStream);

            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Report.png");

            File.WriteAllBytes(path, m_Bytes);

            await SendEmail(path, subject);

            viewModel.IsBusy = false;
        }

        public async Task SendEmail(string fileName, string reportTyle)
        {
            try
            {
                var notes = "";

                if (viewModel != null && viewModel.InhibitorsLine != null && reportTyle == "Inhibitors Report")
                {
                    foreach (var item in viewModel.InhibitorsLine)
                    {
                        IGrouping<string, NegativeFactor> dataSource = item.ItemsSource as IGrouping<string, NegativeFactor>;
                        if (dataSource != null)
                        {
                            notes += $"\n\n";

                            var recentItem = dataSource.LastOrDefault();
                            notes += $"Inhibitor: {recentItem.Name}";
                            notes += $"\nDate time: {recentItem.Date:MMM-dd HH:mm}";
                            notes += $"\nScore: {recentItem.Value}/10";
                            notes += $"\nFeelings: {recentItem.Notes}";
                            notes += $"\nCauses: {recentItem.Causes}";
                            notes += $"\nWhat worked: {recentItem.Fixes}";
                        }
                    }
                }

                if (!File.Exists(fileName))
                {
                    await Application.Current.MainPage.DisplayAlert("", "No Report Found", AppResources.Ok);
                    return;
                }

                EmailAttachment emailAttachment = new EmailAttachment(fileName);

                var recordDurationText = "";
                if ((viewModel.EndDate - viewModel.StartDate).Days == 0)
                {
                    recordDurationText = $"on {viewModel.StartDate.ToString("MMM-dd")}";
                }
                else
                {
                    recordDurationText = $"for the period {viewModel.StartDate.ToString("MMM-dd")} to {viewModel.EndDate.ToString("MMM-dd")}";
                }

                DateTime dateTime = DateTime.Now;

                var message = new EmailMessage
                {
                    Subject = $"{viewModel.User.Name}'s {reportTyle} - {dateTime.ToString("MMM-dd hh:mm")}",
                    Body = $"Hi,\n\nPlease find attached the Report for {viewModel.User.Name}, generated {recordDurationText}.{notes}\n\nWarm regards,\nSan Habits Team",
                    Attachments = new System.Collections.Generic.List<EmailAttachment> {
                        emailAttachment
                    }
                };
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException)
            {
                await Application.Current.MainPage.DisplayAlert("", AppResources.EmailIsNotSupportedOnThisDevice, AppResources.Ok);
            }
            catch (Exception ex)
            {
                // Some other exception occurred
            }
        }

        public static byte[] ReadToEnd(System.IO.Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }
    }
}