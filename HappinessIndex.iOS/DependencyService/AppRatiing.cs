using System;
using Foundation;
using HappinessIndex.DependencyService;
using HappinessIndex.iOS.DependencyService;
using StoreKit;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(AppRatiing))]
namespace HappinessIndex.iOS.DependencyService
{
    public class AppRatiing : IAppRating
    {
        public void RateApp()
        {
            return;

            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 3))
                SKStoreReviewController.RequestReview();
            else
            {
                var storeUrl = "itms-apps://itunes.apple.com/app/1503903016";
                var url = storeUrl + "?action=write-review";

                try
                {
                    UIApplication.SharedApplication.OpenUrl(new NSUrl(url));
                }
                catch (Exception ex)
                {
                    // Here you could show an alert to the user telling that App Store was unable to launch

                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
