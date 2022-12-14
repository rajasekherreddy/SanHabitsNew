using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SQLite;
using Xamarin.Essentials;

namespace HappinessIndex.Models
{
    public class Journal : INotifyPropertyChanged, ISliderModel
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [Ignore]
        public Factor Factor { get; set; }

        private double actualValue;

        public int FactorID { get; set; }

        public int UserID { get; set; }

        [Ignore]
        public string FactorName
        {
            get
            {
                if (Factor == null)
                {
                    return "";
                }
                else
                {
                    return Factor.DisplayName;
                }
            }
        }

        [Ignore]
        public string Icon { get; set; }

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

        public DateTime Date { get; set; }

        public Journal()
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