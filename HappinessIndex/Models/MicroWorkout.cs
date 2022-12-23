using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using HappinessIndex.Helpers;
using HappinessIndex.Resx;
using SQLite;
using Xamarin.Forms;
using System.Collections.Generic;
using HappinessIndex.ViewModels;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Rg.Plugins.Popup.Services;
using HappinessIndex.Views.Popup;

namespace HappinessIndex.Models
{
    public class MicroWorkout : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string YoutubeLink { get; set; }

        private int workoutDurationMin;
        public int WorkoutDurationMin { get => workoutDurationMin; set { workoutDurationMin = value; NotifyPropertyChanged(); } }

        private int workoutDurationSec;
        public int WorkoutDurationSec { get => workoutDurationSec; set { workoutDurationSec = value; NotifyPropertyChanged(); } }

        private int breakDurationMin=0;
        public int BreakDurationMin { get => breakDurationMin; set { breakDurationMin = value; NotifyPropertyChanged(); } }

        private int breakDurationSec=10;
        public int BreakDurationSec { get => breakDurationSec; set { breakDurationSec = value; NotifyPropertyChanged(); } }

        private int noOfSets=3;
        public int NoOfSets { get => noOfSets; set { noOfSets = value; NotifyPropertyChanged(); } }

        private int noOfSetsCompleted = 1;
        public int NoOfSetsCompleted { get => noOfSetsCompleted; set { noOfSetsCompleted = value; NotifyPropertyChanged(); } }

        public TimeSpan WorkoutReminder { get; set; }

        private bool isSelected;
        public bool IsSelected { get => isSelected; set { isSelected = value;NotifyPropertyChanged(); } }

        public string WorkoutIcon { get; set; }

        private bool isPlayed;
        public bool IsPlayed { get => isPlayed; set { isPlayed = value;NotifyPropertyChanged(); } }

        public bool IsCustomWorkout { get; set; } = false;

        //public DateTime WorkoutDate { get; set; } = DateTime.Today;
        public DateTime WorkoutDate { get; set; } = AppSettings.JournalDate.Date.Date;



        [Ignore]
        private bool allowEdit { get; set; } = false;
        [Ignore]
        public bool AllowEdit
        {
            get => allowEdit;
            set
            {
                allowEdit = value;
                NotifyPropertyChanged();
            }
        } 

        [Ignore]
        public bool IsUpdated { get; set; } = false;


        #region commented code


        //private string duration { get; set; } = "00:30";

        //[Ignore]
        //public string Duration
        //{
        //    get => duration;
        //    set
        //    {
        //        duration = value;

        //        NotifyPropertyChanged();
        //    }
        //}

        //private bool durationPlayed { get; set; } = false;

        // [Ignore]
        //public bool DurationPlayed
        //{
        //    get => durationPlayed;
        //    set
        //    {
        //        durationPlayed = value;

        //        NotifyPropertyChanged();
        //    }
        //}

        //private string playedIcon { get; set; } = "play-button";

        //[Ignore]
        //public string PlayedIcon
        //{
        //    get => playedIcon;
        //    set
        //    {
        //        playedIcon = value;

        //        NotifyPropertyChanged();
        //    }
        //}


        //[Ignore]
        //public string DisplayName
        //{
        //    get
        //    {
        //        var localSting = AppResources.ResourceManager.GetString(Name);
        //        if (string.IsNullOrEmpty(localSting))
        //        {
        //            return Name;
        //        }
        //        else
        //        {
        //            return localSting;
        //        }
        //    }
        //}

        //public bool NeedToSave
        //{
        //    get => needToSave;
        //    set
        //    {
        //        needToSave = value;

        //        NotifyPropertyChanged();
        //    }
        //}



        //[Ignore]
        //public string DisplayDescription
        //{
        //    get
        //    {
        //        if (Description == null) return "";

        //        var localSting = AppResources.ResourceManager.GetString(Description);
        //        if (string.IsNullOrEmpty(localSting))
        //        {
        //            return Description;
        //        }
        //        else
        //        {
        //            return localSting;
        //        }
        //    }
        //}

        //public bool IsCustomFactor { get; set; }

        //[Ignore]
        //public bool IsSelected
        //{
        //    get => isSelect;
        //    set
        //    {
        //        if (isSelect != value)
        //        {
        //            isSelect = value;
        //            if (!pauseNotifyPropertyChanged)
        //                NotifyPropertyChanged();
        //        }
        //    }
        //}

        //Color? color = null;

        //[Ignore]
        //public Color Color
        //{
        //    get
        //    {
        //        if (color == null)
        //        {
        //            color = ColorHelper.GetColor(ID);
        //        }
        //        return color.Value;
        //    }
        //}

        //[Ignore]
        //public Command ShareCommand { get; set; }

        //public FactorWorkout()
        //{

        //}

        //private bool pauseNotifyPropertyChanged;
        //private bool needToSave;

        //public void PauseNotifyPropertyChanged()
        //{
        //    pauseNotifyPropertyChanged = true;
        //}

        //public void ResumeNotifyPropertyChanged()
        //{
        //    pauseNotifyPropertyChanged = false;
        //}

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.  
        // The CallerMemberName attribute that is applied to the optional propertyName  
        // parameter causes the property name of the caller to be substituted as an argument.  
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}