using FFImageLoading;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace HappinessIndex.Models
{
    [Preserve(AllMembers = true)]
    public class ListViewContactsInfo
    {
        #region Fields

        private string match;
        private string image;
        private string contactType;
        private string contactName;

        #endregion

        #region Constructor

        public ListViewContactsInfo()
        {

        }

        #endregion

        #region Public Properties

        public string ContactName
        {
            get { return contactName; }
            set
            {
                this.contactName = value;
                RaisePropertyChanged("contactName");
            }
        }


        public string Match
        {
            get { return this.match; }
            set
            {
                this.match = value;
                RaisePropertyChanged("Match");
            }
        }

        public string ContactType
        {
            get { return contactType; }
            set
            {
                this.contactType = value;
                RaisePropertyChanged("ContactType");
            }
        }

        public string Image
        {
            get { return this.image; }
            set
            {
                this.image = value;
                this.RaisePropertyChanged("Image");
            }
        }

        #endregion

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(String name)
        {
            if (PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}
