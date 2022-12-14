using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace HappinessIndex.Views.Popup
{
    public partial class AddFactorMicroWorkout
    {
        public AddFactorMicroWorkout()
        {
            InitializeComponent();
        }

        protected override bool OnBackgroundClicked()
        {
            CloseAllPopup();
            return base.OnBackgroundClicked();
        }

        private async void CloseAllPopup()
        {
            await PopupNavigation.Instance.PopAllAsync();
        }

        void ClosePopup(System.Object sender, System.EventArgs e)
        {
            CloseAllPopup();
        }


        async void YoutubeEntry_Unfocused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
        {
            string youtubelink = (sender as Entry).Text;
            if (!string.IsNullOrEmpty(youtubelink))
            {
                if (!IsValidUrl(youtubelink))
                {
                    (sender as Entry).Text = string.Empty;
                    await Application.Current.MainPage.DisplayAlert("", "Invalid Url value", "OK");
                }
            }
        }



        static bool IsValidUrl(string urlString)
        {
            Uri uri;
            return Uri.TryCreate(urlString, UriKind.Absolute, out uri)
                && (uri.Scheme == Uri.UriSchemeHttp
                 || uri.Scheme == Uri.UriSchemeHttps
                    //|| uri.Scheme == Uri.UriSchemeFtp
                    //|| uri.Scheme == Uri.UriSchemeMailto
                    /*...*/);
        }
    }
}
