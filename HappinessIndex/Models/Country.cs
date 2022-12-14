using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace HappinessIndex.Models
{
    public class Country : INotifyPropertyChanged
    {
        private string id;
        public string Id
        {
            get => Id;
            set
            {
                if (this.Id == value) return;

                this.Id = value;

                NotifyPropertyChanged();
            }
        }

        private DateTime? _createdAt;
        public DateTime? createdAt
        {
            get => _createdAt;
            set
            {
                if (this._createdAt == value) return;

                this._createdAt = value;

                NotifyPropertyChanged();
            }
        }

        private DateTime? _deletedAt;
        public DateTime? deletedAt
        {
            get => _deletedAt;
            set
            {
                if (this._deletedAt == value) return;

                this._deletedAt = value;

                NotifyPropertyChanged();
            }
        }

        private DateTime? _updatedAt;
        public DateTime? updatedAt
        {
            get => _updatedAt;
            set
            {
                if (this._updatedAt == value) return;

                this._updatedAt = value;

                NotifyPropertyChanged();
            }
        }

        private string _code;
        public string code
        {
            get => _code;
            set
            {
                if (this._code == value) return;

                this._code = value;

                NotifyPropertyChanged();
            }
        }

        private string _name;
        public string name
        {
            get => _name;
            set
            {
                if (this._name == value) return;

                this._name = value;

                NotifyPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
    public class CountryEnttiy
    {
        public string name { get; set; }

        public string code
        {
            get
            {
                if (name == "India")
                {
                    return "IN";
                }
                if (name == "France")
                {
                    return "FR";
                }
                if (name == "United States")
                {
                    return "US";
                }
                if (name == "Brazil")
                {
                    return "BR";
                }
                return "";
            }
        }
    }
}