using System;
using System.Threading.Tasks;
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
    public class SignupPageViewModel : ViewModelBase
    {
        public User User { get; set; }

        public ICommand SignupCommand { get; set; }

        public ICommand LoginCommand { get; set; }

        public SignupPageViewModel()
        {
            User = new User() { RegisteredDate = DateTime.Now };

            SignupCommand = new Command(Signup);
            LoginCommand = new Command(BackToLogin);
        }

        private async void Signup()
        {
            IsBusy = true;

            if (User.IsValidNameAndEmail())
            {
                await Signup(User.Email, User.Name, User);
            }

            IsBusy = false;
        }

        public static async Task Signup(string email, string name, User user = null, string profilePhoto = null, bool forceLogin = false, string loginMedium = "Local", string appleID = "")
        {
            if (user == null)
            {
                user = new User
                {
                    Name = name,
                    RegisteredDate = DateTime.Now,
                    Email = email,
                    ProfileURI = profilePhoto,
                    LoginMedium = loginMedium,
                    AppleID = appleID
                };
            }

            User previousUser;

            if (loginMedium == "Apple")
            {
                previousUser = await DataService.GetUserByAppleIDAsync(appleID);
            }
            else
            {
                previousUser = await DataService.GetUserAsync(user.Email);
            }

            if (previousUser == null && string.IsNullOrEmpty(email)) return;

            if (previousUser == null)
            {
                var result = await DataService.RegisterUserAsync(user);

                if (result)
                {
                    //var userChoice = await Application.Current.MainPage.DisplayActionSheet(AppResources.SuccessfullyRegistered, null, null, AppResources.GoToJournals, AppResources.EditProfile);
                    #region older 
                    //var userChoice = await Application.Current.MainPage.DisplayActionSheet(AppResources.SuccessfullyRegistered, null, null, AppResources.GoToJournals, AppResources.ViewAppTour);
                    //var email_list = Preferences.Get(AppSettings.EmailListKey, string.Empty) + ";" + user.Email;
                    //Preferences.Set(AppSettings.EmailListKey, email_list);

                    //LoginPageViewModel.SetPreferenses(user);

                    //if (userChoice == AppResources.GoToJournals)
                    //{
                    //    Application.Current.MainPage = new AppShell();
                    //}
                    //else
                    //{
                    //    AppSettings.JournalDate = DateTime.Now;
                    //    Application.Current.MainPage = new AppTourPage();
                    //    //Application.Current.MainPage = new AppShell();
                    //    //await AppShell.Current.GoToAsync("//profile");
                    //}
                    #endregion
                    var email_list = Preferences.Get(AppSettings.EmailListKey, string.Empty) + ";" + user.Email;
                    Preferences.Set(AppSettings.EmailListKey, email_list);
                    LoginPageViewModel.SetPreferenses(user);
                    Application.Current.MainPage = new AppShell();
                }
            }
            else if (forceLogin)
            {
                if (!string.IsNullOrEmpty(profilePhoto))
                {
                    previousUser.ProfileURI = profilePhoto;
                }

                LoginPageViewModel.SetPreferenses(previousUser);
                Application.Current.MainPage = new AppShell();
            }
            else
            {
                //await Application.Current.MainPage.DisplayAlert("", AppResources.EmailAlreadyRegistered, AppResources.Ok);
                await PopupNavigation.Instance.PushAsync(new CommonMessage("", AppResources.EmailAlreadyRegistered, AppResources.Ok));
            }
        }

        private void BackToLogin()
        {
            Application.Current.MainPage = new LoginPage();
        }
    }
}