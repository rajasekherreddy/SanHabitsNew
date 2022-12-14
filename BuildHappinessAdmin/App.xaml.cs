using System;
using BuildHappinessAdmin.Views;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BuildHappinessAdmin
{
    public partial class App : Application
    {
        public App()
        {
            Xamarin.Forms.Device.SetFlags(new string[] { "Shapes_Experimental", "Expander_Experimental" });

            InitializeComponent();

            MainPage = new LoginPage();

            LoginViaFingerPrint();
        }

        private async void LoginViaFingerPrint()
        {
            var request = new AuthenticationRequestConfiguration("Prove you have fingers!", "Because without it you can't have access");
            var result = await CrossFingerprint.Current.AuthenticateAsync(request);
            if (result.Authenticated)
            {
                // do secret stuff :)
            }
            else
            {
                // not allowed to do secret stuff :(
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
