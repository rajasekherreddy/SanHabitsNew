using System;
using System.Collections.Generic;
using HappinessIndex.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Linq;
using BuildHappiness.Core.Helpers;
using Rg.Plugins.Popup.Services;
using HappinessIndex.Views.Popup;

namespace HappinessIndex.ViewModels
{
    public class DashboardPageViewModel : ViewModelBase
    {
        public Command AttachImageCommand { get; set; }

        public Command NavigateCommand { get; set; }

        public string Email { get; set; }

        public string Name
        {
            get => name;
            set
            {
                name = value;
                NotifyPropertyChanged();
            }
        }

        private IList<Highlights> highlights;

        public IList<Highlights> Highlights
        {
            get => highlights;
            set
            {
                if (highlights == value) return;
                highlights = value;

                NotifyPropertyChanged();
            }
        }

        public DateTime MinimumDate
        {
            get => minimumDate; set
            {
                minimumDate = value;
                NotifyPropertyChanged();
            }
        }

        public DateTime MaximumDate
        {
            get => maximumDate; set
            {
                maximumDate = value;
                NotifyPropertyChanged();
            }
        }

        public DateTime SelectedDate
        {
            get => selectedDate; set
            {
                selectedDate = value;
                NotifyPropertyChanged();
            }
        }

        public DashboardPageViewModel()
        {
            NavigateCommand = new Command(Navigate);
            AttachImageCommand = new Command(AttachImage);

            Name = Preferences.Get(AppSettings.NameKey, string.Empty);
        }

        bool isPopup;
        private DateTime minimumDate;
        private DateTime maximumDate;
        private DateTime selectedDate;
        private string name;

        private async void AttachImage(object highlight)
        {
            IsBusy = true;

            isPopup = true;

            await JournalPageViewModel.UploadImage(highlight as Highlights);
            await DataService.UpdateHighlightsAsync(highlight as Highlights);
            OnAppearing();
            IsBusy = false;
        }

        private void Navigate(object parameter)
        {
            if (parameter != null)
            {
                Highlights highlights = parameter as Highlights;
                AppSettings.JournalDate = highlights.Date;
            }
            else
            {
                AppSettings.JournalDate = DateTime.Now;
            }

            App.GetShell().GoToAsync("//journal");
        }

        protected async override void OnAppearing()
        {
            if (isPopup)
            {
                isPopup = false;
                return;
            }

            var user = await DataService.GetUserAsync(Preferences.Get(AppSettings.EmailKey, ""));
            if (user != null)
            {
                Name = user.Name;
            }

            //AppSettings.JournalDate = DateTime.Now;

            var highlights = await DataService.GetHighlightsAsync();

            var startDate = Preferences.Get(AppSettings.StartDateKey, DateTime.Now.Date);

            var allData = new List<Highlights>();

            var today = DateTime.Now;
            do
            {
                var existingData = highlights.Where(item => item.Date.CompareDate(startDate)).FirstOrDefault();

                if (existingData != null)
                {
                    allData.Add(existingData);
                }
                else
                {
                    allData.Add(new Highlights() { Date = startDate });
                }

                startDate = startDate.AddDays(1);

            } while (startDate.Date <= today.Date);

            allData.Reverse();
            Highlights = allData;

            if (Highlights == null || Highlights.Count == 0)
            {
                Highlights = new List<Highlights> { new Highlights() { Date = DateTime.Today } };
            }

            MaximumDate = Highlights[0].Date.Date;
            MinimumDate = Highlights[Highlights.Count - 1].Date.Date;
            SelectedDate = MaximumDate;

            if (string.IsNullOrEmpty(Name))
            {
                var popup = new EnterName() { BindingContext = user };
                popup.SaveClicked += async (obj, arg) =>
                {
                    await DataService.UpdateUserAsync(user);
                    OnAppearing();
                };
                await PopupNavigation.Instance.PushAsync(popup);
            }
        }
    }
}