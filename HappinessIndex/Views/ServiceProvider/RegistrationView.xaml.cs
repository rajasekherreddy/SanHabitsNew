using HappinessIndex.ViewModels;
using HappinessIndex.Views.Popup;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HappinessIndex.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationView : ContentPage
    {
        public RegistrationView()
        {
            InitializeComponent();
        }

        async void ChooseLanguage(object sender, EventArgs e)
        {
            var actualText = langEntry.Text;
            var selectedItems = new string[1] { "" };
            if (actualText != null)
            {
                selectedItems = actualText.Split(',');
            }

            var popup = new LanguagePopup(new string[] {"English", "Spanish", "French", "Portuguese", "Bengali", "Hindi", "Marathi", "Telugu", "Tamil", "Malayalam" },
                selectedItems);

            popup.SaveClicked += (sender, arg) =>
            {
                langEntry.Text = sender.ToString(); ;
            };

            await PopupNavigation.Instance.PushAsync(popup);
        }

        async void ChooseSpecialities(object sender, EventArgs e)
        {
            var actualText = specialitiesEntry.Text;
            var selectedItems = new string[1] { "" };
            if (actualText != null)
            {
                selectedItems = actualText.Split(',');
            }
            
            var popup = new LanguagePopup(new string[] { "Adoption", "Anger Management", "Anxiety", "Autism Spectrum", "Behavioral Issues",
            "Chronic lllness or pain", "Depression", "Domestic Abuse or Violence", "Men's Issues", "Parenting", "Sleep Problems or Insomnia",
            "Spirituality", "Stress Management", "Suicidal Ideation", "Trauma and PTSD", "Weight Loss", "Women's Issues", "Teenager Issues", "ADHD"}, selectedItems);

            popup.SaveClicked += (sender, arg) =>
            {
                specialitiesEntry.Text = sender.ToString(); ;
            };

            await PopupNavigation.Instance.PushAsync(popup);
        }
    }
}