using Xamarin.Forms;

namespace HappinessIndex.ViewModels
{
    public class TherapySesstionViewModel : ViewModelBase
    {
        public Command CmdGotoTherapyInsurance { get; set; }
        public Command CmdGotoTherapyPreferences { get; set; }

        public TherapySesstionViewModel()
        {
            CmdGotoTherapyInsurance = new Command(GotoTherapyInsurance);
            CmdGotoTherapyPreferences = new Command(GotoTherapyPreferences);
        }
        private async void GotoTherapyInsurance()
        {
            await App.GetShell().GoToAsync("TherapyInsuranceView");
        }
        private async void GotoTherapyPreferences()
        {
            await App.GetShell().GoToAsync("TherapyPreferencesView");
        }
    }
}
