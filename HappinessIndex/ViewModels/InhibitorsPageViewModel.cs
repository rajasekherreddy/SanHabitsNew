using System;
using System.Collections.Generic;
using System.Linq;
using HappinessIndex.Models;
using HappinessIndex.Resx;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Services;
using HappinessIndex.Views.Popup;

namespace HappinessIndex.ViewModels
{
    public class InhibitorsPageViewModel : ViewModelBase
    {
        public User User { get; set; }

        public Command SaveHighlightsCommand { get; set; }

        private List<NegativeFactor> negativeFactors;

        public List<NegativeFactor> NegativeFactors
        {
            get => negativeFactors; set
            {
                if (negativeFactors == value) return;
                negativeFactors = value;

                NotifyPropertyChanged();
            }
        }

        public InhibitorsPageViewModel()
        {
            SaveHighlightsCommand = new Command(SaveHighlights);
        }

        protected async override void OnAppearing()
        {
            IsBusy = true;

            base.OnAppearing();

            User = await DataService.GetUserAsync(Preferences.Get(AppSettings.EmailKey, ""));

            var defaultFactors = GetDefaultNegativeFactors(User.ID);

            //foreach (var data in defaultFactors)
            //{
            //    data.Value = Preferences.Get($"{User.ID}{data.Name}", 0);
            //    data.Notes = Preferences.Get($"{User.ID}{data.Name}Notes", "");
            //    data.Causes = Preferences.Get($"{User.ID}{data.Name}Causes", "");
            //    data.Fixes = Preferences.Get($"{User.ID}{data.Name}Fixes", "");
            //}

            var negativeFactors = await DataService.GetNegativeFactorAsync(AppSettings.JournalDate);

            foreach (var data in defaultFactors)
            {
                var databaseItem = negativeFactors.Where(item => item.Name == data.Name).LastOrDefault();

                if (databaseItem != null)
                {
                    data.Value = databaseItem.Value;

                    data.Notes = databaseItem.Notes;
                    data.Fixes = databaseItem.Fixes;
                    data.Causes = databaseItem.Causes;
                }
            }

            //if (negativeFactors != null && negativeFactors.Count >= defaultFactors.Count)
            //{
            //    NegativeFactors = negativeFactors.Skip(Math.Max(0, negativeFactors.Count - defaultFactors.Count())).ToList();
            //}

            NegativeFactors = defaultFactors;

            foreach (var factor in NegativeFactors)
            {
                factor.SaveClicked -= Factor_SaveClicked;
                factor.SaveClicked += Factor_SaveClicked;
            }

            IsBusy = false;
        }

        private async void Factor_SaveClicked(object sender, EventArgs e)
        {
            await SaveHighlights(false);
        }

        public static List<NegativeFactor> GetDefaultNegativeFactors(int userID)
        {
            return new List<NegativeFactor>()
                {
                    new NegativeFactor(true) {UserID = userID, Name = "Anger"},
                    new NegativeFactor(true) {UserID = userID, Name = "Fear"},
                    new NegativeFactor(true) {UserID = userID, Name = "Sadness"},
                    new NegativeFactor(true) {UserID = userID, Name = "Loneliness" },
                    new NegativeFactor(true) {UserID = userID, Name = "Anxiety"},
                    new NegativeFactor(true) {UserID = userID, Name = "Guilt"},
                    new NegativeFactor(true) {UserID = userID, Name = "Envy" },
                    new NegativeFactor(true) {UserID = userID, Name = "Greed"},
                    new NegativeFactor(true) {UserID = userID, Name = "Negativity"}
                };
        }

        private async void SaveHighlights()
        {
            await SaveHighlights(false);
            //await App.GetShell().GoToAsync("//reports", true);
        }

        private async Task SaveHighlights(bool internaRequest)
        {
            //if (IsBusy && !internaRequest) return;

            IsBusy = true;

            //foreach (var factor in NegativeFactors)
            //{
            //    Preferences.Set(User.ID + "" + factor.Name, factor.Value);
            //    Preferences.Set($"{User.ID}{factor.Name}Notes", factor.Notes);
            //    Preferences.Set($"{User.ID}{factor.Name}Causes", factor.Causes);
            //    Preferences.Set($"{User.ID}{factor.Name}Fixes", factor.Fixes);
            //}
            
            await DataService.SetNegativeFactorAsync(NegativeFactors);

            IsBusy = false;

            if (!internaRequest)
            {
                await PopupNavigation.Instance.PushAsync(new CommonMessage("", AppResources.SavedSuccessfully, AppResources.Ok));
            }
        }
    }
}
