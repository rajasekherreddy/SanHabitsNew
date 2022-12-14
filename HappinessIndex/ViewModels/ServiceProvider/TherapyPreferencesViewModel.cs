using BuildHappiness.Core.Common;
using HappinessIndex.Models;
using HappinessIndex.Resx;
using HappinessIndex.Views.Popup;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HappinessIndex.ViewModels
{
    public class TherapyPreferencesViewModel : ViewModelBase
    {
        private ObservableCollection<CountryEnttiy> countryList;
        public ObservableCollection<CountryEnttiy> CountryList
        {
            get => countryList;
            set
            {
                if (this.countryList == value) return;
                this.countryList = value;
                NotifyPropertyChanged();
            }
        }

        private CountryEnttiy country;
        public CountryEnttiy Country
        {
            get => country;
            set
            {
                if (this.country == value) return;
                this.country = value;
                NotifyPropertyChanged();
            }
        }


        private string[] states;
        public string[] States
        {
            get => states;
            set
            {
                if (this.states == value) return;
                this.states = value;
                NotifyPropertyChanged();
            }
        }
        private string selectState;
        public string SelectState
        {
            get => selectState;
            set
            {
                if (this.selectState == value) return;
                this.selectState = value;
                AppSettings.ProviderFilter.state = value;
                NotifyPropertyChanged();
            }
        }

        public Command CmdGotoTherapists { get; set; }

        public Command CmdCountryChanged { get; set; }

        private ICommand cmdsessionType;
        public ICommand CmdsessionType
        {
            get { return cmdsessionType; }
            set
            {
                cmdsessionType = value;
                NotifyPropertyChanged("CmdsessionType");
            }
        }
        private bool isChecked;
        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                NotifyPropertyChanged("IsChecked");
            }
        }

        private provider_specialities provider_specialities;
        public provider_specialities ProviderSpecialities
        {
            get => provider_specialities;
            set
            {
                if (this.provider_specialities == value) return;
                this.provider_specialities = value;
                NotifyPropertyChanged();
            }
        }
        private string whatAre;
        public string WhatAre
        {
            get => whatAre;
            set
            {
                if (this.whatAre == value) return;
                this.whatAre = value;
                NotifyPropertyChanged();
            }
        }

        private bool isSpecialities;
        public bool IsSpecialities
        {
            get => isSpecialities;
            set
            {
                if (this.isSpecialities == value) return;
                this.isSpecialities = value;
                NotifyPropertyChanged();
            }
        }

        private bool ismilesDisplay;
        public bool IsmilesDisplay
        {
            get => ismilesDisplay;
            set
            {
                if (this.ismilesDisplay == value) return;
                this.ismilesDisplay = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<SelectItemList> miles;
        public ObservableCollection<SelectItemList> Miles
        {
            get => miles;
            set
            {
                if (this.miles == value) return;
                this.miles = value;
                NotifyPropertyChanged();
            }
        }
        private SelectItemList mile;
        public SelectItemList Mile
        {
            get => mile;
            set
            {
                if (this.mile == value) return;
                this.mile = value;
                NotifyPropertyChanged();
            }
        }

        private string postCode;
        public string PostCode
        {
            get => postCode;
            set
            {
                if (this.postCode == value) return;
                this.postCode = value;
                NotifyPropertyChanged();
            }
        }

        private providerLanguage providerLanguage;
        public providerLanguage ProviderLg
        {
            get => providerLanguage;
            set
            {
                if (this.providerLanguage == value) return;
                this.providerLanguage = value;
                NotifyPropertyChanged();
            }
        }
        private providerGender providerGender;
        public providerGender ProviderGr
        {
            get => providerGender;
            set
            {
                if (this.providerGender == value) return;
                this.providerGender = value;
                NotifyPropertyChanged();
            }
        }

        public TherapyPreferencesViewModel()
        {
            Country = new CountryEnttiy();

            CountryList = new ObservableCollection<CountryEnttiy>()
            {
                new CountryEnttiy(){ name = "India" },
                new CountryEnttiy(){ name = "United States" },
                new CountryEnttiy(){ name = "France" },
                new CountryEnttiy(){ name = "Brazil" }
            };

            States = new string[] { "Select the Country" };
            ProviderGr = new providerGender();
            ProviderLg = new providerLanguage();
            ProviderSpecialities = new provider_specialities();
            CmdGotoTherapists = new Command(GotoTherapists);
            CmdsessionType = new Command<string>(SessionType);
            Miles = new ObservableCollection<SelectItemList>();
            SetMiles();

            CmdCountryChanged = new Command(CountryChanged);
            IsChecked = true;
            SessionType("online");

            if (AppSettings.ProviderFilter.provider_type == "Therapist")
            {
                Title = "THERAPIST PREFERENCES";
                IsSpecialities = true;
                WhatAre = "What are your Therapist";
            }
            else
            {
                Title = "LIFE COACH PREFERENCES";
                IsSpecialities = false;
                WhatAre = "What are your Life coache";
            }
        }

        private async void CountryChanged()
        {
            AppSettings.ProviderFilter.country = country;

            if (country.name == "India")
            {
                States = BuildHappiness.Core.Constants.CountryAndStates.India;
            }
            else if (country.name == "United States")
            {
                States = BuildHappiness.Core.Constants.CountryAndStates.USA;
            }
            else if (country.name == "France")
            {
                States = BuildHappiness.Core.Constants.CountryAndStates.France;
            }
            else if (country.name == "Brazil")
            {
                States = BuildHappiness.Core.Constants.CountryAndStates.Brazil;
            }
        }

        protected override void OnAppearing()
        {
            GlobalClass.HideLoadingBar();
        }

        private void SetMiles()
        {
            Miles.Add(new SelectItemList { Text = "5 miles", value = "5" });
            Miles.Add(new SelectItemList { Text = "10 miles", value = "10" });
            Miles.Add(new SelectItemList { Text = "15 miles", value = "15" });
            Miles.Add(new SelectItemList { Text = "25 miles", value = "25" });
            Miles.Add(new SelectItemList { Text = "50 miles", value = "50" });
        }

        private void SessionType(string type)
        {
            if (IsChecked)
            {
                if (type == "online")
                {
                    IsmilesDisplay = false;
                    AppSettings.ProviderFilter.postalCode = "00000";
                }
                else
                {
                    IsmilesDisplay = true;
                    //Mile = Miles.FirstOrDefault();
                }
                AppSettings.ProviderFilter.sessionType = type;
            }
        }

        private void BackToHome()
        {
            GlobalClass.HideLoadingBar();
            WelcomePageViewModel.Navigate();
        }

        private async void GotoTherapists()
        {
            //var hasLocationPermission = await GlobalClass.LocationPermission();
            //if (!hasLocationPermission && AppSettings.ProviderFilter.sessionType != "online")
            //{
            //    await PopupNavigation.Instance.PushAsync(new CommonMessage("", "Location permission is required. Please grant permission from settings.", AppResources.Ok));
            //    //BackToHome();
            //    return;
            //}

            AppSettings.ProviderFilter.latitude = "";

            AppSettings.ProviderFilter.longitude = "";

            try
            {
                if (AppSettings.ProviderFilter.sessionType != "online")
                {
                    GlobalClass.ShowLoadingBar();
                    //var location = await Geolocation.GetLastKnownLocationAsync();
                    //if (location != null)
                    //{
                    //    AppSettings.ProviderFilter.latitude = location.Latitude.ToString();//"64.2008413"; 
                    //    AppSettings.ProviderFilter.longitude = location.Longitude.ToString();//"-149.4937";
                    //}
                    //AppSettings.providerfilter.latitude = "64.2008413";
                    //AppSettings.providerfilter.longitude = "-149.4937";
                    GlobalClass.HideLoadingBar();
                }
            }

            catch (FeatureNotSupportedException fnsEx)
            {
                GlobalClass.HideLoadingBar();
                // Handle not supported on device exception
                GlobalClass.ShowToastMessage(fnsEx.Message);
                //BackToHome();
                return;
            }

            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
                GlobalClass.ShowToastMessage(fneEx.Message);
                //BackToHome();
                return;
            }

            catch (PermissionException pEx)
            {
                GlobalClass.ShowToastMessage(pEx.Message);
                //BackToHome();
                return;
                // Handle permission exception
            }

            catch (Exception ex)
            {
                GlobalClass.ShowToastMessage(ex.Message);
                //BackToHome();
                return;
            }

            if (AppSettings.ProviderFilter.sessionType != "online" && AppSettings.ProviderFilter.country == null)
            {
                await PopupNavigation.Instance.PushAsync(new CommonMessage("", "Choose the Country. It is required for In-Person sessions.", AppResources.Ok));
                return;
            }

            AppSettings.ProviderFilter.postalCode = postCode;
            AppSettings.ProviderFilter.miles = mile?.value;
            AppSettings.ProviderFilter.providerGender = new providerGender();
            AppSettings.ProviderFilter.providerGender = ProviderGr;
            AppSettings.ProviderFilter.providerLanguage = new providerLanguage();
            AppSettings.ProviderFilter.providerLanguage = ProviderLg;
            AppSettings.ProviderFilter.provider_specialities = new provider_specialities();
            AppSettings.ProviderFilter.provider_specialities = ProviderSpecialities;
            await App.GetShell().GoToAsync("TherapistsView");
        }
    }
}