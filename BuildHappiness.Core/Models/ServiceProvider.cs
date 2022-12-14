using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Syncfusion.XForms.AvatarView;
using BuildHappiness.Core.Helpers;
using System.Collections.Generic;

namespace BuildHappiness.Core.Models
{
    public class ServiceProvider : INotifyPropertyChanged
    {
        private string selectedCountry;

        public string Name
        {
            get => name;
            set
            {
                if (name == value) return;
                name = value;
                NotifyPropertyChanged();
            }
        }

        public string Email
        {
            get => email;
            set
            {
                if (email == value) return;
                email = value;
                NotifyPropertyChanged();
            }
        }

        private string type;
        public string Type
        {
            get => type;
            set
            {
                if (type == value) return;
                type = value;
                NotifyPropertyChanged();
            }
        }

        [JsonIgnore]
        public string TypeWithCategory
        {
            get
            {
                if(type == "Therapist")
                {
                    return type + ", " + therapistCategory;
                }
                return type;
            }
        }

        private string insurance;
        public string Insurance
        {
            get => insurance;
            set
            {
                if (insurance == value) return;
                insurance = value;
                NotifyPropertyChanged();
            }
        }

        private string gender;
        public string Gender
        {
            get => gender;
            set
            {
                if (gender == value) return;
                gender = value;
                NotifyPropertyChanged();
            }
        }

        private string videoChat;
        public string VideoChat
        {
            get => videoChat;
            set
            {
                if (videoChat == value) return;
                videoChat = value;
                NotifyPropertyChanged();
            }
        }

        private bool isChecked;
        [JsonIgnore]
        public bool IsChecked
        {
            get => isChecked;
            set
            {
                if (isChecked == value) return;
                isChecked = value;
                NotifyPropertyChanged();
            }
        }

        private string[] states;
        [JsonIgnore]
        public string[] States
        {
            get => states;
            set
            {
                if (states == value) return;
                states = value;
                NotifyPropertyChanged();
            }
        }

        public string Country
        {
            get => selectedCountry;
            set
            {
                if (selectedCountry != value)
                {
                    selectedCountry = value;

                    //State = "";
                    //City = "";
                    //PostalCode = "";

                    if (selectedCountry == "Brazil")
                    {
                        PhoneMask = "(+55)(0000000000)";
                        States = Constants.CountryAndStates.Brazil;
                    }
                    else if (selectedCountry == "France")
                    {
                        PhoneMask = "(+33)(0000000000)";
                        States = Constants.CountryAndStates.France;
                    }
                    else if (selectedCountry == "India")
                    {
                        PhoneMask = "(+91)(0000000000)";
                        States = Constants.CountryAndStates.India;
                    }
                    else if (selectedCountry == "United States")
                    {
                        PhoneMask = "(+1)(0000000000)";
                        States = Constants.CountryAndStates.USA;
                    }

                    NotifyPropertyChanged();
                }
            }
        }

        private string postalCode;
        public string PostalCode
        {
            get { return postalCode; }
            set
            {
                if (postalCode == value) return;
                postalCode = value;

                if (!string.IsNullOrEmpty(Country))
                {
                    if ((Country == "India" && postalCode.Length == 6) ||
                        (Country == "Brazil" && postalCode.Length == 8) ||
                        (Country == "France" && postalCode.Length == 5) ||
                        (Country == "United States" && postalCode.Length == 5))
                    {
                        FetchCountryDetails();
                    }
                }

                NotifyPropertyChanged();
            }
        }

        private string phoneMask;
        [JsonIgnore]
        public string PhoneMask
        {
            get => phoneMask;
            set
            {
                if (phoneMask == value) return;
                phoneMask = value;

                NotifyPropertyChanged();
            }
        }

        private string profilePhoto;
        [JsonIgnore]
        public string ProfilePhoto
        {
            get => profilePhoto;
            set
            {
                if (profilePhoto == value) return;
                profilePhoto = value;
                UpdateProfilePhotoStream();
                NotifyPropertyChanged();
            }
        }

        private void UpdateProfilePhotoStream()
        {
            using (FileStream stream = File.Open(ProfilePhoto, FileMode.Open))
            {
                ProfilePhotoStream = new byte[stream.Length];
                stream.ReadAsync(ProfilePhotoStream, 0, (int)stream.Length).Wait();
            }
        }

        private byte[] profilePhotoStream;
        public byte[] ProfilePhotoStream
        {
            get
            {
                return profilePhotoStream;
            }
            set
            {
                if (profilePhotoStream == value) return;

                if (value == null)
                {
                    AvatarType = ContentType.AvatarCharacter;
                }
                else
                {
                    AvatarType = ContentType.Custom;
                }

                profilePhotoStream = value;

                NotifyPropertyChanged();
            }
        }

        private ContentType avatarType = ContentType.AvatarCharacter;
        [JsonIgnore]
        public ContentType AvatarType
        {
            get => avatarType;
            set
            {
                if (avatarType == value) return;
                avatarType = value;

                NotifyPropertyChanged();
            }
        }

        private string state;
        public string State
        {
            get => state;
            set
            {
                if (state == value) return;
                state = value;

                NotifyPropertyChanged();
            }
        }

        private string city;
        public string City
        {
            get => city;
            set
            {
                if (city == value) return;
                city = value;

                NotifyPropertyChanged();
            }
        }

        private string address;
        public string Address
        {
            get => address;
            set
            {
                if (address == value) return;
                address = value;

                NotifyPropertyChanged();
            }
        }

        private string status;
        public string Status
        {
            get => status;
            set
            {
                if (status == value) return;
                status = value;

                NotifyPropertyChanged();
            }
        }

        private string comments;
        public string Comments
        {
            get => comments;
            set
            {
                if (comments == value) return;
                comments = value;

                NotifyPropertyChanged();
            }
        }

        private string specialities;
        public string Specialities
        {
            get
            {
                if (type != null && !type.Contains("Therapist"))
                {
                    return string.Empty;
                }
                return specialities;
            }
            set
            {
                if (specialities == value) return;
                specialities = value;

                NotifyPropertyChanged();
            }
        }

        private string therapistCategory;
        public string TherapistCategory
        {
            get
            {
                if (type != null && !type.Contains("Therapist"))
                {
                    return string.Empty;
                }
                return therapistCategory;
            }
            set
            {
                if (therapistCategory == value) return;
                therapistCategory = value;
                NotifyPropertyChanged();
            }
        }

        private string language;
        public string Language
        {
            get => language;
            set
            {
                if (language == value) return;
                language = value;

                NotifyPropertyChanged();
            }
        }

        private string businessNumber;
        public string BusinessNumber
        {
            get => businessNumber;
            set
            {
                if (businessNumber == value) return;
                businessNumber = value;

                NotifyPropertyChanged();
            }
        }

        private string mobileNumber;
        public string MobileNumber
        {
            get => mobileNumber;
            set
            {
                if (mobileNumber == value) return;
                mobileNumber = value;

                NotifyPropertyChanged();
            }
        }

        private string fax;
        public string Fax
        {
            get => fax;
            set
            {
                if (fax == value) return;
                fax = value;

                NotifyPropertyChanged();
            }
        }

        private string website;
        private string email;
        private string name;

        public string Website
        {
            get => website;
            set
            {
                if (website == value) return;
                website = value;

                NotifyPropertyChanged();
            }
        }

        private async void FetchCountryDetails()
        {
            return;
            if (!string.IsNullOrEmpty(PostalCode))
            {
                var result = await LocationHelper.GetLocationByPostAsync(PostalCode, Country.GetCountyCode());

                if (result == null) return;

                foreach (var item in result)
                {
                    State = item.State;
                    City = item.City;

                    return;
                }
            }
        }

        public string GetMissingFields()
        {
            var missingFieldsList = new List<string>();

            if (string.IsNullOrEmpty(email))
            {
                missingFieldsList.Add("Email");
            }
            if (string.IsNullOrEmpty(name))
            {
                missingFieldsList.Add("Name");
            }

            if (string.IsNullOrEmpty(mobileNumber))
            {
                missingFieldsList.Add("Mobile Number");
            }

            if (string.IsNullOrEmpty(gender))
            {
                missingFieldsList.Add("Gender");
            }

            if (string.IsNullOrEmpty(videoChat))
            {
                missingFieldsList.Add("Video Chat");
            }

            if (string.IsNullOrEmpty(postalCode))
            {
                missingFieldsList.Add("Postal Code");
            }

            if (string.IsNullOrEmpty(Country))
            {
                missingFieldsList.Add("Country");
            }

            if (string.IsNullOrEmpty(state))
            {
                missingFieldsList.Add("State");
            }

            if (string.IsNullOrEmpty(city))
            {
                missingFieldsList.Add("City");
            }

            if (string.IsNullOrEmpty(language))
            {
                missingFieldsList.Add("Language");
            }

            if (string.IsNullOrEmpty(address))
            {
                missingFieldsList.Add("Address");
            }

            if (string.IsNullOrEmpty(Type))
            {
                missingFieldsList.Add("Service Provider Type");
            }
            return string.Join(", ", missingFieldsList);
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
