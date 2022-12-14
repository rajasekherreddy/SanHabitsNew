using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using HappinessIndex.Helpers;
using HappinessIndex.Resx;
using HappinessIndex.Views.Popup;
using Rg.Plugins.Popup.Services;
using SQLite;
using Xamarin.Forms;

namespace HappinessIndex.Models
{
    public class NegativeFactor : INotifyPropertyChanged
    {
        public event EventHandler<EventArgs> SaveClicked;

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        private double val;

        private Color color;

        internal bool initCompleted;

        private bool hasAnyUpdate;

        [Ignore]
        public Command EnterNotesCommand { get; set; }

        public string Name { get; set; }

        public int UserID { get; set; }

        public DateTime Date { get; set; }

        [Ignore]
        public string DisplayName
        {
            get
            {
                return AppResources.ResourceManager.GetString(Name); 
            }
        }

        private string notes;
        public string Notes
        {
            get => notes; set
            {
                if (notes == value) return;
                notes = value;
                //NotifyPropertyChanged();
                //NotifyPropertyChanged("NotesImage");
            }
        }

        private string causes;
        public string Causes
        {
            get => causes; set
            {
                if (causes == value) return;
                causes = value;
                //NotifyPropertyChanged();
                //NotifyPropertyChanged("NotesImage");
            }
        }

        private string fixes;
        public string Fixes
        {
            get => fixes; set
            {
                if (fixes == value) return;
                fixes = value;
                //NotifyPropertyChanged();
                //NotifyPropertyChanged("NotesImage");
            }
        }

        [Ignore]
        public string NotesImage
        {
            get
            {
                return string.IsNullOrEmpty(Notes) && string.IsNullOrEmpty(Causes) && string.IsNullOrEmpty(Fixes) ? "Edit.png" : "View.png";
            }
        }

        [Ignore]
        public Color Color
        {
            get => color; set
            {
                if (color == value) return;
                color = value;
                NotifyPropertyChanged();
            }
        }

        public double Value
        {
            get => val; set
            {
                if (val == value) return;
                val = value;
                NotifyPropertyChanged();
                UpdateColor();
            }
        }

        private void UpdateColor()
        {
            Color = ColorHelper.GetColorOf(Value, 0, 10);
        }

        internal bool HasNotes()
        {
            return !(string.IsNullOrEmpty(Notes) && string.IsNullOrEmpty(Causes) &&
                string.IsNullOrEmpty(Fixes));
        }

        public NegativeFactor()
        {
            EnterNotesCommand = new Command(EnterNotes);
        }

        public NegativeFactor(bool isLocalInit)
        {
            initCompleted = isLocalInit;
            EnterNotesCommand = new Command(EnterNotes);
        }

        internal bool IsValid()
        {
            return hasAnyUpdate;
        }

        private async void EnterNotes(object param)
        {
            if (param != null)
            {
                NotifyPropertyChanged("NotesImage");
                SaveClicked?.Invoke(this, new EventArgs());
            }
            else
            {
                await PopupNavigation.Instance.PushAsync(new EnterNotes(DisplayName) { BindingContext = this });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.  
        // The CallerMemberName attribute that is applied to the optional propertyName  
        // parameter causes the property name of the caller to be substituted as an argument.  
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (initCompleted)
            {
                hasAnyUpdate = true;
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}