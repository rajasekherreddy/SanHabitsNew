using System;
using HappinessIndex.Models;
using HappinessIndex.Resx;
using HappinessIndex.Views.Popup;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HappinessIndex.ViewModels
{
    public class NotesPageViewModel : ViewModelBase
    {
        public User User { get; set; }

        public Command SaveHighlightsCommand { get; set; }

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

        public NotesPageViewModel()
        {
            SaveHighlightsCommand = new Command(SaveHighlights);
        }

        private async void SaveHighlights()
        {
            IsBusy = true;
            await DataService.UpdateHighlightsAsync(Highlights);
            IsBusy = false;

            await PopupNavigation.Instance.PushAsync(new CommonMessage("", AppResources.SavedSuccessfully, AppResources.Ok));
        }

        protected async override void OnAppearing()
        {
            IsBusy = true;

            Highlights = await DataService.GetHighlightsAsync(AppSettings.JournalDate);

            if (Highlights == null)
            {
                Highlights = new Highlights() { Date = AppSettings.JournalDate };
            }

            base.OnAppearing();

            IsBusy = false;
        }
    }
}
