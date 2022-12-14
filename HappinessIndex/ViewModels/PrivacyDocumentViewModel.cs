using System;
using HappinessIndex.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HappinessIndex.ViewModels
{
    public class PrivacyDocumentViewModel : ViewModelBase
    {
        public Command TermsAcceptedCommand { get; set; }

        public PrivacyDocumentViewModel()
        {
            TermsAcceptedCommand = new Command(TermsAccepted);
        }

        private void TermsAccepted()
        {
            Preferences.Set(AppSettings.ReadPrivacyKey, "true");
            Application.Current.MainPage = new TermsPage();
        }
    }
}
