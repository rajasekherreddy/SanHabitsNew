using HappinessIndex.ViewModels;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HappinessIndex.Views.Popup
{
    public partial class AffirmationPopup
    {
        public AffirmationPopup()
        {
            InitializeComponent();
            this.BindingContext = new AffirmationQuoteViewModel();
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
    }
}