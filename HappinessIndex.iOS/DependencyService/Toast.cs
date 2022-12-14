using System;
using Foundation;
using HappinessIndex.DependencyService;
using HappinessIndex.iOS.DependencyService;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(Toast))]
namespace HappinessIndex.iOS.DependencyService
{
    public class Toast : IToast
    {
        NSTimer alertDelay;

        UIAlertController alert;

        public void Show(string message, double delay)
        {
            ShowAlert(message, delay);
        }

        void ShowAlert(string message, double seconds)
        {
            alertDelay = NSTimer.CreateScheduledTimer(seconds, (obj) =>
            {
                DismissMessage();
            });

            alert = UIAlertController.Create(null, message, UIAlertControllerStyle.Alert);
            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alert, true, null);
        }

        void DismissMessage()
        {
            if (alert != null)
            {
                alert.DismissViewController(true, null);
            }
            if (alertDelay != null)
            {
                alertDelay.Dispose();
            }
        }

    }
}