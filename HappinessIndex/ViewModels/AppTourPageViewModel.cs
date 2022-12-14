using System;
using System.Collections.Generic;
using HappinessIndex.Resx;
using HappinessIndex.Views;
using Xamarin.Forms;

namespace HappinessIndex.ViewModels
{
    public class AppTourPageViewModel : ViewModelBase
    {
        public Command CloseCommand { get; set; }

        public Command GetStartedCommand { get; set; }

        public List<TourItem> Source { get; set; }

        public AppTourPageViewModel()
        {
            Source = new List<TourItem>();

            Source.Add(new TourItem
            {
                Title = AppResources.HomeAppTour,
                Description = AppResources.DashboardContent,
                Image = "Dashboard.png"
            });

            Source.Add(new TourItem
            {
                Title = AppResources.FactorsList,
                Description = AppResources.FactorsListContent,
                Image = "Factors.png"
            });

            Source.Add(new TourItem
            {
                Title = AppResources.JournalAppTourTitle,
                Description = AppResources.HappinessJournalEntryContent,
                Image = "Journal.png"
            });

            Source.Add(new TourItem
            {
                Title = AppResources.NotesAppTourTitle,
                Description = AppResources.NotesAppTourContent,
                Image = "Notes.png"
            });

            Source.Add(new TourItem
            {
                Title = AppResources.HappinessJournalHighlights,
                Description = AppResources.HappinessJournalHighlightsContent,
                Image = "AppTourInhibitors.png"
            });

            Source.Add(new TourItem
            {
                Title = AppResources.ReportsAppTourTitle,
                Description = AppResources.ReportsContent,
                Image = "Report.png"
            });

            Source.Add(new TourItem
            {
                Title = AppResources.InhibitorsReportAppTourTitle,
                Description = AppResources.InhibitorsReportAppTourContent,
                Image = "AppTourInhibitorsReport.png"
            });

            Source.Add(new TourItem
            {
                Title = AppResources.PersonalNoteAppTour,
                Description = AppResources.PersonalNoteAppTourContent,
                Image = "PersonalNote.png"
            });

            //Source.Add(new TourItem
            //{
            //    Title = AppResources.SettingsAppTour,
            //    Description = AppResources.SettingsAppTourContent,
            //    Image = "Settings.png"
            //});

            Source.Add(new TourItem
            {
                Title = AppResources.ComingSoon,
                Description = AppResources.ComingSoonContent,
                Image = "Phase2_1.png"
            });

            CloseCommand = new Command(Close);
            GetStartedCommand = new Command(NavigateToGetStarted);
        }

        private void Close()
        {
            Application.Current.MainPage = new WelcomePage();
        }

        private void NavigateToGetStarted()
        {
            WelcomePageViewModel.Navigate();
        }
    }

    public class TourItem
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }
    }
}