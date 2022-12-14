using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HappinessIndex.Views.Popup
{
    public partial class PasswordInput
    {
        public PasswordInput()
        {
            InitializeComponent();
        }

        //protected override bool OnBackgroundClicked()
        //{
        //    CloseAllPopup();
        //    return base.OnBackgroundClicked();
        //}

        private async void CloseAllPopup()
        {
            await PopupNavigation.Instance.PopAllAsync();
        }

        void ClosePopup(System.Object sender, System.EventArgs e)
        {
            CloseAllPopup();
        }

        async void ResetPassword(System.Object sender, System.EventArgs e)
        {
            var email = Preferences.Get(AppSettings.EmailKey, "");

            if (!string.IsNullOrEmpty(email))
            {
                CloseAllPopup();
                await App.GetShell().GoToAsync($"resetpassword?email={email}&resetUsingPassword={true}");
            }
        }
    }
}
