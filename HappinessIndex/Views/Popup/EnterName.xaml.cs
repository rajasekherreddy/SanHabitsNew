using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace HappinessIndex.Views.Popup
{
    public partial class EnterName
    {
        public EventHandler<EventArgs> SaveClicked;

        public EnterName()
        {
            InitializeComponent();
        }

        private async void CloseAllPopup()
        {
            await PopupNavigation.Instance.PopAllAsync();
        }

        void ClosePopup(System.Object sender, System.EventArgs e)
        {
            CloseAllPopup();
            SaveClicked?.Invoke(this, new EventArgs());
        }
    }
}