using System;
using System.Windows.Input;
using HappinessIndex.Models;
using HappinessIndex.Resx;
using HappinessIndex.Views;
using HappinessIndex.Views.Popup;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HappinessIndex.ViewModels
{
    public class ProfilePageViewModel : ViewModelBase
    {
        public Command SaveCommand { get; set; }

        public bool HasEmptyPassword
        {
            get => hasEmptyPassword;
            set
            {
                hasEmptyPassword = value;
                NotifyPropertyChanged();
            }
        }

        private User user;
        private bool hasEmptyPassword;

        public User User
        {
            get => user;
            set
            {
                if (user == value) return;
                user = value;

                NotifyPropertyChanged();
            }
        }

        public ProfilePageViewModel()
        {
            SaveCommand = new Command(SaveProfile);
            HomeCommand = new Command(HomeFactor);

            Init();

        }

        public Command HomeCommand { get; set; }
        private async void HomeFactor(object parameter)
        {
            try
            {
                await App.GetShell().GoToAsync("//main");
            }
            catch (Exception ex)
            {

            }
        }

        private async void Init()
        {
            var email = Preferences.Get(AppSettings.EmailKey, "");
            User = await DataService.GetUserAsync(email);

            if (User != null && !string.IsNullOrEmpty(User.Password))
            {
                User.ConfirmPassword = User.Password;
                HasEmptyPassword = false;
            }
            else
            {
                HasEmptyPassword = true;
            }
        }

        private async void SaveProfile()
        {
            IsBusy = true;

            if (user.IsValid())
            {
                (App.GetShell() as AppShell).UpdateBindingContext(user);

                await DataService.UpdateUserDataAsync(user);

                await PopupNavigation.Instance.PushAsync(new CommonMessage("", AppResources.SavedSuccessfully, AppResources.Ok));
            }

            IsBusy = false;
        }
    }
}