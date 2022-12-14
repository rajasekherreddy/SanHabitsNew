using Xamarin.Forms;

namespace HappinessIndex.ViewModels
{
    public class TherapyViewModel : ViewModelBase
    {
        public Command GetStartedCommand { get; set; }
        public Command CmdGotoTherapyLocation { get; set; }
        public TherapyViewModel()
        {
            GetStartedCommand = new Command(NavigateToGetStarted);
            CmdGotoTherapyLocation = new Command(GotoTherapyLocation);
            if (AppSettings.ProviderFilter.provider_type == "Therapist")
            {
                IsTherapist = true;
                MainTitle = "Get the support you need with convenient and on-demand\nTele-Psychology sessions with a Licensed Clinician.";
                Title = "THERAPISTS";
            }
            else
            {
                IsTherapist = false;
                MainTitle = "Get the support, guidance and coaching you need with convenient\non-demand\nTele-LifeCoaching with Certified Life Coaches.";
                Title = "LIFE COACHES";
            }
        }

        private bool isTherapist;
        public bool IsTherapist
        {
            get { return isTherapist; }
            set
            {
                isTherapist = value;
                NotifyPropertyChanged("IsTherapist");
            }
        }

        private string maintitle;

        public string MainTitle
        {
            get { return maintitle; }
            set { maintitle = value; NotifyPropertyChanged(nameof(MainTitle)); }
        }

        private void NavigateToGetStarted()
        {
            WelcomePageViewModel.Navigate();
        }
        private async void GotoTherapyLocation()
        {
            await App.GetShell().GoToAsync("TherapyPreferencesView");
        }
    }
}
