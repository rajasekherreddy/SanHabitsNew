using HappinessIndex.Common;
using HappinessIndex.ViewModels;
using Newtonsoft.Json;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HappinessIndex.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TherapistsView : ContentPage
    {
        public TherapistsView()
        {
            InitializeComponent();
        }

        async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            try
            {
                Label label = sender as Label;
                var span = label.FormattedText.Spans[1];
                var link = span.Text;
                if (!link.ToLower().Contains("http"))
                {
                    link = link.Insert(0, "https://");
                }
                await Browser.OpenAsync(link, BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {
                // An unexpected error occured. No browser may be installed on the device.
            }
        }

        void PhoneTapped(object sender, EventArgs e)
        {
            try
            {
                Label label = sender as Label;
                var span = label.FormattedText.Spans[1];
                var phone = span.Text;
                PhoneDialer.Open(phone);
            }
            catch (Exception ex)
            {
                // An unexpected error occured. No browser may be installed on the device.
            }
        }
    }
}