using System;
using HappinessIndex.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HappinessIndex.ViewModels
{
    public class TermsPageViewModel : ViewModelBase
    {
        public Command TermsAcceptedCommand { get; set; }

        public Command ShowTermsCommand { get; set; }

        private bool enableAcceptButton;

        public bool EnableAcceptButton
        {
            get => enableAcceptButton;
            set
            {
                if (enableAcceptButton == value) return;
                enableAcceptButton = value;

                NotifyPropertyChanged();
            }
        }

        public TermsPageViewModel()
        {
            TermsAcceptedCommand = new Command(TermsAccepted);
            ShowTermsCommand = new Command(ShowTerms);
        }

        private void ShowTerms()
        {
            Application.Current.MainPage = new PrivacyDocument();
        }

        private void TermsAccepted()
        {
            Application.Current.MainPage = new WelcomePage();
            Preferences.Set("new_user", "false");
        }

        protected override void OnAppearing()
        {
            var readPrivacy = bool.Parse(Preferences.Get(AppSettings.ReadPrivacyKey, "false"));
            if (readPrivacy)
            {
                EnableAcceptButton = true;
            }
        }
    }
}