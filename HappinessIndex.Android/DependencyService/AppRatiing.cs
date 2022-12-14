using Android.Content;
using Android.Content.PM;
using Android.Net;
using HappinessIndex.DependencyService;
using HappinessIndex.Droid.DependencyService;

[assembly: Xamarin.Forms.Dependency(typeof(AppRatiing))]
namespace HappinessIndex.Droid.DependencyService
{
    public class AppRatiing : IAppRating
    {
        public void RateApp()
        {
            return;
            var activity = Android.App.Application.Context;
            var url = $"market://details?id={(activity as Context)?.PackageName}";

            try
            {
                activity.PackageManager.GetPackageInfo("com.android.vending", PackageInfoFlags.Activities);
                Intent intent = new Intent(Intent.ActionView, Uri.Parse(url));
                intent.SetFlags(ActivityFlags.NewTask);

                activity.StartActivity(intent);
            }
            catch (PackageManager.NameNotFoundException ex)
            {
                // this won't happen. But catching just in case the user has downloaded the app without having Google Play installed.

                //Console.WriteLine(ex.Message);
            }
            catch (ActivityNotFoundException)
            {
                // if Google Play fails to load, open the App link on the browser 

                var playStoreUrl = "https://play.google.com/store/apps/details?id=com.yourapplicationpackagename"; //Add here the url of your application on the store

                var browserIntent = new Intent(Intent.ActionView, Uri.Parse(playStoreUrl));
                browserIntent.AddFlags(ActivityFlags.NewTask | ActivityFlags.ResetTaskIfNeeded);

                activity.StartActivity(browserIntent);
            }
        }
    }
}
