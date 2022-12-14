using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Input;
using HappinessIndex.Helpers;
using HappinessIndex.Models;
using HappinessIndex.Resx;
using HappinessIndex.Views;
using HappinessIndex.Views.Popup;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HappinessIndex.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        private string email;

        public List<string> EmailList { get; set; }

        private bool isInValidEmail = false;
        private bool canEnableLogin;
        private string password;

        public ICommand LoginCommand { get; set; }

        public ICommand SignupCommand { get; set; }

        public ICommand ResetPasswordCommand { get; set; }

        public bool IsInValidEmail
        {
            get => isInValidEmail;
            set
            {
                if (isInValidEmail == value) return;
                isInValidEmail = value;
                NotifyPropertyChanged();
            }
        }

        public bool CanEnableLogin
        {
            get => canEnableLogin;
            set
            {
                if (canEnableLogin == value) return;
                canEnableLogin = value;
                NotifyPropertyChanged();
            }
        }

        public string Email
        {
            get => email;
            set
            {
                if (email == value) return;
                email = value;

                if (string.IsNullOrEmpty(email))
                {
                    IsInValidEmail = false;
                }
                else
                {
                    IsInValidEmail = !EmailHelper.IsValid(email);
                }

                ValidateAndEnableLoginButton();
            }
        }

        public string Password
        {
            get => password;
            set
            {
                if (password == value) return;
                password = value;
                ValidateAndEnableLoginButton();
            }
        }

        private bool isRegisteredUser;

        public bool IsRegisteredUser
        {
            get => isRegisteredUser;
            set
            {
                if (isRegisteredUser == value) return;
                isRegisteredUser = value;
                NotifyPropertyChanged();
            }
        }

        public LoginPageViewModel()
        {
            LoginCommand = new Command(Login);
            SignupCommand = new Command(Signup);
            ResetPasswordCommand = new Command(ResetPassword);

            EmailList = new List<string>(Preferences.Get("email_list", string.Empty).Split(';'));

            if (EmailList.Contains(""))
            {
                EmailList.Remove("");
            }
        }

        protected async override void OnAppearing()
        {
            //if (EmailList.Count == 0)
            //{
                //await Application.Current.MainPage.DisplayAlert("", AppResources.NoUserFound, AppResources.Ok);
                //Application.Current.MainPage = new SignupPage();
            //}
            base.OnAppearing();
        }

        private void ValidateAndEnableLoginButton()
        {
            if (EmailHelper.IsValid(email) || !string.IsNullOrEmpty(Password))
            {
                CanEnableLogin = false;
            }
            else
            {
                CanEnableLogin = true;
            }
        }

        private async void Login()
        {
            IsBusy = true;

            IsInValidEmail = !EmailHelper.IsValid(Email);

            if (!IsInValidEmail)
            {

                var isRegisteredUser = await DataService.IsRegisteredUser(Email);

                if (isRegisteredUser && !IsRegisteredUser)
                {
                    IsRegisteredUser = true;
                    IsBusy = false;
                    return;
                }

                var user = await DataService.Login(email, password);

                if (!IsInValidEmail && user != null)
                {
                    SetPreferenses(user);

                    Application.Current.MainPage = new AppShell();
                }
                else
                {
                    //await Application.Current.MainPage.DisplayAlert("", AppResources.InvalidUserName, AppResources.Ok);
                    await PopupNavigation.Instance.PushAsync(new CommonMessage("", AppResources.InvalidUserName, AppResources.Ok));
                }
            }

            IsBusy = false;
        }

        public static void SetPreferenses(User user)
        {
            Preferences.Set(AppSettings.NameKey, user.Name);
            Preferences.Set(AppSettings.EmailKey, user.Email);
            Preferences.Set(AppSettings.UserIDKey, user.ID);
            Preferences.Set(AppSettings.StartDateKey, user.RegisteredDate.Date);
            Preferences.Set("show_welcome_screen", "false");
        }

        private void Signup()
        {
            //TODO: It has to be changed.
            Application.Current.MainPage = new SignupPage();
        }

        private async void ResetPassword()
        {
            if (isInValidEmail || string.IsNullOrEmpty(Email))
            {
                //Application.Current.MainPage.DisplayAlert("", AppResources.EnterYourEmailToReset, AppResources.Ok);
                await PopupNavigation.Instance.PushAsync(new CommonMessage("", AppResources.EnterYourEmailToReset, AppResources.Ok));
            }
            else
            {
                //await App.GetShell().GoToAsync($"resetpassword?email={Email},resetUsingPassword={false}");
                Application.Current.MainPage = new ResetPasswordPage() { Email = this.Email, ResetUsingPassword = "false" };
            }
        }
    }
}