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
using HappinessIndex.Resx;
using HappinessIndex.Views.Popup;
using System.Runtime.InteropServices.ComTypes;

namespace HappinessIndex.ViewModels
{
    public class MircoWorkoutListViewModel : ViewModelBase
    {
        bool appeared;

        private string editDate="good";
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

        public Command StartTimerCommand { get; set; }

        public Command PauseTimerCommand { get; set; }

        public Command CloseTimerCommand { get; set; }

        public MircoWorkoutListViewModel()
        {
            isFromYoutube = false;
            timerInProgess = false;
            PlayInYoutubeCommand = new Command(PlayInYoutubeHanlder);
            PlayTimerCommand = new Command(PlayTimerHanlder);
            SaveHighlightsCommand = new Command(SaveHighlights);
            StartTimerCommand = new Command(StartTimerHanlder);
            PauseTimerCommand = new Command(StopTimerHanlder);
            CloseTimerCommand = new Command(CloseTimerHanlder);
            
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
        int DurationMin = 0, DurationSec = 30;
        int breakMins = 0, breakSecs = 10;
        bool timerInProgess = false;
        bool isWorkout = true;
        bool isPause = false;
        bool isFromPlayTimerHanlder;
        private MicroWorkout microWorkout;
        public MicroWorkout MicroWorkout
        {
            get => microWorkout;
            set
            {
                if (microWorkout == value) return;
                microWorkout = value;

                NotifyPropertyChanged();
            }
        }

        private async void PlayTimerHanlder(object Id)
        {
            //isFromPlayTimerHanlder = true;
            //if (timerInProgess)
            //{
            //    await Application.Current.MainPage.DisplayAlert("", "Workout In-Progress.", "OK");
            //    return;
            //}

            microWorkout = SelectedFactors.Where(x => x.ID == (int)Id).FirstOrDefault();
            //mins = microWorkout.WorkoutDurationMin;
            //secs = microWorkout.WorkoutDurationSec;
            //timer = new System.Timers.Timer();
            //timer.Interval = 1000; // 1 sec  
            //timer.Elapsed += Timer_Elapsed;
            //timer.Start();

            //isFromYoutube = true;

            //if (!string.IsNullOrEmpty(MicroWorkout.YoutubeLink))
            //{
            //    await Browser.OpenAsync(MicroWorkout.YoutubeLink, BrowserLaunchMode.SystemPreferred);
            //}




            microWorkout = SelectedFactors.Where(x => x.ID == (int)Id).FirstOrDefault();
            var popup = new MicroWorkoutTimerPage(microWorkout) ;

            popup.TaskCompleted += (sender, arg) =>
            {
                 Application.Current.MainPage.DisplayAlert("", "Workout In-Progress.", "OK");
            };

            await PopupNavigation.Instance.PushAsync(popup);

        }

        private async void StartTimerHanlder(object Id)
        {
            if (isPause)
            {
                isPause = false;
                timer.Start();
            }
            else
            {
                isFromPlayTimerHanlder = true;
                if (timerInProgess)
                {
                    await Application.Current.MainPage.DisplayAlert("", "Workout In-Progress.", "OK");
                    return;
                }

                //microWorkout = SelectedFactors.Where(x => x.ID == (int)Id).FirstOrDefault();
                DurationMin = microWorkout.WorkoutDurationMin;
                DurationSec = microWorkout.WorkoutDurationSec;
                breakMins = microWorkout.BreakDurationMin;
                breakSecs = microWorkout.BreakDurationSec;
                timer = new System.Timers.Timer();
                timer.Interval = 1000; // 1 sec  
                timer.Elapsed += Timer_Elapsed;
                timer.Start();
            }
        }

        private async void StopTimerHanlder(object Id)
        {
            if (timer != null)
            {
                isPause = true;
                timer.Stop();
            }
            
        }

        private async void CloseTimerHanlder(object Id)
        {

            

            if (timer != null)
            {
                microWorkout.NoOfSetsCompleted = 1;
                microWorkout.WorkoutDurationMin = DurationMin;
                microWorkout.WorkoutDurationSec = DurationSec;
                microWorkout.BreakDurationMin = breakMins;
                microWorkout.BreakDurationSec = breakSecs;
                timer.Stop();
            }

            await PopupNavigation.Instance.PopAllAsync();

        }



        private async void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timerInProgess = true;
            microWorkout.WorkoutDurationSec--;

            if (microWorkout.WorkoutDurationSec == 0)
            {
                if (microWorkout.WorkoutDurationMin == 0)
                {

                    if (isWorkout && microWorkout.NoOfSets>1 && microWorkout.NoOfSetsCompleted!=microWorkout.NoOfSets)
                    {
                        isWorkout = false;
                        microWorkout.IsBreakTime = true;
                        microWorkout.IsWorkoutTime = false ;
                        microWorkout.WorkoutDurationMin = breakMins;
                        microWorkout.WorkoutDurationSec = breakSecs;
                    }
                    else
                    {
                        isWorkout = true;
                        microWorkout.IsBreakTime = false;
                        microWorkout.IsWorkoutTime = true;
                        microWorkout.WorkoutDurationMin = DurationMin;
                        microWorkout.WorkoutDurationSec = DurationSec;
                        if (microWorkout.NoOfSets <= microWorkout.NoOfSetsCompleted)
                        {
                            //Make it 1 once sets completed.
                            microWorkout.NoOfSetsCompleted = 1;
                            timer.Stop();
                            //show alert
                            timerInProgess = false;
                            await DataService.UpdateMicroWoroutAsync(microWorkout);
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                //PopupNavigation.Instance.PopAllAsync();
                                Application.Current.MainPage.DisplayAlert("", "Duration completed", "OK");
                            });
                            microWorkout.IsPlayed = true;
                            await PopupNavigation.Instance.PopAllAsync();

                        }
                        else
                        {
                            microWorkout.NoOfSetsCompleted++;
                        }
                    }


                }
                else if (microWorkout.WorkoutDurationMin > 0)
                {
                    microWorkout.WorkoutDurationMin--;
                    microWorkout.WorkoutDurationSec = 60;
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

