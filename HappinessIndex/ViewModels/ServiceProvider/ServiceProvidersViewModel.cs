using BuildHappiness.Core.Common;
using BuildHappiness.Core.Models;
using HappinessIndex.Models;
using HappinessIndex.Views;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HappinessIndex.ViewModels
{
    public class ServiceProvidersViewModel : ViewModelBase
    {
        public Command cmdServiceProvider { get; set; }
        public Command CmdRregistrationView { get; set; }

        private List<menu> serviceProviderMenu;
        public List<menu> ServiceProviderMenu
        {
            get => serviceProviderMenu;
            set
            {
                if (this.serviceProviderMenu == value) return;

                this.serviceProviderMenu = value;

                NotifyPropertyChanged(nameof(serviceProviderMenu));
            }
        }

        public ServiceProvidersViewModel()
        {
            cmdServiceProvider = new Command<long>(ServiceProvider);
            CmdRregistrationView = new Command<string>(Rregistration);
            ServiceProviderMenu = new List<menu>();
            SetserviceProviderMenu();
        }

        internal static ServiceProvider ExistingData;

        private async void Rregistration(string from)
        {
            GlobalClass.ShowLoadingBar();
            Routing.RegisterRoute("RregistrationView", typeof(RegistrationView));

            var email = Preferences.Get(AppSettings.EmailKey, "");
            var existingData = await CloudService.GetServiceProviderForEdit(email);

            //if(existingData != null && existingData.Status == "Decline")
            //{
            //    GlobalClass.ShowAlertMessage($"Your are not authorised");
            //    GlobalClass.HideLoadingBar();
            //    return;
            //}

            if (from == "Edit")
            {
                if(existingData == null)
                {
                    GlobalClass.HideLoadingBar();
                    GlobalClass.ShowAlertMessage($"No existing entry for {email}, please use the registration form to create a new profile");
                }
                else
                {
                    ExistingData = existingData;
                    await Shell.Current.GoToAsync($"RregistrationView?IsNewRegistration=false");
                }
            }
            else
            {
                if(existingData != null)
                {
                    GlobalClass.ShowAlertMessage($"The request for {email}, is already registered, please use the edit registration option");
                }
                else
                {
                    await Shell.Current.GoToAsync($"RregistrationView?IsNewRegistration=true");
                }
            }
            GlobalClass.HideLoadingBar();
        }

        private void SetserviceProviderMenu()
        {
            ServiceProviderMenu.Add(new menu
            {
                Id = 1,
                Name = "New Registration Form",
                SubName = "(Use only if you are a Therapist or Life Coach)",
                PageName = "RregistrationView",
                Value = "New"
            });
            ServiceProviderMenu.Add(new menu
            {
                Id = 2,
                Name = "Edit Registration Form",
                SubName = "(Use only if you are a Therapist or Life Coach)",
                PageName = "RregistrationView",
                Value = "Edit"
            });

            ServiceProviderMenu.Add(new menu
            {
                Id = 3,
                Name = "Find Therapists",
                SubName = "",
                PageName = "TherapyView",
                Value = "Therapist"
            });
            ServiceProviderMenu.Add(new menu
            {
                Id = 4,
                Name = "Find Life Coaches",
                SubName = "",
                PageName = "TherapyView",
                Value = "Life Coach"
            });

            ServiceProviderMenu.Add(new menu
            {
                Id = 5,
                Name = "My Therapists",
                SubName = "",
                PageName = "TherapySearchListView",
                Value = "Therapist"
            });

            ServiceProviderMenu.Add(new menu
            {
                Id = 6,
                Name = "My Life Coaches",
                SubName = "",
                PageName = "TherapySearchListView",
                Value = "Life Coach"
            });
        }
        private async void ServiceProvider(long id)
        {
            var selectMenu = ServiceProviderMenu.Find(s => s.Id == id);
            AppSettings.ProviderFilter = new providerFilter();

            switch (selectMenu.Id)
            {
                case 1:
                    Rregistration(selectMenu.Value);
                    break;
                case 2:
                    Rregistration(selectMenu.Value);
                    break;
                case 3:
                    AppSettings.ProviderFilter.provider_type = selectMenu.Value;
                    await App.GetShell().GoToAsync(selectMenu.PageName);
                    break;
                case 4:
                    AppSettings.ProviderFilter.provider_type = selectMenu.Value;
                    await App.GetShell().GoToAsync(selectMenu.PageName);
                    break;
                case 5:
                    AppSettings.ProviderFilter.provider_type = selectMenu.Value;
                    await App.GetShell().GoToAsync(selectMenu.PageName);
                    break;
                case 6:
                    AppSettings.ProviderFilter.provider_type = selectMenu.Value;
                    await App.GetShell().GoToAsync(selectMenu.PageName);
                    break;
                default:
                    break;
            }
        }
    }
}
