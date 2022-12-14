using System;
using System.Collections.Generic;
using System.Globalization;
using Syncfusion.XForms.Buttons;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HappinessIndex.Views
{
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
        }

        async void ResetPassword(System.Object sender, System.EventArgs e)
        {
            var email = Preferences.Get(AppSettings.EmailKey, "");

            await App.GetShell().GoToAsync($"resetpassword?email={email}&resetUsingPassword={true}");
            if (!string.IsNullOrEmpty(email) && App.Current.MainPage is NavigationPage)
            {
                //(App.Current.MainPage as NavigationPage).PushAsync(new ResetPasswordPage(email, true));
            }
        }
    }

    public class IntToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return int.Parse(value.ToString());
        }
    }
}
