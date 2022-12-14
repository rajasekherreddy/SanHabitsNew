using System;
using System.Globalization;
using System.Threading.Tasks;
using HappinessIndex.Models;
using HappinessIndex.Resx;
using HappinessIndex.ViewModels;
using HappinessIndex.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HappinessIndex
{
    public partial class AppShell
    {
        public AppShell(bool isPopup = true)
        {
            InitializeComponent();
            RegisterRoute();
            if (isPopup)
                Init();
        }

        #region Workaround_https://github.com/xamarin/Xamarin.Forms/issues/7521

        private DateTime LastFlyoutHiddenUtcDateTime { get; set; }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (Device.RuntimePlatform == "Android")
            {
                if (propertyName == nameof(FlyoutIsPresented))
                {
                    if (!FlyoutIsPresented)
                    {
                        LastFlyoutHiddenUtcDateTime = DateTime.UtcNow;
                    }
                }
            }
        }

        protected override async void OnNavigating(ShellNavigatingEventArgs args)
        {
            if (Device.RuntimePlatform == "Android")
            {
                if (!WasNavigationCancelledToCloseFlyoutAndReRunAfterADelayToAvoidJitteryFlyoutCloseTransitionBug)
                {
                    // if the above value is true, then this is the re-run navigation from the GoToAsync(args.Target) call below - skip this block this second pass through, as the flyout is now closed
                    if ((DateTime.UtcNow - LastFlyoutHiddenUtcDateTime).TotalMilliseconds < 1000)
                    {
                        args.Cancel();

                        FlyoutIsPresented = false;

                        OnPropertyChanged(nameof(FlyoutIsPresented));

                        await Task.Delay(280);

                        WasNavigationCancelledToCloseFlyoutAndReRunAfterADelayToAvoidJitteryFlyoutCloseTransitionBug = true;

                        // re-run the originally requested navigation
                        await GoToAsync(args.Target);

                        return;
                    }
                }

                WasNavigationCancelledToCloseFlyoutAndReRunAfterADelayToAvoidJitteryFlyoutCloseTransitionBug = false;
            }
            base.OnNavigating(args);
        }

        private bool WasNavigationCancelledToCloseFlyoutAndReRunAfterADelayToAvoidJitteryFlyoutCloseTransitionBug = false;

        #endregion

        public void UpdateBindingContext(User user)
        {
            BindingContext = user;
        }

        private async void Init()
        {
            var user = await ViewModelBase.DataService.GetUserAsync(Preferences.Get(AppSettings.EmailKey, ""));

            if (user == null)
            {
                App.Current.MainPage = new LoginPage();
                return;
            }

            UpdateBindingContext(user);

            //Reset inhebitors for the day
            //var lastUserDate = Preferences.Get("LAST_USED_DATE", DateTime.MinValue);
            //if (lastUserDate.Date != DateTime.Now.Date && user != null)
            //{
            //    Preferences.Set("LAST_USED_DATE", DateTime.Now.Date);

            //    var defaultFactors = InhibitorsPageViewModel.GetDefaultNegativeFactors(user.ID);

            //    foreach (var data in defaultFactors)
            //    {
            //         Preferences.Set($"{user.ID}{data.Name}", 0);
            //         Preferences.Set($"{user.ID}{data.Name}Notes", "");
            //         Preferences.Set($"{user.ID}{data.Name}Causes", "");
            //         Preferences.Set($"{user.ID}{data.Name}Fixes", "");
            //    }
            //}

            App.Email = user.Email;
            await App.RegisterNotification(Preferences.Get(AppSettings.NameKey, string.Empty), new TimeSpan(7, 0, 0));
        }

        private void RegisterRoute()
        {
            Routing.RegisterRoute("resetpassword", typeof(ResetPasswordPage));
            Routing.RegisterRoute("lang", typeof(LangugeSelectionPage));
            //Routing.RegisterRoute("ServiceProvidersView", typeof(ServiceProvidersView));
            Routing.RegisterRoute("TherapyView", typeof(TherapyView));
            Routing.RegisterRoute("TherapyInsuranceView", typeof(TherapyInsuranceView));
            Routing.RegisterRoute("TherapyPreferencesView", typeof(TherapyPreferencesView));
            Routing.RegisterRoute("TherapistsView", typeof(TherapistsView));
            Routing.RegisterRoute("TherapySesstionView", typeof(TherapySesstionView));
            Routing.RegisterRoute("TherapySearchListView", typeof(TherapySearchListView));
        }

        void ShowAppTour(object sender, EventArgs e)
        {
            Application.Current.MainPage = new AppTourPage();
        }

        void NavigateChooseLanguage(object sender, EventArgs e)
        {
            //(App.Current.MainPage as NavigationPage).PushAsync(new LangugeSelectionPage());
            App.GetShell().GoToAsync("lang");
            this.FlyoutIsPresented = false;
        }
    }

    public class MarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString() == "LANGUAGES")
            {
                return new Thickness(13, 280, 2, 0);
            }
            return new Thickness(13, 10, 2, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ChartTooltipConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null) return "Notes";

            if (parameter.ToString() == "Notes")
            {
                return $"{ AppResources.Notes}: {value}";
            }
            else if (parameter.ToString() == "Fixes")
            {
                return $"{ AppResources.WhatHelped}: {value}";
            }
            else
            {
                return $"{ AppResources.Causes}: {value}";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
