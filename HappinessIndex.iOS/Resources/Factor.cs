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
    public class Wishlist
    {
        private string description;

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public int AssociatedID { get; set; }

        public string Name { get; set; }

        public string Description
        {
            get => description; set
            {
                if(description != value)
                {
                    description = value;
                    if(Factor != null)
                    {
                        Factor.IsWishlistUpdated = true;
                    }
                }
            }
        }

        [Ignore]
        public Factor Factor { get; set; }
    }

    public class Factor : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [Ignore]
        public Command ShareCommand { get; set; }

        [Ignore]
        public bool IsWishlistUpdated { get; set; }

        private bool isSelect;

        public string Icon { get; set; }

        public string Name { get; set; }

        List<Wishlist> wishlist;

        [Ignore]
        public List<Wishlist> Wishlist
        {
            get
            {
                if (wishlist != null && wishlist.Count > 2)
                {
                    return wishlist;
                }

                Task.Run(async () =>
                {
                    wishlist = await ViewModelBase.DataService.GetWishlistAsync(ID);

                    foreach (var item in wishlist)
                    {
                        item.Factor = this;
                    }
                }).Wait();

                if (wishlist == null || wishlist.Count < 3)
                {
                    wishlist = new List<Wishlist>();
                    wishlist.Add(new Wishlist { Name = AppResources.Wishlist1, Factor = this, AssociatedID = ID });
                    wishlist.Add(new Wishlist { Name = AppResources.Wishlist2, Factor = this, AssociatedID = ID });
                    wishlist.Add(new Wishlist { Name = AppResources.Wishlist3, Factor = this, AssociatedID = ID });
                };

                return wishlist;
            }
        }

        [Ignore]
        public string DisplayName
        {
            get
            {
                var localSting = AppResources.ResourceManager.GetString(Name);
                if (string.IsNullOrEmpty(localSting))
                {
                    return Name;
                }
                else
                {
                    return localSting;
                }
            }
        }

        public bool NeedToSave
        {
            get => needToSave;
            set
            {
                needToSave = value;

                NotifyPropertyChanged();
            }
        }

        public string Description { get; set; }

        [Ignore]
        public string DisplayDescription
        {
            get
            {
                if (Description == null) return "";

                var localSting = AppResources.ResourceManager.GetString(Description);
                if (string.IsNullOrEmpty(localSting))
                {
                    return Description;
                }
                else
                {
                    return localSting;
                }
            }
        }

        public bool IsCustomFactor { get; set; }

        [Ignore]
        public bool IsSelected
        {
            get => isSelect;
            set
            {
                if (isSelect != value)
                {
                    isSelect = value;
                    if (!pauseNotifyPropertyChanged)
                        NotifyPropertyChanged();
                }
            }
        }

        Color? color = null;

        [Ignore]
        public Color Color
        {
            get
            {
                if (color == null)
                {
                    color = ColorHelper.GetColor(ID);
                }
                return color.Value;
            }
        }

        public Factor()
        {
            ShareCommand = new Command(ShareWishlist);
        }

        private bool pauseNotifyPropertyChanged;
        private bool needToSave;

        public void PauseNotifyPropertyChanged()
        {
            pauseNotifyPropertyChanged = true;
        }

        public void ResumeNotifyPropertyChanged()
        {
            pauseNotifyPropertyChanged = false;
        }

        private async void ShareWishlist()
        {
            var wishlist = await ViewModelBase.DataService.GetWishlistAsync(ID);

            bool isChanged = false;

            if(wishlist != null && wishlist.Count > 2 && this.wishlist != null && this.wishlist.Count > 2)
            {
                for (int i = 0; i < wishlist.Count; i++)
                {
                    if(this.wishlist[i].Description != wishlist[i].Description)
                    {
                        isChanged = true;
                        break;
                    }
                }
            }
            else
            {
                isChanged = true;
            }

            NeedToSave = isChanged;

            if (isChanged)
            {
                //await Application.Current.MainPage.DisplayAlert("", AppResources.SaveMessage, AppResources.Ok);
                await PopupNavigation.Instance.PushAsync(new CommonMessage("", AppResources.SaveMessage, AppResources.Ok));
            }
            else
            {
                //Share the wishlist
                var text = "Message from Build Happiness app \nHappiness Enabler : " + DisplayName + "\nWishlist 1 : " + wishlist[0].Description
                + "\nWishlist 2 : " + wishlist[1].Description + "\nWishlist 3 : " + wishlist[2].Description;

                await Share.RequestAsync(new ShareTextRequest
                {
                    Text = text,
                    Title = "Share your wishlist"
                });
            }
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