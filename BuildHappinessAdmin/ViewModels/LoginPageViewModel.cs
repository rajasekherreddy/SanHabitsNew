using System;
using System.Windows.Input;
using BuildHappinessAdmin.Views;
using Xamarin.Forms;

namespace BuildHappinessAdmin.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        public string Username { get; set; } = "sg5DKcC65DyC@buildhappiness.app";

        public string Password { get; set; }

        public ICommand LoginCommand { get; set; }

        public LoginPageViewModel()
        {
            LoginCommand = new Command(Login);
        }

        private async void Login()
        {
            var result = await DataService.Login(Username, Password);

            if (result == 1)
            {
                App.Current.MainPage = new ServiceProviderRequestPage();
            }
        }
    }
}