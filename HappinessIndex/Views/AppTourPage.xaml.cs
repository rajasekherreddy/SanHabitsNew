using System;
using System.Collections.Generic;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms;

namespace HappinessIndex.Views
{
    public partial class AppTourPage : ContentPage
    {
        public AppTourPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            var safeAreaInset = On<Xamarin.Forms.PlatformConfiguration.iOS>().SafeAreaInsets();
            BoxView.Margin = new Thickness(0, 0, 0, -safeAreaInset.Bottom);

            var height = Xamarin.Forms.Application.Current.MainPage.Height;
            MainContent.TranslationY = height;
            
            base.OnAppearing();

            MainContent.TranslateTo(0, 0, 250); 
        }
    }
}