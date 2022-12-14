using BuildHappiness.Core.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Linq;
using Rg.Plugins.Popup.Services;
using HappinessIndex.Views.Popup;
using HappinessIndex.Resx;
using BuildHappiness.Core.Common;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace HappinessIndex.ViewModels
{
    public class TherapySearchListViewModel : ViewModelBase
    {
        public Command RemoveItem { get; set; }

        private bool isNotRecord;
        public bool IsNotRecord
        {
            get => isNotRecord;
            set
            {
                if (isNotRecord == value) return;
                isNotRecord = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<ServiceProvider> therapists;
        public ObservableCollection<ServiceProvider> Therapists
        {
            get => therapists;
            set
            {
                if (this.therapists == value) return;
                this.therapists = value;
                NotifyPropertyChanged();
            }
        }

        public TherapySearchListViewModel()
        {
            Title = "MY LIFE COACHES";

            if (AppSettings.ProviderFilter.provider_type == "Therapist")
            {
                Title = "MY THERAPISTS";
            }

            GetSearchList();
            RemoveItem = new Command<string>(Remove);
        }

        private async void Remove(object obj)
        {
            if (obj != null && userData != null && userData.IsCompletedSuccessfully)
            {
                var data = userData.Result;
                var email = obj.ToString();
                if (data.ServiceProviders.Contains(email))
                {
                    data.ServiceProviders.Remove(email);
                    await CloudService.SaveUserDataWithServiceProvider(Preferences.Get(AppSettings.EmailKey, ""), data);
                    //await PopupNavigation.Instance.PushAsync(new CommonMessage("", "Removed Successfully", AppResources.Ok));

                    Therapists.Remove(Therapists.Where(item => item.Email == email).FirstOrDefault());

                    await ValidateServiceProviderData(false);
                }
            }
        }

        Task<UserData> userData;

        private async void GetSearchList()
        {
            GlobalClass.ShowLoadingBar();

            var email = Preferences.Get(AppSettings.EmailKey, "");

            var result = await CloudService.GetUserDataWithServiceProvider(email);

            userData = CloudService.GetUserData(email);

            if (result != null)
            {
                if (Title == "MY THERAPISTS")
                {
                    Therapists = new ObservableCollection<ServiceProvider>(result.Where(item => item != null && item.Type.Contains("Therapist")));
                }
                else
                {
                    Therapists = new ObservableCollection<ServiceProvider>(result.Where(item => item != null && item.Type.Contains("LifeCoach")));
                }
            }

            await ValidateServiceProviderData();

            GlobalClass.HideLoadingBar();
        }

        private async Task ValidateServiceProviderData(bool showMessage = true)
        {
            if (Therapists == null || !Therapists.Any())
            {
                string message;

                if (Title == "MY THERAPISTS")
                {
                    message = "No records found. Please click ‘Find Therapists’ to select and save.";
                }
                else
                {
                    message = "No records found. Please click ‘Find Life Coaches’ to select and save.";
                }

                if (showMessage)
                    await PopupNavigation.Instance.PushAsync(new CommonMessage("", message, AppResources.Ok));

                await App.GetShell().GoToAsync("..");
            }
        }
    }
}