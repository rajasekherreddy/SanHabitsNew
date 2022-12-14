using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BuildHappinessAdmin.Views
{
    public partial class ServiceProviderRequestPage : ContentPage
    {
        public ServiceProviderRequestPage()
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
