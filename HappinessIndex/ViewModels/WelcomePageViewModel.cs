using System;
using System.Collections.Generic;
using HappinessIndex.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HappinessIndex.ViewModels
{
    public class WelcomePageViewModel : ViewModelBase
    {
        public Command NavigateCommand { get; set; }

        public WelcomePageViewModel()
        {
            NavigateCommand = new Command(Navigate);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();


        }

        public static void Navigate()
        {
            var email = Preferences.Get(AppSettings.EmailKey, string.Empty);

            if (string.IsNullOrEmpty(email))
            {
                var registeredEmails = new List<string>(Preferences.Get(AppSettings.EmailListKey, string.Empty).Split(';'));
                if (registeredEmails == null || registeredEmails.Count == 1)
                {
                    Application.Current.MainPage = new SignupPage();
                }
                else
                {
                    Application.Current.MainPage = new LoginPage();
                }
            }
            else
            {
                Application.Current.MainPage = new AppShell();
            }
        }
    }
}
