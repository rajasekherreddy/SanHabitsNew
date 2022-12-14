using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using HappinessIndex.Data;
using HappinessIndex.Helpers;
using HappinessIndex.Resx;
using HappinessIndex.Views;
using SQLite;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HappinessIndex.Models
{
    public class User : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public static IDataService DataService { get; set; } = Xamarin.Forms.DependencyService.Resolve<IDataService>();


        private bool isInvalidEmail;
        private bool isInvalidName;
        private string confirmPassword;
        private bool isInvalidQuestion;
        private bool isInvalidPassword;
        private bool isInvalidAnswer;
        private bool isInvalidConfirmPassword;

        public string AppleID { get; set; }

        private int age;

        [Ignore]
        public ICommand LogoutCommand { get; set; }
        [Ignore]
        public ICommand RemoveCommand { get; set; }

        private string name;
        public string Name
        {
            get => name;
            set
            {
                if (name == value) return;
                name = value;
                NotifyPropertyChanged();
                IsInvalidName = string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);
            }
        }

        private string email;
        public string Email
        {
            get => email;
            set
            {
                if (email == value) return;
                email = value;
                IsInvalidEmail = !EmailHelper.IsValid(email);
            }
        }

        public DateTime RegisteredDate { get; set; }

        public string SelectedFactors { get; set; } = "1, 2, 3, 4,";

        public string CustomFactors { get; set; }
       // public string CustomFactorsMicroWorkout { get; set; }

        // public string CustomFactorsMicroWorkout { get; set; }

        public string Phone { get; set; }

        public int Age
        {
            get => age;
            set
            {
                if (age == value) return;
                age = value;
            }
        }

        public string Country { get; set; }

        public string Gender { get; set; }

        public string Password { get; set; }

        public string SecurityQuestion { get; set; }

        public string SecurityAnswer { get; set; }

        [Ignore]
        public string ConfirmPassword
        {
            get => confirmPassword;
            set
            {
                if (confirmPassword == value) return;
                if (value != Password)
                {
                    IsInvalidConfirmPassword = true;
                }
                else
                {
                    IsInvalidConfirmPassword = false;
                }
                confirmPassword = value;

                NotifyPropertyChanged();
            }
        }

        [Ignore]
        public bool IsInvalidConfirmPassword
        {
            get => isInvalidConfirmPassword;
            set
            {
                if (isInvalidConfirmPassword == value) return;
                isInvalidConfirmPassword = value;
                NotifyPropertyChanged();
            }
        }

        [Ignore]
        public bool IsInvalidPassword
        {
            get => isInvalidPassword;
            set
            {
                if (isInvalidPassword == value) return;
                isInvalidPassword = value;
                NotifyPropertyChanged();
            }
        }

        [Ignore]
        public bool IsInvalidQuestion
        {
            get => isInvalidQuestion;
            set
            {
                if (isInvalidQuestion == value) return;
                isInvalidQuestion = value;
                NotifyPropertyChanged();
            }
        }

        [Ignore]
        public bool IsInvalidAnswer
        {
            get => isInvalidAnswer;
            set
            {
                if (isInvalidAnswer == value) return;
                isInvalidAnswer = value;
                NotifyPropertyChanged();
            }
        }

        [Ignore]
        public bool IsInvalidEmail
        {
            get => isInvalidEmail;
            set
            {
                if (isInvalidEmail == value) return;
                isInvalidEmail = value;
                NotifyPropertyChanged();
            }
        }

        [Ignore]
        public bool IsInvalidName
        {
            get => isInvalidName;
            set
            {
                if (isInvalidName == value) return;
                isInvalidName = value;
                NotifyPropertyChanged();
            }
        }

        public string ProfileURI { get; set; }

        public string LoginMedium { get; set; } = "Local";

        public User()
        {
            LogoutCommand = new Command(Logout);
            RemoveCommand = new Command(RemoveAccount);

        }

        private async void Logout()
        {
            //Application.Current.MainPage.DisplayPromptAsync("", "Close app and stay logged-in", "")

            var result = await Application.Current.MainPage.DisplayActionSheet(AppResources.CloseAppAndStayLogged_in, AppResources.Cancel, null, AppResources.Yes, AppResources.NoLogMeOut);

            if (result == AppResources.Yes)
            {
                Environment.Exit(0);
            }
            else if (result == AppResources.NoLogMeOut)
            {
                Preferences.Set(AppSettings.NameKey, string.Empty);
                Preferences.Set(AppSettings.EmailKey, string.Empty);

                Application.Current.MainPage = new LoginPage();
            }
        }

        private async void RemoveAccount()
        {
            //Application.Current.MainPage.DisplayPromptAsync("", "Close app and stay logged-in", "")

            var result = await Application.Current.MainPage.DisplayActionSheet(AppResources.ConfirmRemoveAccount, AppResources.Cancel, null, AppResources.NoRemoveAccount);

            if (result == AppResources.Yes)
            {
                Environment.Exit(0);
            }
            else if (result == AppResources.NoRemoveAccount)
            {
            //    Preferences.Set(AppSettings.NameKey, string.Empty);
            //    Preferences.Set(AppSettings.EmailKey, string.Empty);

                Preferences.Remove(AppSettings.NameKey, string.Empty);
                Preferences.Remove(AppSettings.EmailKey, string.Empty);
                Preferences.Clear();

                DataService.deleteAllUsersAsync();

                Application.Current.MainPage = new LoginPage();
            }
        }

        public bool IsValid()
        {
            IsInvalidName = string.IsNullOrEmpty(Name) || string.IsNullOrWhiteSpace(Name);
            IsInvalidEmail = !Helpers.EmailHelper.IsValid(Email);

            IsInvalidPassword = false;
            IsInvalidConfirmPassword = false;
            IsInvalidQuestion = false;
            IsInvalidAnswer = false;

            if (IsInvalidName || IsInvalidEmail)
            {
                return false;
            }

            if ((string.IsNullOrEmpty(Password) && string.IsNullOrEmpty(ConfirmPassword) &&
                string.IsNullOrEmpty(SecurityQuestion) && string.IsNullOrEmpty(SecurityAnswer)) ||
                !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(ConfirmPassword) &&
                !string.IsNullOrEmpty(SecurityQuestion) && !string.IsNullOrEmpty(SecurityAnswer) && Password == ConfirmPassword)
            {

                return true;
            }

            if (string.IsNullOrEmpty(Password))
            {
                IsInvalidPassword = true;
            }
            if (Password != ConfirmPassword)
            {
                IsInvalidConfirmPassword = true;
            }
            if (string.IsNullOrEmpty(SecurityQuestion))
            {
                IsInvalidQuestion = true;
            }

            if (string.IsNullOrEmpty(SecurityAnswer))
            {
                IsInvalidAnswer = true;
            }

            return false;
        }

        public bool IsValidNameAndEmail()
        {
            IsInvalidEmail = !Helpers.EmailHelper.IsValid(Email);
            IsInvalidName = string.IsNullOrEmpty(name);

            return !string.IsNullOrEmpty(name) && !IsInvalidEmail;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}