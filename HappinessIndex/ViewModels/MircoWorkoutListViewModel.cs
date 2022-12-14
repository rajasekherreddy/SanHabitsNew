using System;
using System.Collections.Generic;
using HappinessIndex.Models;
using Xamarin.Essentials;
using System.Linq;
using Xamarin.Forms;
using HappinessIndex.Helpers;
using System.Timers;
using System.Threading;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Services;
using HappinessIndex.Views;

namespace HappinessIndex.ViewModels
{
    public class MircoWorkoutListViewModel : ViewModelBase
    {
        bool appeared;

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

        public User User { get; set; }
        private List<MicroWorkout> selectedFactors;

        public List<MicroWorkout> SelectedFactors
        {
            get => selectedFactors;
            set
            {
                if (selectedFactors == value) return;
                selectedFactors = value;

                NotifyPropertyChanged();
            }
        }

        public Command PlayInYoutubeCommand { get; set; }

        public Command PlayTimerCommand { get; set; }

        public MircoWorkoutListViewModel()
        {
            isFromYoutube = false;
            timerInProgess = false;
            PlayInYoutubeCommand = new Command(PlayInYoutubeHanlder);
            PlayTimerCommand = new Command(PlayTimerHanlder);
            SaveHighlightsCommand = new Command(SaveHighlights);
        }

        bool isPreviousDayAndHasRecords;bool isFromYoutube = false;
        protected async override void OnAppearing()
        {
            isFromPlayTimerHanlder = false;
            if (!isFromYoutube)
            {
                IsBusy = true;
                appeared = false;
                EditDate = string.Empty;
                //isPreviousDayAndHasRecords = false;

                //if (User == null)
                {
                    User = await DataService.GetUserAsync(Preferences.Get(AppSettings.EmailKey, ""));
                }

                SelectedFactors = await DataService.GetAllMicroWorkout(User);

                SelectedFactors = SelectedFactors.Where(x => x.IsSelected).ToList();
                appeared = true;
                IsBusy = false;
            }
        }

        private async void PlayInYoutubeHanlder(object Id)
        {
            isFromYoutube = true;
           var microWorkout = SelectedFactors.Where(x => x.ID == (int)Id).FirstOrDefault();
            // await Launcher.OpenAsync(new Uri("https://www.youtube.com/watch?v=pLlI2N9z-cQ"));
            if (!string.IsNullOrEmpty(microWorkout.YoutubeLink)){
                await Browser.OpenAsync(microWorkout.YoutubeLink, BrowserLaunchMode.SystemPreferred);
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("", "No video to play", "OK");
            }
        }


        System.Timers.Timer timer;
        int mins = 0, secs = 30;
        bool timerInProgess = false;
        bool isFromPlayTimerHanlder;
        MicroWorkout MicroWorkout;
        private async void PlayTimerHanlder(object Id)
        {
            isFromPlayTimerHanlder = true;
            if (timerInProgess)
            {
                await Application.Current.MainPage.DisplayAlert("", "Workout In-Progress.", "OK");
                return;
            }

            MicroWorkout = SelectedFactors.Where(x => x.ID == (int)Id).FirstOrDefault();
            mins = MicroWorkout.WorkoutDurationMin;
            secs = MicroWorkout.WorkoutDurationSec;
            timer = new System.Timers.Timer();

            timer.Interval = 1000; // 1 sec  
            timer.Elapsed += Timer_Elapsed;
            timer.Start();

            isFromYoutube = true;

            if (!string.IsNullOrEmpty(MicroWorkout.YoutubeLink))
            {
                await Browser.OpenAsync(MicroWorkout.YoutubeLink, BrowserLaunchMode.SystemPreferred);
            }
            
            
    }



        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timerInProgess = true;
            MicroWorkout.WorkoutDurationSec--;

            if (MicroWorkout.WorkoutDurationSec == 0)
            {
                if (MicroWorkout.WorkoutDurationMin == 0)
                {
                    timer.Stop();
                    //show alert
                    timerInProgess = false;
                    MicroWorkout.WorkoutDurationMin = mins;
                    MicroWorkout.WorkoutDurationSec = secs;
                    DataService.UpdateMicroWoroutAsync(MicroWorkout);
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Application.Current.MainPage.DisplayAlert("", "Duration completed", "OK");
                    });
                    MicroWorkout.IsPlayed = true;
                }
                if (MicroWorkout.WorkoutDurationMin > 0)
                {
                    MicroWorkout.WorkoutDurationMin--;
                    MicroWorkout.WorkoutDurationSec = 60;
                }
            }
        }


        private bool showWorkoutList = true;
        public bool ShowWorkoutList
        {
            get => showWorkoutList;
            set
            {
                showWorkoutList = value;
                NotifyPropertyChanged();
            }
        }

        private bool showExitingView = false;
        public bool ShowExitingView
        {
            get => showExitingView;
            set
            {
                showExitingView = value;
                NotifyPropertyChanged();
            }
        }


        public Command SegmentChange;

        #region segment 2
        public Command SaveHighlightsCommand { get; set; }

        private Highlights highlights;

        public Highlights Highlights
        {
            get => highlights;
            set
            {
                if (highlights == value) return;
                highlights = value;
                NotifyPropertyChanged();
            }
        }

        private async void SaveHighlights()
        {
            IsBusy = true;
            await DataService.UpdateHighlightsAsync(Highlights);
            IsBusy = false;

            await PopupNavigation.Instance.PushAsync(new Views.Popup.CommonMessage("", Resx.AppResources.SavedSuccessfully, Resx.AppResources.Ok));
        }

        #endregion


        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            //going out from the page
            if (!isFromPlayTimerHanlder && timer != null)
            {
                timerInProgess = false;
                isFromYoutube = false;
                //isFromPlayTimerHanlder = false;
                timer.Stop();
                timer.Elapsed -= Timer_Elapsed;
                timer = null;
                //if (MicroWorkout != null)
                //{
                //    MicroWorkout.WorkoutDurationMin = mins;
                //    MicroWorkout.WorkoutDurationSec = secs;
                //    MicroWorkout.IsPlayed = false;
                //}
            }

        }
    }
}

