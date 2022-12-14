using System;
using Android.Widget;
using HappinessIndex.DependencyService;

[assembly: Xamarin.Forms.Dependency(typeof(HappinessIndex.Droid.DependencyService.Toast))]
namespace HappinessIndex.Droid.DependencyService
{
    public class Toast : IToast
    {
        public void Show(string message, double delay)
        {
            if(delay > 2)
            {
                Android.Widget.Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long).Show();
            }
            else
            {
                Android.Widget.Toast.MakeText(Android.App.Application.Context, message, ToastLength.Short).Show();
            }
        }
    }
}