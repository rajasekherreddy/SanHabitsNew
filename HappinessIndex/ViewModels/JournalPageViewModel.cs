using System.Collections.Generic;
using HappinessIndex.Models;
using Xamarin.Forms;
using System.Linq;
using Xamarin.Essentials;
using Plugin.Media;
using System.IO;
using Plugin.Media.Abstractions;
using HappinessIndex.Helpers;
using HappinessIndex.Resx;
using HappinessIndex.DependencyService;
using System.Threading.Tasks;
using System;
using Rg.Plugins.Popup.Services;
using HappinessIndex.Views.Popup;

namespace HappinessIndex.ViewModels
{
    public class JournalPageViewModel : ViewModelBase
    {
        public Command SubmitCommand { get; set; }

        public Command ShowFactorsListCommand { get; set; }

        public Command UploadImageCommand { get; set; }

        public User User { get; set; }

        private Highlights highlights;

        public Highlights Highlights
        {
            get => highlights;
            set
            {
                if (highlights == value) return;
                highlights = value;

                NotifyPropertyChanged();
            }
        }

        private IList<Factor> factors;

        public IList<Factor> Factors
        {
            get => factors;
            set
            {
                if (factors == value) return;
                factors = value;

                NotifyPropertyChanged();
            }
        }

        private List<Journal> records;
        private List<OveralScore> overall;

        public List<Journal> Records
        {
            get => records;
            set
            {
                if (records == value) return;
                records = value;

                NotifyPropertyChanged();
            }
        }

        public List<OveralScore> Overall
        {
            get => overall;
            set
            {
                if (overall == value) return;
                overall = value;

                NotifyPropertyChanged();
            }
        }

        public JournalPageViewModel()
        {
            SubmitCommand = new Command(SubmitJournal);
            ShowFactorsListCommand = new Command(ShowFactorsListPage);
            UploadImageCommand = new Command(UploadImage);

            Init();
        }

        List<Factor> allFactors;

        private async void Init()
        {
            IsBusy = true;

            await CrossMedia.Current.Initialize();

            IsBusy = false;

            if (forceLoding)
            {
                OnAppearing();
            }
        }

        private async void UploadImage()
        {
            IsBusy = true;
            await UploadImage(Highlights);

            foreach (var journal in Records)
            {
                if (journal.ActualValue != 0)
                {
                    await SubmitJournal(true);
                    break;
                }
            }

            IsBusy = false;
        }

        public static async Task UploadImage(Highlights highlights)
        {
            var mediaFile = await PhotoPicker.Pick();

            if (mediaFile == null) return;

            highlights.pause = true;
            using (FileStream stream = File.Open(mediaFile.Path, FileMode.Open))
            {
                highlights.Photo = new byte[stream.Length];
                stream.ReadAsync(highlights.Photo, 0, (int)stream.Length).Wait();
            }
            highlights.pause = false;
            //highlights.NotifyPropertyChanged("Photo");

            await DataService.UpdateHighlightsAsync(highlights);

            Xamarin.Forms.DependencyService.Get<IToast>().Show(AppResources.UploadedSuccessfully, 1.5);
        }

        private async void ShowFactorsListPage()
        {
            await App.GetShell().GoToAsync("//factors", true);
        }

        private async void SubmitJournal()
        {
            await SubmitJournal(false);
        }

        private async Task SubmitJournal(bool internalRequest)
        {
            //if (IsBusy && !internalRequest) return;
            try
            {
                IsBusy = true;

                var result = await DataService.UpdateJounralsAsync(Records);

                result += await DataService.UpdateOverallScoreAsync(Overall[0]);

                App.CancelNotification(AppSettings.JournalDate);

                IsBusy = false;

                if (!internalRequest)
                {
                    await PopupNavigation.Instance.PushAsync(new CommonMessage("", AppResources.SavedSuccessfully, AppResources.Ok));
                }
            }
            catch (Exception e)
            {

            }
        }

        bool forceLoding;

        protected async override void OnAppearing()
        {
            if (IsBusy)
            {
                forceLoding = true;
                return;
            }

            IsBusy = true;

            //if (User == null)
            {
                User = await DataService.GetUserAsync(Preferences.Get(AppSettings.EmailKey, ""));
            }

            Highlights = await DataService.GetHighlightsAsync(AppSettings.JournalDate);

            if (Highlights == null)
            {
                Highlights = new Highlights() { Date = AppSettings.JournalDate };
            }

            var actualRecords = await DataService.GetJournalsAsync(AppSettings.JournalDate);

            if (actualRecords == null)
            {
                actualRecords = new List<Journal>();
            }

            //if (allFactors == null)
            {
                allFactors = await DataService.GetAllFactors(User);
            }

            //TODO: Consider logic change
            Factors = User.GetSelectedFactors(allFactors);

            var date = AppSettings.JournalDate;

            var overallJournal = await DataService.GetOverallScoreAsync(date);

            if (overallJournal == null)
            {
                overallJournal = new OveralScore();
                overallJournal.ActualValue = 0;
                overallJournal.Date = date;
            }

            Overall = new List<OveralScore> { overallJournal };

            List<Journal> records = new List<Journal>();

            bool hasPreviousEntry = false;
            bool hasMissingEntry = false;

            string missingRecordsText = string.Empty;

            var isOlderDate = date.Date != DateTime.Now.Date;

            //Add all enabled factors only for today.
            PullJournalForToday(actualRecords, date, records, ref hasPreviousEntry, ref hasMissingEntry, ref missingRecordsText, isOlderDate);

            //Add missing journal that is not enabled for older date.
            PullJournalForPreviousDates(actualRecords, records, isOlderDate);

            //Older date which does not have journal entry.
            if (isOlderDate && records.Count == 0)
            {
                PullJournalForToday(actualRecords, date, records, ref hasPreviousEntry, ref hasMissingEntry, ref missingRecordsText, false);
            }

            Records = records;

            if (hasMissingEntry && hasPreviousEntry && !isOlderDate)
            {
                IsBusy = false;
                //await Application.Current.MainPage.DisplayAlert("", AppResources.TheJournalEntryFor + missingRecordsText + AppResources.IsMissing, AppResources.Ok);
                await PopupNavigation.Instance.PushAsync(new CommonMessage("", AppResources.TheJournalEntryFor + missingRecordsText + AppResources.IsMissing, AppResources.Ok));
            }

            IsBusy = false;
        }

        private void PullJournalForPreviousDates(List<Journal> actualRecords, List<Journal> records, bool isOlderDate)
        {
            if (isOlderDate)
            {
                foreach (var journal in actualRecords)
                {
                    if (!records.Contains(journal))
                    {
                        var factor = allFactors.Where(item => item.ID == journal.FactorID).FirstOrDefault();
                        journal.Factor = factor;
                        journal.Icon = factor.Icon;
                        records.Add(journal);
                    }
                }
            }
        }

        private void PullJournalForToday(List<Journal> actualRecords, DateTime date, List<Journal> records, ref bool hasPreviousEntry, ref bool hasMissingEntry, ref string missingRecordsText, bool isOlderDate)
        {
            if (!isOlderDate)
            {
                foreach (var factor in Factors)
                {
                    var entry = actualRecords.Where(item => item.FactorID == factor.ID).FirstOrDefault();

                    Journal journal = null;

                    if (entry != null)
                    {
                        hasPreviousEntry = true;

                        journal = entry;
                        journal.Factor = factor;
                        journal.Icon = factor.Icon;
                    }

                    if (journal == null)
                    {
                        hasMissingEntry = true;

                        if (!string.IsNullOrEmpty(missingRecordsText))
                        {
                            missingRecordsText += ", ";
                        }

                        missingRecordsText += factor.Name;

                        journal = new Journal
                        {
                            FactorID = factor.ID,
                            ActualValue = 0,
                            Icon = factor.Icon,
                            Date = date,
                            Factor = factor
                        };
                    }
                    records.Add(journal);
                }
            }
        }
    }
}