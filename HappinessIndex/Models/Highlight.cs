using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using SQLite;
using Xamarin.Essentials;

namespace HappinessIndex.Models
{
    public class Highlights : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public int UserID { get; set; }

        private string bestThing;

        public string BestThing
        {
            get => bestThing;
            set
            {
                if (this.bestThing == value) return;

                this.bestThing = value;

                NotifyPropertyChanged();
            }
        }

        [Ignore]
        public bool CanEdit
        {
            get
            {
                return DateTime.Now.Date != Date.Date;
            }
        }

        private string challenge;

        public string Challenge
        {
            get => challenge;
            set
            {
                if (this.challenge == value) return;

                this.challenge = value;

                NotifyPropertyChanged();
            }
        }

        private string lesson;
        private byte[] photo;

        public string Lesson
        {
            get => lesson;
            set
            {
                if (this.lesson == value) return;

                this.lesson = value;

                NotifyPropertyChanged();
            }
        }

        internal bool pause = false;

        public byte[] Photo
        {
            get => photo;
            set
            {
                if (photo == value) return;
                photo = value;
                if (!pause)
                    NotifyPropertyChanged();
            }
        }

        public DateTime Date { get; set; }

        public Highlights()
        {
            UserID = Preferences.Get(AppSettings.UserIDKey, 1);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.  
        // The CallerMemberName attribute that is applied to the optional propertyName  
        // parameter causes the property name of the caller to be substituted as an argument.  
        public void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}