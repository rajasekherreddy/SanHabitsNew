using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HappinessIndex.Views
{
    public partial class LangugeSelectionPage : ContentPage
    {
        public LangugeSelectionPage()
        {
            InitializeComponent();

            //((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.FromHex("#8fc449");
            //((NavigationPage)Application.Current.MainPage).BarTextColor = Color.White;     

            Lang.ItemsSource = new List<String>
            {
                "English",
                "Portuguese",
                "Hindi",
                "French",
                "Spanish"
            };

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var preference = Preferences.Get(AppSettings.LanguageKey, "English");
            Lang.SelectedItem = preference.ToString();
        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            var language = Lang.SelectedItem.ToString();

            SettingsPage.ChangeLanguage(language);
            Preferences.Set(AppSettings.LanguageKey, language);
            App.RestartApp();
        }

        void Button1_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}
