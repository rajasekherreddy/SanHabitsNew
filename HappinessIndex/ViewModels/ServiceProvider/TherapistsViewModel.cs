using BuildHappiness.Core.Common;
using BuildHappiness.Core.Helpers;
using BuildHappiness.Core.Models;
using FFImageLoading;
using HappinessIndex.Models;
using HappinessIndex.Resx;
using HappinessIndex.Views.Popup;
using Rg.Plugins.Popup.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HappinessIndex.ViewModels
{
    [Preserve(AllMembers = true)]
    public class TherapistsViewModel : ViewModelBase
    {
        public Command CmdSaveTherapy { get; set; }

        public ObservableCollection<ListViewContactsInfo> ContactsInfo { get; set; }

        private List<ProvidersEntity> providers;
        public List<ProvidersEntity> Providers
        {
            get => providers;
            set
            {
                if (this.providers == value) return;

                this.providers = value;

                NotifyPropertyChanged();
            }
        }

        private List<ServiceProvider> therapists;
        public List<ServiceProvider> Therapists
        {
            get => therapists;
            set
            {
                if (this.therapists == value) return;
                this.therapists = value;
                NotifyPropertyChanged(nameof(Therapists));
            }
        }

        public TherapistsViewModel()
        {
            GetList();
            CmdSaveTherapy = new Command(SaveTherapy);
            SetTitle();
        }

        private void SetTitle()
        {
            if (AppSettings.ProviderFilter.provider_type == "Therapist")
            {
                Title = "THERAPISTS";
            }
            else
            {
                Title = "LIFE COACHES";
            }
        }

        private UserData userData;

        private async void GetList()
        {
            GlobalClass.ShowLoadingBar();

            var userEmail = Preferences.Get(AppSettings.EmailKey, "");

            var userDataRunner = CloudService.GetUserData(userEmail);

            Therapists = await ApplyFilter();

            if (userDataRunner.IsCompleted)
            {
                userData = userDataRunner.Result;
            }

            if (userData != null && userData.ServiceProviders != null)
            {
                foreach (var therapist in Therapists)
                {
                    if (userData.ServiceProviders.Contains(therapist.Email))
                    {
                        therapist.IsChecked = true;
                    }
                }
            }

            GlobalClass.HideLoadingBar();

            if (Therapists == null || Therapists.Count() == 0)
            {
                string message;
                if (Title == "THERAPISTS")
                {
                    message = "Sorry, no therapist found for your search criteria.";
                }
                else
                {
                    message = "Sorry, no life coach found for your search criteria.";
                }

                await PopupNavigation.Instance.PushAsync(new CommonMessage("", message, AppResources.Ok));
                await App.GetShell().GoToAsync("..");
            }
        }

        private async Task<List<ServiceProvider>> ApplyFilter()
        {
            var therapists = await CloudService.GetValidServiceProviders();

            var filteredItems = new List<ServiceProvider>();
            var pref = AppSettings.ProviderFilter;

            var ignoreLanguage = pref.providerLanguage.IsEmpty();
            var ignoreGender = pref.providerGender.IsEmpty();
            var ignoreSpecialities = pref.provider_specialities.IsEmpty();
            var ignoreCountry = pref.country == null || string.IsNullOrEmpty(pref.country.name);
            var ignoreState = string.IsNullOrEmpty(pref.state);

            var nearbyCode = "";
            if (pref.sessionType != "online")
            {
                nearbyCode = await LocationHelper.GetNearbyLocation(pref.postalCode, pref.miles, pref.country.code);
            }

            var type = Regex.Replace(pref.provider_type, @"\s+", "");

            foreach (var therapist in therapists)
            {
                if (therapist.Type.Contains(type)
                    && (ignoreLanguage || pref.providerLanguage.IsLanguageMatch(therapist.Language))
                    && (ignoreGender || pref.providerGender.IsMatch(therapist.Gender))
                    && (ignoreSpecialities || pref.provider_specialities.IsSpecialitiesMatch(therapist.Specialities != null ? therapist.Specialities : ""))
                    && (ignoreCountry || pref.country.name == therapist.Country)
                    && (ignoreState || pref.state == therapist.State))
                {
                    if (pref.sessionType == "online")
                    {
                        filteredItems.Add(therapist);
                    }
                    else if (nearbyCode.Contains(therapist.PostalCode))
                    {
                        filteredItems.Add(therapist);
                    }
                }
            }

            return filteredItems;
        }

        public async void SaveTherapy()
        {
            GlobalClass.ShowLoadingBar();

            if (userData == null)
            {
                userData = new UserData();
                userData.ServiceProviders = new List<string>();
            }

            foreach (var therapist in Therapists)
            {
                if (therapist.IsChecked)
                {
                    if (!userData.ServiceProviders.Contains(therapist.Email))
                        userData.ServiceProviders.Add(therapist.Email);
                }
                else
                {
                    if (userData.ServiceProviders.Contains(therapist.Email))
                        userData.ServiceProviders.Remove(therapist.Email);
                }
            }

            var result = await CloudService.SaveUserDataWithServiceProvider(Preferences.Get(AppSettings.EmailKey, ""), userData);

            GlobalClass.HideLoadingBar();

            if (result == 1)
            {
                await PopupNavigation.Instance.PushAsync(new CommonMessage("", "Saved Successfully", AppResources.Ok));
                await App.GetShell().GoToAsync("../../..");
            }
        }
    }
}