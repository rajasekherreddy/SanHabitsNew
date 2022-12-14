using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;

namespace HappinessIndex.Views
{
    public partial class SignupPage
    {
        public SignupPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            var height = Application.Current.MainPage.Height;
            MainContent.TranslationY = height;

            base.OnAppearing();

            MainContent.TranslateTo(0, 0, 250); base.OnAppearing();
        }
    }

    public class DefaultAgeToEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString() == "0")
            {
                return string.Empty;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}