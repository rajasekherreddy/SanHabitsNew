using BuildHappiness.Core.Common;
using BuildHappiness.Core.Models;
using HappinessIndex.Helpers;
using HappinessIndex.Resx;
using HappinessIndex.Views.Popup;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HappinessIndex.ViewModels
{
    [QueryProperty("IsNewRegistration", "IsNewRegistration")]
    public class RregistrationViewModel : ViewModelBase
    {
        [JsonIgnore]
        public Command SubmitCommand { get; set; }

        [JsonIgnore]
        public Command PickPhotoCommand { get; set; }

        [JsonIgnore]
        public List<string> CountryList { get; set; }

        public ServiceProvider ServiceProvider
        {
            get => serviceProvider;
            set
            {
                if (serviceProvider == value) return;
                serviceProvider = value;

                NotifyPropertyChanged();
            }
        }

        public RregistrationViewModel()
        {
            SubmitCommand = new Command(Submit);
            PickPhotoCommand = new Command(PickPhoto);

            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(Preferences.Get(AppSettings.EmailKey, string.Empty));
            var value = Convert.ToBase64String(plainTextBytes);
            WebUrl = "http://ec2-3-135-225-41.us-east-2.compute.amazonaws.com/pages/service-provider?data=" + value;

            CountryList = new List<string>()
            {
                "Brazil",
                "France",
                "India",
                "United States"
            };

            ServiceProvider = new ServiceProvider();
            
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var user = await DataService.GetUserAsync(Preferences.Get(AppSettings.EmailKey, ""));

            if (user != null && IsNewRegistration == "true")
            {
                ServiceProvider.Email = user.Email;
                ServiceProvider.Name = user.Name;
                ServiceProvider.Country = user.Country;
            }
        }

        private async void PickPhoto(object obj)
        {
            var mediaFile = await PhotoPicker.Pick();

            if (mediaFile != null)
                ServiceProvider.ProfilePhoto = mediaFile.Path;
        }

        private async void Submit(object obj)
        {
            var missingFields = ServiceProvider.GetMissingFields();
            if (!string.IsNullOrEmpty(missingFields))
            {
                await PopupNavigation.Instance.PushAsync(new CommonMessage("", $"Please fill the {missingFields} fields.", AppResources.Ok));
                return;
            }

            IsBusy = true;
            GlobalClass.ShowLoadingBar();
            Preferences.Set("sr_email", ServiceProvider.Email);
            var result = await CloudService.SubmitServiceProviderForReview(ServiceProvider);

            if (result == 1)
            {
                await PopupNavigation.Instance.PushAsync(new CommonMessage("", "Data has been submitted for review.", AppResources.Ok));
                //GlobalClass.ShowAlertMessage("Data has been submitted for review");
                await App.GetShell().GoToAsync("///serviceproviders");
            }
            IsBusy = false;
            GlobalClass.HideLoadingBar();
        }

        private string webUrl;
        public string WebUrl
        {
            get => webUrl;
            set
            {
                if (webUrl == value) return;
                webUrl = value;

                NotifyPropertyChanged();
            }
        }

        private string isNewRegistration;
        private ServiceProvider serviceProvider;

        [JsonIgnore]
        public string IsNewRegistration
        {
            get => isNewRegistration;
            set
            {
                if (isNewRegistration == value) return;
                isNewRegistration = value;

                if (value != "true")
                {
                    //ServiceProvidersViewModel.ExistingData.CopyTo(ServiceProvider);
                    ServiceProvider = ServiceProvidersViewModel.ExistingData;
                    ServiceProvidersViewModel.ExistingData = null;
                    Title = "Edit Registration";
                }
                else
                {
                    Title = "New Registration";
                }
            }
        }

        //public async void PageClose()
        //{
        //    GlobalClass.ShowLoadingBar();
        //    try
        //    {
        //        ClsEmail clsEmail = new ClsEmail();
        //        var emailId = Preferences.Get(AppSettings.EmailKey, string.Empty);
        //        clsEmail.email = emailId;
        //        HttpResponseMessage response = await HttpCall.Post(clsEmail, "provider/getByEmail");
        //        var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        //        var output = JsonConvert.DeserializeObject<ApiResult>(content);
        //        provider_type provider_Type = new provider_type();
        //        Country country = new Country();
        //        providerLanguage language = new providerLanguage();
        //        State state = new State();
        //        if (output.status == true)
        //        {
        //            ProvidersEntity data = new ProvidersEntity();
        //            try
        //            {
        //                data = Newtonsoft.Json.JsonConvert.DeserializeObject<ProvidersEntity>(output.data.ToString());
        //                provider_Type = Newtonsoft.Json.JsonConvert.DeserializeObject<provider_type>(data.provider_type.ToString());
        //                country = Newtonsoft.Json.JsonConvert.DeserializeObject<Country>(data.country.ToString());
        //                language = Newtonsoft.Json.JsonConvert.DeserializeObject<providerLanguage>(data.language.ToString());
        //                state = Newtonsoft.Json.JsonConvert.DeserializeObject<State>(data.state.ToString());
        //            }
        //            catch (Exception ex)
        //            {
        //            }
        //            Providers providers = new Providers();
        //            providers.address1 = data.address1;
        //            providers.address2 = data.address2;
        //            providers.city = data.city;
        //            providers.country = country != null ? country.name : "";
        //            providers.createdAt = data.createdAt;
        //            providers.deletedAt = data.deletedAt;
        //            providers.effdate = data.effdate;
        //            providers.email = data.email;
        //            providers.enable_video_chat = data.enable_video_chat;
        //            providers.fax = data.fax;
        //            providers.fullname = data.fullname;
        //            providers.gender = data.gender;
        //            providers.id = data.id;
        //            //providers._id = data._id;
        //            providers.insurances_accepted = data.insurances_accepted;
        //            providers.isActive = data.isActive;
        //            providers.isDeleted = data.isDeleted;
        //            providers.lastupdated = data.lastupdated;
        //            providers.mobile = data.mobile;
        //            providers.phone_business = data.phone_business;
        //            providers.provider_type = provider_Type.name;
        //            providers.profile_image = data.profile_image;
        //            providers.state = state != null ? state.name : "";
        //            providers.zip = data.zip;

        //            if (language != null)
        //            {
        //                language.ProviderId = data.id;
        //            }
        //            provider_specialities provider_Specialities = new provider_specialities();
        //            provider_Specialities.ProviderId = data.id;
        //            if (data.provider_specialities != null)
        //            {
        //                try
        //                {
        //                    var _Specialities = Newtonsoft.Json.JsonConvert.DeserializeObject<provider_specialities>(data.provider_specialities.ToString());
        //                    if (_Specialities is provider_specialities Specialities)
        //                    {
        //                        provider_Specialities.ADHD = Specialities.ADHD;
        //                        provider_Specialities.adoption = Specialities.adoption;
        //                        provider_Specialities.angerManagement = Specialities.angerManagement;
        //                        provider_Specialities.anxiety = Specialities.anxiety;
        //                        provider_Specialities.autismSpectrum = Specialities.autismSpectrum;
        //                        provider_Specialities.behavioralIssues = Specialities.behavioralIssues;
        //                        provider_Specialities.chronicIllnessorPain = Specialities.chronicIllnessorPain;
        //                        provider_Specialities.depression = Specialities.depression;
        //                        provider_Specialities.domesticAbuseorViolence = Specialities.domesticAbuseorViolence;
        //                        provider_Specialities.mensIssues = Specialities.mensIssues;
        //                        provider_Specialities.parenting = Specialities.parenting;
        //                        provider_Specialities.sleepProblems = Specialities.sleepProblems;
        //                        provider_Specialities.spirituality = Specialities.spirituality;
        //                        provider_Specialities.stressManagement = Specialities.stressManagement;
        //                        provider_Specialities.suicidalIdeation = Specialities.suicidalIdeation;
        //                        provider_Specialities.teenagerIssues = Specialities.teenagerIssues;
        //                        provider_Specialities.traumaandPTSD = Specialities.traumaandPTSD;
        //                        provider_Specialities.weightLoss = Specialities.weightLoss;
        //                        provider_Specialities.womensIssues = Specialities.womensIssues;
        //                    }
        //                }
        //                catch { }

        //                await DataService.ProvidersSave(providers, provider_Specialities, language);
        //            }
        //        }
        //        GlobalClass.HideLoadingBar();
        //    }
        //    catch
        //    {
        //        GlobalClass.HideLoadingBar();
        //    }
        //}
    }
}