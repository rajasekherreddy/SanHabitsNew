using BuildHappiness.Core.Common;
using Xamarin.Forms;

namespace HappinessIndex.ViewModels
{
    public class TherapyInsuranceViewModel : ViewModelBase
    {
        private string insurances_accepted;
        public string Insurances_accepted
        {
            get => insurances_accepted;
            set
            {
                if (this.insurances_accepted == value) return;
                this.insurances_accepted = value;
                NotifyPropertyChanged();
            }
        }
        public Command CmdGotoTherapyPreferences { get; set; }
        public TherapyInsuranceViewModel()
        {
            Insurances_accepted = "";
            CmdGotoTherapyPreferences = new Command(GotoTherapyPreferences);
        }
        private async void GotoTherapyPreferences()
        {
            if (string.IsNullOrEmpty(Insurances_accepted))
            {
                GlobalClass.ShowToastMessage("Please input Value");
                return;
            }
            AppSettings.ProviderFilter.insurances_accepted = Insurances_accepted;
            Application.Current.MainPage = new AppShell(false);
            await App.GetShell().GoToAsync("TherapyPreferencesView");
        }
    }
}