using System;
using System.Collections.Generic;
using HappinessIndex.ViewModels;
using Xamarin.Forms;

namespace HappinessIndex.Views
{
    [QueryProperty("Email", "email")]
    [QueryProperty("ResetUsingPassword", "resetUsingPassword")]
    public partial class ResetPasswordPage : ContentPage
    {
        public string Email { get; set; }

        public string ResetUsingPassword { get; set; }

        public ResetPasswordPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            bool.TryParse(ResetUsingPassword, out bool resetUsingPassword);

            if (resetUsingPassword)
            {
                TitleLabel.IsVisible = false;
                InputBox.Hint = "Enter Old Password";
                AppTour.IsVisible = false;
                AnswerEntry.IsPassword = true;
                BackToLoginLabel.IsVisible = false;

                BindingContext = new ResetPasswordPageViewModel(Email, bool.Parse(ResetUsingPassword));
                base.OnAppearing();
            }
            else
            {
                var height = Application.Current.MainPage.Height;
                MainContent.TranslationY = height;

                base.OnAppearing();

                MainContent.TranslateTo(0, 0, 250);
            }
        }
    }
}
