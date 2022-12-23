using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace HappinessIndex.Views.Popup
{
    public partial class AddFactor
    {
        public AddFactor()
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


    }
}
