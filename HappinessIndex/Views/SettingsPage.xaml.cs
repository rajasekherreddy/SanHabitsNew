using System;
using System.Globalization;
using HappinessIndex.DependencyService;
using HappinessIndex.Resx;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HappinessIndex.Views
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            BackupSwitch.On = Preferences.Get(AppSettings.EnableBackupKey, true);
            //LightThemeSwitch.On = Preferences.Get(AppSettings.IsEnabledLightBackgroundKey, false);
           // Reminder.Time = TimeSpan.Parse(Preferences.Get(AppSettings.PreferredNotificationTimeKey, "19:00"));
            //previousPreferredTime = Reminder.Time;

            //SetLanguage(Preferences.Get(AppSettings.LanguageKey, "en-US"));
        }

        //private void SetLanguage(string lang)
        //{
        //    if (lang == "en-US")
        //    {
        //        Picker.SelectedIndex = 0;
        //    }
        //    else if (lang == "pt-PT")
        //    {
        //        Picker.SelectedIndex = 1;
        //    }
        //    else if (lang == "hi-IN")
        //    {
        //        Picker.SelectedIndex = 2;
        //    }
        //    else if (lang == "fr-CH")
        //    {
        //        Picker.SelectedIndex = 3;
        //    }
        //}

        //void LightTheme(System.Object sender, System.EventArgs e)
        //{
        //    if (LightThemeSwitch.On)
        //    {
        //        App.EnableLightTheme();
        //    }
        //    else
        //    {
        //        App.EnableDarkTheme();
        //    }
        //}


        void Backup(System.Object sender, System.EventArgs e)
        {
            if (BackupSwitch.On)
            {
                Xamarin.Forms.DependencyService.Resolve<IBackUp>().BackUp();
            }
            Preferences.Set(AppSettings.EnableBackupKey, BackupSwitch.On);
        }

        async void DeleteBackup(System.Object sender, System.EventArgs e)
        {
            var confirmDelete = await Application.Current.MainPage.DisplayAlert("Are you sure you want to delete the backup of your Journals from iCloud?", "", AppResources.Ok, AppResources.Cancel);

            if (confirmDelete)
            {
                Xamarin.Forms.DependencyService.Resolve<IBackUp>().DeleteBackup();
            }
        }

        TimeSpan previousPreferredTime;

        async void Reminder_PropertyChanged(System.Object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //if (e.PropertyName == TimePicker.TimeProperty.PropertyName)
            //{
            //    Preferences.Set(AppSettings.PreferredNotificationTimeKey, Reminder.Time.ToString());

            //    if (previousPreferredTime != Reminder.Time)
            //    {
            //        await App.RegisterNotification(Preferences.Get(AppSettings.NameKey, string.Empty), Reminder.Time, true);
            //        previousPreferredTime = Reminder.Time;
            //    }
            //}
        }

        public static void ChangeLanguage(string language)
        {
            string langCode = "";
            if (language == "English")
            {
                langCode = "en-US";
            }
            else if (language == "Portuguese")
            {
                langCode = "pt-PT";
            }
            else if (language == "Hindi")
            {
                langCode = "hi-IN";
            }
            else if (language == "French")
            {
                langCode = "fr-CH";
            }
            else if (language == "Spanish")
            {
                langCode = "es-ES";
            }

            CultureInfo culture = new CultureInfo(langCode);
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }
    }
}