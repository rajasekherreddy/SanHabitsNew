using System;
using System.Threading.Tasks;
using BuildHappiness.Core.Helpers;
using HappinessIndex.Models;
using HappinessIndex.Resx;
using HappinessIndex.Views.Popup;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HappinessIndex.ViewModels
{
    public class PersonalNoteViewModel : ViewModelBase
    {
        public Command SaveCommand { get; set; }

        public Command ValidatePasswordCommand { get; set; }

        private DateTime selectedDate = DateTime.Now;

        public DateTime SelectedDate
        {
            get => selectedDate;
            set
            {
                if (selectedDate.CompareDate(value)) return;

                selectedDate = value;
                UpdateNote(false);
            }
        }

        private string note;

        public string Note
        {
            get => note;
            set
            {
                if (note == value) return;

                note = value;
                NotifyPropertyChanged();
            }
        }

        private bool isEnabled;

        public bool IsEnabled
        {
            get => isEnabled;
            set
            {
                if (isEnabled == value) return;

                isEnabled = value;
                NotifyPropertyChanged();
            }
        }

        private double opacity = 0.5;

        public double Opacity
        {
            get => opacity;
            set
            {
                if (opacity == value) return;

                opacity = value;
                NotifyPropertyChanged();
            }
        }

        private async void UpdateNote(bool needValidation = true)
        {
            Note = string.Empty;

            if (needValidation || user == null)
            {
                await ValidateUser();
            }
            else
            {
                await RetriveNote();
            }
        }

        private async void ValidatePassword(object password)
        {
            if (password != null && user != null && user.Password == password.ToString())
            {
                Opacity = 1;
                IsEnabled = true;
                await RetriveNote();
            }
            else
            {
                IsEnabled = false;
                Opacity = 0.5;

                await Application.Current.MainPage.DisplayAlert("", AppResources.InvalidInputPassword, AppResources.Ok);
                await PopupNavigation.Instance.PushAsync(new PasswordInput() { BindingContext = this });
            }
        }

        private async Task RetriveNote()
        {
            PersonalNote note = await DataService.GetPersonalNoteAsync(selectedDate);
            if (note != null)
            {
                Note = note.Note;
            }
            else
            {
                Note = "";
            }
        }

        public PersonalNoteViewModel()
        {
            SaveCommand = new Command(SaveNote);
            ValidatePasswordCommand = new Command(ValidatePassword);
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await Task.Delay(400);
            UpdateNote();
        }

        User user;

        private async Task ValidateUser()
        {
            user = await DataService.GetUserAsync(Preferences.Get(AppSettings.EmailKey, ""));
            if (string.IsNullOrEmpty(user.Password))
            {
                IsEnabled = false;
                Opacity = 0.5;
                await Application.Current.MainPage.DisplayAlert("", AppResources.SetYourPassword, AppResources.SetPassword);
                await App.GetShell().GoToAsync("//profile");
            }
            else
            {
                await PopupNavigation.Instance.PushAsync(new PasswordInput() { BindingContext = this });
            }
        }

        private async void SaveNote()
        {
            var result = await DataService.SetPersonalNoteAsync(selectedDate, Note);

            if (result == 1)
            {
                await Application.Current.MainPage.DisplayAlert("", AppResources.SavedSuccessfully, AppResources.Ok);
                await App.GetShell().GoToAsync("//home");
            }
        }
    }
}