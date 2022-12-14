using System;
using System.Collections.Generic;
using HappinessIndex.Models;
using Xamarin.Forms;
using System.Linq;
using Xamarin.Essentials;
using HappinessIndex.Resx;
using Rg.Plugins.Popup.Services;
using HappinessIndex.Views.Popup;
using HappinessIndex.DependencyService;
using HappinessIndex.Helpers;
using System.Threading.Tasks;

namespace HappinessIndex.ViewModels
{
    public class FactorsListPageViewModel : ViewModelBase
    {
        private string editDate;

        public string EditDate
        {
            get => editDate;
            set
            {
                if (editDate == value) return;
                editDate = value;

                NotifyPropertyChanged();
            }
        }

        public Command SaveCommand { get; set; }

        public Command AddFactorCommand { get; set; }

        public Command ValidateFactorsCommand { get; set; }

        public bool HasInvalidName
        {
            get => hasInvalidName;
            set
            {
                if (hasInvalidName == value) return;
                hasInvalidName = value;
                NotifyPropertyChanged();
            }
        }

        public Factor CustomFactor { get; set; }

        public User User { get; set; }

        private List<Factor> factors;

        public List<Factor> Factors
        {
            get => factors;
            set
            {
                if (factors == value) return;
                factors = value;

                NotifyPropertyChanged();
            }
        }

        public FactorsListPageViewModel()
        {
            SaveCommand = new Command(SaveFactors);
            ValidateFactorsCommand = new Command(ValidateFactors);
            AddFactorCommand = new Command(AddFactor);
            HomeCommand = new Command(HomeFactor);

        }

        public Command HomeCommand { get; set; }
        private async void HomeFactor(object parameter)
        {
            try
            {
                await App.GetShell().GoToAsync("//main");
            }
            catch (Exception ex)
            {

            }
        }

        bool isPreviousDayAndHasRecords;

        protected async override void OnAppearing()
        {
            IsBusy = true;
            appeared = false;
            EditDate = string.Empty;
            isPreviousDayAndHasRecords = false;

            //if (User == null)
            {
                User = await DataService.GetUserAsync(Preferences.Get(AppSettings.EmailKey, ""));
            }

            Factors = await DataService.GetAllFactors(User);

            if (AppSettings.JournalDate.Date != DateTime.Now.Date)
            {
                var journals = await DataService.GetJournalsAsync(AppSettings.JournalDate);
                if (journals != null && journals.Count > 0)
                {
                    EditDate = AppResources.EditFactorsFor + AppSettings.JournalDate.ToString("MMM-dd");
                    isPreviousDayAndHasRecords = true;
                    foreach (var factor in Factors)
                    {
                        if (journals.Where(item => item.FactorID == factor.ID).Count() > 0)
                        {
                            factor.IsSelected = true;
                        }
                    }
                }
                else
                {
                    //If no entry for previous date, then use the current available selected factors.
                    Factors.UpdateSelectedFactors(User);
                }
            }
            else
            {
                Factors.UpdateSelectedFactors(User);
            }

            appeared = true;

            IsBusy = false;
        }

        bool appeared;
        private bool hasInvalidName;

        private async void ValidateFactors(object parameter)
        {
            Factor factor = parameter as Factor;

            if (appeared)
            {
                var selectedFactors = Factors.Where(item => item.IsSelected).ToList();

                if (selectedFactors.Count > 7)
                {
                    factor.IsSelected = false;

                    await PopupNavigation.Instance.PushAsync(new CommonMessage("", AppResources.PleaseSelectAMaximumOf6Factors, AppResources.Ok));
                    return;
                }
                else if (selectedFactors.Count < 2)
                {
                    factor.IsSelected = true;
                    await PopupNavigation.Instance.PushAsync(new CommonMessage("", AppResources.PleaseSelectAMinimumOf2Factors, AppResources.Ok));
                    return;
                }
            }
        }

        private async void AddFactor(object parameter)
        {
            if (parameter == null)
            {
                CustomFactor = new Factor();
                await PopupNavigation.Instance.PushAsync(new AddFactor() { BindingContext = this });
            }
            else
            {
                hasInvalidName = false;
                if (string.IsNullOrEmpty(CustomFactor.Name))
                {
                    hasInvalidName = true;
                }
                else
                {
                    if (!string.IsNullOrEmpty(User.CustomFactors))
                    {
                        User.CustomFactors += "±";
                    }
                    string factorNameWithWishlist = CustomFactor.Name;

                    User.CustomFactors += factorNameWithWishlist + "±" + CustomFactor.Description;
                    await PopupNavigation.Instance.PopAllAsync();
                    await DataService.UpdateUserAsync(User);
                    Xamarin.Forms.DependencyService.Get<IToast>().Show(AppResources.AddedSuccessfully, 1.5);

                    OnAppearing();
                }
            }
        }

        private async void SaveFactors()
        {
            //TODO: Refactor this code.
            IsBusy = true;

            canUpdateDB = false;
            //Previous days

            if (isPreviousDayAndHasRecords)
            {
                await ValidateAndDeletePreviousRecords();
            }
            else
            {
                canUpdateDB = true;
            }

            if (canUpdateDB)
            {
                await ValidateAndDeletePreviousRecords();

                if (canUpdateDB)
                {
                    User.SelectedFactors = "";
                    foreach (var factor in Factors)
                    {
                        if (factor.IsSelected)
                        {
                            User.SelectedFactors += factor.ID + ",";
                        }

                        if (factor.IsWishlistUpdated)
                        {
                            await DataService.AddWishlistAsync(factor.Wishlist);
                            factor.IsWishlistUpdated = false;
                        }
                    }

                    var result = await DataService.UpdateUserAsync(User);

                    if (result == 1)
                    {
                        //await PopupNavigation.Instance.PushAsync(new CommonMessage("", AppResources.FactorChangeWarning, AppResources.Ok));
                        await App.GetShell().GoToAsync("//journal");
                    }
                }
            }

            IsBusy = false;
        }

        bool canUpdateDB;

        private async Task ValidateAndDeletePreviousRecords()
        {
            var previousRecords = await DataService.GetJournalsAsync(AppSettings.JournalDate);
            if (previousRecords == null || previousRecords.Count == 0)
            {
                canUpdateDB = true;
            }
            else
            {
                List<Journal> deleteJournals = new List<Journal>();

                string deleteJournalsText = string.Empty;

                foreach (var journal in previousRecords)
                {
                    var factor = Factors.Where(item => item.ID == journal.FactorID).FirstOrDefault();

                    if (factor != null && !factor.IsSelected)
                    {
                        if (!string.IsNullOrEmpty(deleteJournalsText))
                        {
                            deleteJournalsText += ", ";
                        }
                        deleteJournals.Add(journal);

                        deleteJournalsText += factor.Name;
                    }
                }

                var result = false;

                if (deleteJournals.Count > 0)
                {
                    //result = await Application.Current.MainPage.DisplayAlert("", "The \"Day’s Score\" entered for " + deleteJournalsText + " on " + AppSettings.JournalDate.ToString("MMM-dd") + " will be deleted",
                    result = await Application.Current.MainPage.DisplayAlert("", string.Format(AppResources.JournalDeleteWarning, deleteJournalsText, AppSettings.JournalDate.ToString("MMM-dd")),
                    AppResources.Ok, AppResources.Cancel);

                    if (result)
                    {
                        foreach (var journal in deleteJournals)
                        {
                            await DataService.DeleteJournalAsync(journal);
                        }
                    }
                    else
                    {
                        canUpdateDB = false;
                        OnAppearing();
                    }
                }
                else if (isPreviousDayAndHasRecords)
                {
                    canUpdateDB = true;
                }
            }
        }
    }
}