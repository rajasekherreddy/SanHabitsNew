using System;
using System.Windows.Input;
using HappinessIndex.Models;
using HappinessIndex.Resx;
using HappinessIndex.Views;
using HappinessIndex.Views.Popup;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace HappinessIndex.ViewModels
{
    public class MicroWorkoutTimerViewModel : ViewModelBase
    {
        public bool ResetUsingPassword { get; set; }

        public User User { get; set; }

        public ICommand ResetPasswordCommand { get; set; }

        public ICommand ValidateCommand { get; set; }

        public ICommand LoginCommand { get; set; }

        private string securityQuestion;

        public string SecurityQuestion
        {
            get => securityQuestion;
            set
            {
                if (securityQuestion == value) return;
                securityQuestion = value;
                NotifyPropertyChanged();
            }
        }

        private bool isValidAnswer;

        public bool IsValidAnswer
        {
            get => isValidAnswer;
            set
            {
                if (isValidAnswer == value) return;
                isValidAnswer = value;
                NotifyPropertyChanged();
            }
        }

        private string securityAnswer;

        public string SecurityAnswer
        {
            get => securityAnswer;
            set
            {
                if (securityAnswer == value) return;
                securityAnswer = value;
                NotifyPropertyChanged();
            }
        }

        private bool isInvalidConfirmPassword;

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

        public string Password { get; set; }

        private string confirmPassword;

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
            }
        }

        public string Email { get; set; }

        public MicroWorkoutTimerViewModel(string email, bool resetUsingPassword)
        {
            ResetUsingPassword = resetUsingPassword;

            Email = email;
            RetrieveUser();

            ResetPasswordCommand = new Command(ResetPassword);
            LoginCommand = new Command(BackToLogin);
            ValidateCommand = new Command(ValidateAnswer);
        }

        private async void RetrieveUser()
        {
            User = await DataService.GetUserAsync(Email);
            if (User == null)
            {
                await Application.Current.MainPage.DisplayAlert("", AppResources.EmailNotRegistered, AppResources.Ok);
                Application.Current.MainPage = new LoginPage();
                return;
            }
            SecurityQuestion = User.SecurityQuestion;
        }

        private async void ResetPassword()
        {
            if (ResetUsingPassword)
            {
                if (!IsCorrectPassword())
                {
                    await Application.Current.MainPage.DisplayAlert("", "Wrong Old Password", AppResources.Ok);
                    return;
                }
            }
            else
            {
                if (!IsCorrectAnswer())
                {
                    await Application.Current.MainPage.DisplayAlert("", AppResources.InvalidAnswer, AppResources.Ok);
                    return;
                }
            }
            
            if (IsInvalidConfirmPassword || string.IsNullOrEmpty(Password))
            {
                await Application.Current.MainPage.DisplayAlert("", AppResources.InvalidPassword, AppResources.Ok);
                return;
            }

            User.Password = Password;

            var result = await DataService.UpdateUserAsync(User);

            if (result == 1)
            {
                await Application.Current.MainPage.DisplayAlert("", AppResources.PasswordResetSuccessfully, AppResources.Ok);
                Application.Current.MainPage = new LoginPage();
            }
        }

        private async void ValidateAnswer()
        {
            if (ResetUsingPassword)
            {
                if (IsCorrectPassword())
                {
                    IsValidAnswer = true;
                }
                else
                {
                    IsValidAnswer = false;
                    //Application.Current.MainPage.DisplayAlert("", "Wrong Old Password", AppResources.Ok);
                    await PopupNavigation.Instance.PushAsync(new CommonMessage("", "Wrong Old Password", AppResources.Ok));
                }
                return;
            }
            if (IsCorrectAnswer())
            {
                IsValidAnswer = true;
            }
            else
            {
                IsValidAnswer = false;
                //Application.Current.MainPage.DisplayAlert("", AppResources.InvalidAnswer, AppResources.Ok);
                await PopupNavigation.Instance.PushAsync(new CommonMessage("", AppResources.InvalidAnswer, AppResources.Ok));
            }
        }

        private bool IsCorrectAnswer()
        {
            return User != null && User.SecurityAnswer != null && SecurityAnswer != null
                && User.SecurityAnswer.ToLower() == SecurityAnswer?.ToLower();
        }

        private bool IsCorrectPassword()
        {
            return User != null && User.Password != null && SecurityAnswer != null
                && User.Password.ToLower() == SecurityAnswer?.ToLower();
        }

        protected override void ShowAppTour()
        {
            if (ResetUsingPassword && Application.Current.MainPage is NavigationPage)
            {
                (Application.Current.MainPage as NavigationPage).PopAsync();
            }
            else
            {
                base.ShowAppTour();
            }
        }

        private void BackToLogin()
        {
            if (ResetUsingPassword && Application.Current.MainPage is NavigationPage)
            {
                (Application.Current.MainPage as NavigationPage).PopAsync();
            }
            else
            {
                Application.Current.MainPage = new LoginPage();
            }
        }
    }
}