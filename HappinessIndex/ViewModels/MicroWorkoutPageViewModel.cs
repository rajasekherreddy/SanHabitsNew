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
    public class MicroWorkoutPageViewModel : ViewModelBase
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
        public Command ShareCommand { get; set; }
        

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

        public User User { get; set; }

        public MicroWorkout AddNewMicroWorkout { get; set; }

        private List<MicroWorkout> microWorkoutList;

        public List<MicroWorkout> MicroWorkoutList
        {
            get => microWorkoutList;
            set
            {
               // if (microWorkoutList == value) return;
                microWorkoutList = value;
                NotifyPropertyChanged();
            }
        }

        private List<int> durationPickerMinutes;

        public List<int> DurationPickerMinutes
        {
            get
            {
                durationPickerMinutes = new List<int>();
                for (int i = 0; i <= 60; i++)
                    durationPickerMinutes.Add(i);
                return durationPickerMinutes;
            }

        }

        public int DurationPickerMinutesSelected { get; set; }


        private List<int> durationPickerSeconds;

        public List<int> DurationPickerSeconds
        {
            get
            {
                durationPickerSeconds = new List<int>();
                for (int i = 0; i <= 60; i++)
                    durationPickerSeconds.Add(i);
                return durationPickerSeconds;
            }

        }

        public int DurationPickerSecondsSelected { get; set; }

        public TimeSpan SelectedReminderTime { get; set; }

        public Command EditCommand { get; set; }


        public MicroWorkoutPageViewModel()
        {
           // AddNewMicroWorkout = new MicroWorkout();
            SaveCommand = new Command(SaveFactors);
            ValidateFactorsCommand = new Command(ValidateFactors);
            AddFactorCommand = new Command(AddFactor);
            HomeCommand = new Command(HomeFactor);

            EditCommand = new Command(EditCommandHandler);
            ShareCommand = new Command(ShareCommandHandler);
         
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

            MicroWorkoutList = await DataService.GetAllMicroWorkout(User);

            appeared = true;
            IsBusy = false;
        }

        private async void EditCommandHandler(object parameter)
        {
           var workoutItem = MicroWorkoutList.Where(x => x.ID == (int)parameter).FirstOrDefault();
           workoutItem.AllowEdit = true;
           workoutItem.IsUpdated = true;
        }

        bool appeared;
        private bool hasInvalidName;

        private async void ValidateFactors(object Id)
        {
            if (appeared)
            {
                var workoutItem = MicroWorkoutList.Where(x => x.ID == (int)Id).FirstOrDefault();
                if (MicroWorkoutList.Where(x => x.IsSelected).ToList().Count > 7)
                {
                    await PopupNavigation.Instance.PushAsync(new CommonMessage("", AppResources.PleaseSelectAMaximumOf6Factors, AppResources.Ok));
                    workoutItem.IsSelected = false;
                    return;
                }

                
                workoutItem.AllowEdit = true;
                workoutItem.IsUpdated = true;
            }
        }

        private async void AddFactor(object parameter)
        {
            try
            {
                if (parameter == null)
                {
                    AddNewMicroWorkout = new MicroWorkout();
                    //CustomFactor = new MicroWorkout();
                    await PopupNavigation.Instance.PushAsync(new AddFactorMicroWorkout() { BindingContext = this });
                }
                else
                {
                    if(String.IsNullOrEmpty(AddNewMicroWorkout.Name))
                    {
                        await Application.Current.MainPage.DisplayAlert("", "Workout name is required.", "OK");
                        return;
                    }
                    int customWorkoutCount =  MicroWorkoutList.Where(x => x.IsCustomWorkout).ToList().Count;
                    AddNewMicroWorkout.IsCustomWorkout = true;

                    AddNewMicroWorkout.WorkoutIcon =(customWorkoutCount<11)? string.Format("CW{0}", ++customWorkoutCount): "CW1";
                        await DataService.AddMicroWoroutAsync(AddNewMicroWorkout);
                        await PopupNavigation.Instance.PopAllAsync();
                        //await DataService.UpdateUserAsync(User);
                        Xamarin.Forms.DependencyService.Get<IToast>().Show(AppResources.AddedSuccessfully, 1.5);
                        OnAppearing();
                }
            }
            catch(Exception ex)
            {

            }
        }

        

        private async void SaveFactors()
        {
            //TODO: Refactor this code.
            IsBusy = true;

            canUpdateDB = false;
            //Previous days

            #region commented code
            //if (isPreviousDayAndHasRecords)
            //{
            //    await ValidateAndDeletePreviousRecords();
            //}
            //else
            //{
            //    canUpdateDB = true;
            //}

            //if (canUpdateDB)
            //{
            //    await ValidateAndDeletePreviousRecords();

            //    if (canUpdateDB)
            //    {
            //        User.SelectedFactors = "";
            //        foreach (var factor in MicroWorkout)
            //        {
            //            if (factor.IsSelected)
            //            {
            //                User.SelectedFactors += factor.ID + ",";
            //            }


            //        }

            //        var result = await DataService.UpdateUserAsync(User);

            //        if (result == 1)
            //        {
            //            //await PopupNavigation.Instance.PushAsync(new CommonMessage("", AppResources.FactorChangeWarning, AppResources.Ok));
            //            await App.GetShell().GoToAsync("//journal");
            //        }
            //    }
            //}

            #endregion

           var result = await DataService.UpdateAllMicroWoroutAsync(MicroWorkoutList.Where(x => x.IsUpdated).ToList());
            if (result == 1)
            {
                //await PopupNavigation.Instance.PushAsync(new CommonMessage("", AppResources.FactorChangeWarning, AppResources.Ok));
               
            }
            await App.GetShell().GoToAsync("//journal");
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
                    var factor = MicroWorkoutList.Where(item => item.ID == journal.FactorID).FirstOrDefault();

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

        private async void ShareCommandHandler(object Id)
        {
            //if (isChanged)
            //{
            //    //await Application.Current.MainPage.DisplayAlert("", AppResources.SaveMessage, AppResources.Ok);
            //    await PopupNavigation.Instance.PushAsync(new CommonMessage("", AppResources.SaveMessage, AppResources.Ok));
            //}
            //else
            //{
            //    //Share the wishlist
            //    var text = "Message from San Habits app \nHappiness Enabler : " + DisplayName + "\nWishlist 1 : " + wishlist[0].Description
            //    + "\nWishlist 2 : " + wishlist[1].Description + "\nWishlist 3 : " + wishlist[2].Description;

            //    await Share.RequestAsync(new ShareTextRequest
            //    {
            //        Text = text,
            //        Title = "Share your wishlist"
            //    });
            //}

            var mw  = microWorkoutList.Where(x => x.ID == (int)Id).FirstOrDefault();
            //Share the wishlist
            var text = "Message from San Habits app \nMcro Workout: " + mw.Name + "\nMicro workout Name : " + mw.Name
            + "\nMicro workout Description : " + mw.Description + "\nYoutube Description : " + mw.YoutubeLink +"\n Duration : " + string.Format( "{0}:{1}", mw.WorkoutDurationMin, mw.WorkoutDurationSec) + "\nReminder : " + mw.WorkoutReminder;

            await Share.RequestAsync(new ShareTextRequest
            {
                Text = text,
                Title = "Share your Micro Workout Details"
            });
        }
    }
}