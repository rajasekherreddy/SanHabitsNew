using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using HappinessIndex.Resx;
using HappinessIndex.Views;
using HappinessIndex.Views.Popup;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HappinessIndex.ViewModels
{
    public class LogoutPageViewModel : ViewModelBase
    {
        public Command SendEmailCommand { get; set; }

        public LogoutPageViewModel()
        {
            SendEmailCommand = new Command(SendEmail);
        }

        private async void SendEmail()
        {
            await SendEmail(AppResources.FeedbackOnBuildHappinessApp, "", new List<String> { "feedback@buildhappiness.app" });
        }

        public async Task SendEmail(string subject, string body, List<string> recipients)
        {
            try
            {
                var message = new EmailMessage
                {
                    Subject = subject,
                    Body = body,
                    To = recipients,
                };
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException)
            {
                //await Application.Current.MainPage.DisplayAlert("", AppResources.EmailIsNotSupportedOnThisDevice, AppResources.Ok);
                await PopupNavigation.Instance.PushAsync(new CommonMessage("", AppResources.EmailIsNotSupportedOnThisDevice, AppResources.Ok));
            }
            catch (Exception)
            {
                // Some other exception occurred
            }
        }

    }
}