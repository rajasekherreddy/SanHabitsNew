using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using HappinessIndex.Data;
using HappinessIndex.Data.CloudData;
using HappinessIndex.Data.LocalData;
using HappinessIndex.DependencyService;
using HappinessIndex.Resx;
using HappinessIndex.ViewModels;
using HappinessIndex.Views;
using HappinessIndex.Views.Popup;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Plugin.FirebasePushNotification;
using Plugin.LocalNotification;
using Rg.Plugins.Popup.Services;
using SQLite;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HappinessIndex
{
    public partial class App : Application
    {
        public static string LoginType { get; set; }

        public static bool Flag_Testing = false;

        public App()
        {
            Xamarin.Forms.Device.SetFlags(new string[] { "MediaElement_Experimental","Shapes_Experimental", "Expander_Experimental", "RadioButton_Experimental", "SwipeView_Experimental" });

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzQzNDg0QDMxMzgyZTMzMmUzMFhEOHZmWFdXa1JKbytiM3I4eEpTQS95YldLNW84bUQ5aHg5OHdhMHhHV3M9");
            AppCenter.Start("ios=e5cf8c7e-69ba-471a-b98e-d1a2efeacc9c;android=570c69e0-cb0f-4334-abec-e383a03c1eea", typeof(Crashes), typeof(Analytics));

            SettingsPage.ChangeLanguage(Preferences.Get(AppSettings.LanguageKey, "English"));

            InitializeComponent();

            if (Preferences.Get(AppSettings.IsEnabledLightBackgroundKey, false))
            {
                Current.Resources["BackgroundColor"] = Color.White;
            }

            //CrossFirebasePushNotification.Current.RegisterForPushNotifications();
            //CrossFirebasePushNotification.Current.OnTokenRefresh += Current_OnTokenRefresh;
            //CrossFirebasePushNotification.Current.OnNotificationReceived += Current_OnNotificationReceived;

            Init();
            //ValidateAppleSignInService();
            ValidateNewUser();
        }

        private void Current_OnNotificationReceived(object source, FirebasePushNotificationDataEventArgs e)
        {

        }

        private void Current_OnTokenRefresh(object source, FirebasePushNotificationTokenEventArgs e)
        {

        }

        private void Init()
        {
            Xamarin.Forms.DependencyService.Register<IDataService, LocalDataService>();
            Xamarin.Forms.DependencyService.Register<ICloudService, FirebaseService>();
            AppSettings.JournalDate = DateTime.Now;
        }

        private async void ValidateAppleSignInService()
        {
            var appleSignInService = Xamarin.Forms.DependencyService.Get<IAppleSignInService>();
            if (appleSignInService != null)
            {
                var userId = await SecureStorage.GetAsync("AppleUserIdKey");

                if (appleSignInService.IsAvailable && !string.IsNullOrEmpty(userId))
                {

                    var credentialState = await appleSignInService.GetCredentialStateAsync(userId);

                    switch (credentialState)
                    {
                        case AppleSignInCredentialState.Authorized:
                            //Do logic to navigate to app shell.
                            break;
                        case AppleSignInCredentialState.NotFound:
                        case AppleSignInCredentialState.Revoked:
                            //Logout;
                            SecureStorage.Remove("AppleUserIdKey");
                            MainPage = new LoginPage();
                            break;
                    }
                }
            }
        }

        internal static Shell GetShell()
        {
            return Shell.Current;
        }

        public static void RestartApp()
        {
            Application.Current.MainPage = new AppShell();
        }

        private async Task ValidateNewUser()
        {
            var newUser = Preferences.Get("new_user", true);

            if (newUser)
            {
                MainPage = new WelcomePage();

                Preferences.Set(AppSettings.PreferredNotificationTimeKey, TimeSpan.FromHours(19).ToString());

                //Default light theme
                EnableLightTheme(false);

                #region Backup
                if (Xamarin.Forms.Device.RuntimePlatform == "iOS")
                {
                    //var allowBackup = await MainPage.DisplayAlert(AppResources.iCloucBackupPermission, "", AppResources.Yes, AppResources.No);

                    //if (allowBackup)
                    //{
                    //    await PopupNavigation.Instance.PushAsync(new BackUp());

                    //    Preferences.Set(AppSettings.EnableBackupKey, true);

                    //    await Xamarin.Forms.DependencyService.Resolve<IBackUp>().Fetch();

                    //    //ViewModelBase.DataService.InvalidateConnection();

                    //    var allUsers = await ViewModelBase.DataService.GetAllUsersAsync();

                    //    var emailList = "";
                    //    foreach (var user in allUsers)
                    //    {
                    //        emailList += user.Email + ";";
                    //    }

                    //    Preferences.Set(AppSettings.EmailListKey, emailList);

                    //    await PopupNavigation.Instance.PopAllAsync();
                    //}
                    //else
                    {
                        Preferences.Set(AppSettings.EnableBackupKey, false);
                    }
                    #endregion
                }
                Preferences.Set("new_user", false);
            }
            else
            {
                WelcomePageViewModel.Navigate();
            }
        }

        public static void EnableLightTheme(bool restart = true)
        {
            if (Preferences.Get(AppSettings.IsEnabledLightBackgroundKey, false)) return;
            Application.Current.Resources["BackgroundColor"] = Color.White;
            Preferences.Set(AppSettings.IsEnabledLightBackgroundKey, true);
            if (restart)
                App.RestartApp();
        }

        public static void EnableDarkTheme(bool restart = true)
        {
            if (!Preferences.Get(AppSettings.IsEnabledLightBackgroundKey, true)) return;
            Application.Current.Resources["BackgroundColor"] = Color.FromHex("#fff6a3");
            Preferences.Set(AppSettings.IsEnabledLightBackgroundKey, false);
            if (restart)
                App.RestartApp();
        }

        public static void CancelNotification(DateTime date)
        {
            NotificationCenter.Current.Cancel(date.Year + date.Month + date.Day);
        }

        public static async Task RegisterNotification(string name, TimeSpan time, bool forceUpdate = false)
        {
            //if (CultureInfo.CurrentCulture.Name == "en-US")
            //    await PopupNavigation.Instance.PushAsync(new AffirmationPopup() { });

            if (string.IsNullOrEmpty(name)) return;

            string content = AppResources.Hi + name + ", " + AppResources.NotificationMessage;

            DateTime registeredUpto;

            if (forceUpdate)
            {
                registeredUpto = DateTime.Now.AddDays(-2).Date;
            }
            else
            {
                registeredUpto = Preferences.Get(AppSettings.NotificationRegisteredDateKey, DateTime.Now.AddDays(-2)).Date;
            }

            var requiredDateUpto = DateTime.Now.AddDays(10).Date;

            NotificationCenter.Current.CancelAll();

            await Task.Run(() =>
            {
                while (registeredUpto.Date < requiredDateUpto)
                {
                    registeredUpto = registeredUpto.AddDays(1);

                    Console.WriteLine("Notification Registered for " + registeredUpto.Add(time));

                    var notification = new NotificationRequest
                    {
                        NotificationId = registeredUpto.Year + registeredUpto.Month + registeredUpto.Day,
                        Title = AppResources.AppName,
                        Description = content,
                        NotifyTime = registeredUpto.Add(time)
                    };
                    NotificationCenter.Current.Show(notification);
                }

                Preferences.Set(AppSettings.NotificationRegisteredDateKey, requiredDateUpto);
            });
        }

        DateTime start;

        public static string Email;

        protected override void OnStart()
        {
            start = DateTime.Now;
        }

        protected override void OnSleep()
        {
            if (!string.IsNullOrEmpty(Email))
            {
                Analytics.TrackEvent("session+", new Dictionary<string, string>
                {
                    { Email, (DateTime.Now - start).TotalSeconds.ToString()}
                });
            }
        }

        protected override void OnResume()
        {
            start = DateTime.Now;
        }

        public static string GetString(string name)
        {
            return AppResources.ResourceManager.GetString(name);
        }
    }
}