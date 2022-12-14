using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using HappinessIndex.Resx;
using SQLite;
using Xamarin.Essentials;

namespace HappinessIndex.Models
{
    public class OveralScore : INotifyPropertyChanged, ISliderModel
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public int UserID { get; set; }

        private double actualValue;

        public double ActualValue
        {
            get => actualValue;
            set
            {
                if (this.actualValue == value) return;

                this.actualValue = value;

                NotifyPropertyChanged();
            }
        }

        private bool canDisplaySliderLabel = true;

        [Ignore]
        public bool CanDisplaySliderLabel
        {
            get => canDisplaySliderLabel; set
            {
                if (canDisplaySliderLabel == value) return;
                canDisplaySliderLabel = value;
                NotifyPropertyChanged();
            }
        }

        [Ignore]
        public string FactorName { get; set; } = AppResources.YourDay;

        [Ignore]
        public string Icon { get; set; } = "DayScore.png";

        public DateTime Date { get; set; }

        public OveralScore()
        {
            UserID = Preferences.Get(AppSettings.UserIDKey, 1);
        }

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