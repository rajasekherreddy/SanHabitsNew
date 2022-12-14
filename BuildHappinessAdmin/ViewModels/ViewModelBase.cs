using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using BuildHappinessAdmin.Data;
using BuildHappinessAdmin.Data.Firebase;

namespace BuildHappinessAdmin.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        internal static IDataService DataService = new CloudDataService();

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
