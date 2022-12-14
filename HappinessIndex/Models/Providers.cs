using HappinessIndex.ViewModels;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace HappinessIndex.Models
{
    public class Providers
    {
        [PrimaryKey]
        // public int ID { get; set; }
        public string id { get; set; }

        public string provider_type { get; set; }
        public bool enable_video_chat { get; set; }
        public bool isActive { get; set; }
        public bool isDeleted { get; set; }
        public DateTime? effdate { get; set; }
        public DateTime? lastupdated { get; set; }
        public DateTime? deletedAt { get; set; }
        public DateTime? createdAt { get; set; }
        public string _id { get; set; }
        public string fullname { get; set; }
        public string email { get; set; }
        public string address1 { get; set; }
        public string profile_image { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string country { get; set; }
        public string gender { get; set; }
        public string phone_business { get; set; }
        public string mobile { get; set; }
        public string fax { get; set; }
        public string website { get; set; }
        public string insurances_accepted { get; set; }

    }
    public class provider_specialities
    {
        //[JsonIgnore]
        [PrimaryKey]
        public string ProviderId { get; set; }
        public bool adoption { get; set; }
        public bool angerManagement { get; set; }
        public bool anxiety { get; set; }
        public bool autismSpectrum { get; set; }
        public bool behavioralIssues { get; set; }
        public bool chronicIllnessorPain { get; set; }
        public bool depression { get; set; }
        public bool domesticAbuseorViolence { get; set; }
        public bool mensIssues { get; set; }
        public bool parenting { get; set; }
        public bool sleepProblems { get; set; }
        public bool spirituality { get; set; }
        public bool stressManagement { get; set; }
        public bool suicidalIdeation { get; set; }
        public bool traumaandPTSD { get; set; }
        public bool weightLoss { get; set; }
        public bool womensIssues { get; set; }
        public bool teenagerIssues { get; set; }
        public bool ADHD { get; set; }

        public bool IsEmpty()
        {
            return !adoption && !angerManagement && !anxiety & !autismSpectrum && !behavioralIssues
                && !chronicIllnessorPain && !depression && !domesticAbuseorViolence && !mensIssues
                && !parenting && !sleepProblems && !spirituality && !stressManagement && !suicidalIdeation
                && !traumaandPTSD && !weightLoss && !womensIssues && !teenagerIssues && !ADHD;
        }

        public bool IsSpecialitiesMatch(string specialitie)
        {
            if (adoption && specialitie.Contains("Adoption"))
            {
                return true;
            }
            else if (angerManagement && specialitie.Contains("Anger Management"))
            {
                return true;
            }
            else if (anxiety && specialitie.Contains("Anxiety"))
            {
                return true;
            }
            if (autismSpectrum && specialitie.Contains("Autism Spectrum"))
            {
                return true;
            }
            else if (behavioralIssues && specialitie.Contains("Behavioral Issues"))
            {
                return true;
            }
            else if (chronicIllnessorPain && specialitie.Contains("Chronic lllness or pain"))
            {
                return true;
            }
            if (depression && specialitie.Contains("Depression"))
            {
                return true;
            }
            else if (domesticAbuseorViolence && specialitie.Contains("Domestic Abuse or Violence"))
            {
                return true;
            }
            else if (mensIssues && specialitie.Contains("Men's Issues"))
            {
                return true;
            }
            if (parenting && specialitie.Contains("Parenting"))
            {
                return true;
            }
            else if (sleepProblems && specialitie.Contains("Sleep Problems or Insomnia"))
            {
                return true;
            }
            else if (spirituality && specialitie.Contains("Spirituality"))
            {
                return true;
            }
            else if (stressManagement && specialitie.Contains("Stress Management"))
            {
                return true;
            }
            if (suicidalIdeation && specialitie.Contains("Suicidal Ideation"))
            {
                return true;
            }
            else if (traumaandPTSD && specialitie.Contains("Trauma and PTSD"))
            {
                return true;
            }
            else if (weightLoss && specialitie.Contains("Weight Loss"))
            {
                return true;
            }
            if (womensIssues && specialitie.Contains("Women's Issues"))
            {
                return true;
            }
            else if (teenagerIssues && specialitie.Contains("Teenager Issues"))
            {
                return true;
            }
            else if (ADHD && specialitie.Contains("ADHD"))
            {
                return true;
            }

            return false;
        }
    }

    public class ProvidersEntity : Providers
    {
        public object provider_specialities { get; set; }
        public object provider_type { get; set; }
        public object state { get; set; }
        public object country { get; set; }
        public object language { get; set; }
    }
    public class ClsEmail
    {
        public string email { get; set; }
    }
    public class providerFilter
    {
        public provider_specialities provider_specialities { get; set; } = new provider_specialities();
        public providerLanguage providerLanguage { get; set; } = new providerLanguage();
        public providerGender providerGender { get; set; } = new providerGender();
        public CountryEnttiy country { get; set; }
        public string state { get; set; }
        public string provider_type { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string insurances_accepted { get; set; }
        public string sessionType { get; set; }
        public string postalCode { get; set; }
        public string miles { get; set; }
    }
    public class providerLanguage
    {
        [PrimaryKey]
        public string ProviderId { get; set; }

        public bool English { get; set; }
        public bool Spanish { get; set; }
        public bool French { get; set; }
        public bool Portuguese { get; set; }
        public bool Bengali { get; set; }
        public bool Hindi { get; set; }
        public bool Marathi { get; set; }
        public bool Telugu { get; set; }
        public bool Tamil { get; set; }
        public bool Malayalam { get; set; }

        public bool IsLanguageMatch(string language)
        {
            if (English && language.Contains("English"))
            {
                return true;
            }
            else if (Spanish && language.Contains("Spanish"))
            {
                return true;
            }
            else if (French && language.Contains("French"))
            {
                return true;
            }
            else if (Portuguese && language.Contains("Portuguese"))
            {
                return true;
            }
            else if (Bengali && language.Contains("Bengali"))
            {
                return true;
            }
            else if (Hindi && language.Contains("Hindi"))
            {
                return true;
            }
            else if (Marathi && language.Contains("Marathi"))
            {
                return true;
            }
            else if (Telugu && language.Contains("Telugu"))
            {
                return true;
            }
            else if (Tamil && language.Contains("Tamil"))
            {
                return true;
            }
            else if (Malayalam && language.Contains("Malayalam"))
            {
                return true;
            }
            return false;
        }

        public bool IsEmpty()
        {
            return !English && !Spanish && !French && !Portuguese && !Bengali && !Hindi && !Marathi && !Telugu && !Tamil && !Malayalam;
        }
    }

    public class provider_type
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class State
    {
        public DateTime? deletedAt { get; set; }
        public DateTime? createdAt { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string country_id { get; set; }
        public DateTime? updatedAt { get; set; }
        public int? v { get; set; }
        public string _id { get; set; }
        public int? _v { get; set; }
    }
    public class providerGender
    {
        public bool female { get; set; }
        public bool male { get; set; }

        public bool IsMatch(string gender)
        {
            if (female && gender.Contains("Female"))
            {
                return true;
            }
            else if (male && gender.Contains("Male"))
            {
                return true;
            }
            else if (!female && !male)
            {
                return true;
            }
            return false;
        }

        public bool IsEmpty()
        {
            return !female && !male;
        }
    }

    public class Therapists : ViewModelBase
    {
        public string id { get; set; }
        public string Match { get; set; }
        public string Image { get; set; }
        public string ContactName { get; set; }
        public string ContactType { get; set; }

        private bool _ISChecked;
        public bool ISChecked
        {
            get => _ISChecked;
            set
            {
                if (this._ISChecked == value) return;

                this._ISChecked = value;

                NotifyPropertyChanged();
            }
        }
    }

    public class Search
    {
        public string id { get; set; }
        public bool isBusinessCard { get; set; }
        public string provider_type { get; set; }
        public string name { get; set; }
        public string contact { get; set; }
        public bool isContact { get; set; }
        public string email { get; set; }
        public bool isEmail { get; set; }

        public string address { get; set; }
        public bool isAddress { get; set; }

        public string appUrl { get; set; }
        public string Image { get; set; }
    }
}
