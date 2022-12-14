using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using HappinessIndex.Data;
using HappinessIndex.DependencyService;
using HappinessIndex.Helpers;
using HappinessIndex.Views;
using Plugin.FacebookClient;
using Plugin.GoogleClient;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HappinessIndex.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public static IDataService DataService { get; set; } = Xamarin.Forms.DependencyService.Resolve<IDataService>();

        public static ICloudService CloudService { get; set; } = Xamarin.Forms.DependencyService.Resolve<ICloudService>();

        public bool IsAppleSignInAvailable { get { return appleSignInService?.IsAvailable ?? false; } }

        IAppleSignInService appleSignInService;

        public Command NavigateBackCommand { get; set; }

        public Command ShowAppTourCommand { get; set; }

        public Command RefreshCommand { get; set; }

        public Command DisappearingCommand { get; set; }

        public Command LoginWithFacebookCommand { get; set; }

        public Command LoginWithGoogleCommand { get; set; }

        public Command SignInWithAppleCommand { get; set; }

        private bool isBusy;

        public bool IsBusy
        {
            get => isBusy;
            set
            {
                if (isBusy == value) return;
                isBusy = value;
                NotifyPropertyChanged();
            }
        }
        private string title;
        public string Title
        {
            get { return title; }
            set
            {

                if (title == value) return;
                title = value;
                NotifyPropertyChanged();
            }
        }

        public ViewModelBase()
        {
            NavigateBackCommand = new Command(NavigateBack);
            RefreshCommand = new Command(Refresh);
            DisappearingCommand = new Command(OnDisappearing);
            ShowAppTourCommand = new Command(ShowAppTour);
            LoginWithFacebookCommand = new Command(LoginWithFacebook);
            LoginWithGoogleCommand = new Command(LoginWithGoogle);
            SignInWithAppleCommand = new Command(OnAppleSignInRequest);

            if (Device.RuntimePlatform == "iOS")
            {
                appleSignInService = Xamarin.Forms.DependencyService.Get<IAppleSignInService>();
            }
        }

        protected virtual void ShowAppTour()
        {
            Application.Current.MainPage = new AppTourPage();
        }

        async void OnAppleSignInRequest()
        {
            IsBusy = true;
            await SocialLogin.LoginAppleAsync(appleSignInService);
            IsBusy = false;
        }

        IGoogleClientManager googleService = CrossGoogleClient.Current;
        private async void LoginWithGoogle()
        {
            IsBusy = true;
            await SocialLogin.LoginGoogleAsync(googleService);
            IsBusy = false;
        }

        IFacebookClient facebookService = CrossFacebookClient.Current;
        private async void LoginWithFacebook()
        {
            IsBusy = true;
            await SocialLogin.LoginFacebookAsync(facebookService);
            IsBusy = false;
        }

        private void NavigateBack()
        {
            //TODD:
            //Application.Current.MainPage = new DashboardPage();
        }

        private void Refresh()
        {
            OnAppearing();
        }

        protected virtual void OnAppearing()
        {

        }


        protected virtual void OnDisappearing()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}