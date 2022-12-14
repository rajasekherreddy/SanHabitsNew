using System;
using Android.App;
using Firebase.Iid;
using Firebase.Messaging;

namespace HappinessIndex.Droid.Services
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class FirebaseInstanceIDService : FirebaseMessagingService
    {
        const string TAG = "MyFirebaseIIDService";

        public override void OnNewToken(string p0)
        {
            // Get updated InstanceID token.
            var refreshedToken = p0;
            Android.Util.Log.Debug(TAG, "Refreshed token: " + refreshedToken);
            System.Diagnostics.Debug.WriteLine($"######Token######  :  {refreshedToken}");
            Xamarin.Forms.Application.Current.Properties["Fcmtocken"] = refreshedToken ?? "";
            Xamarin.Forms.Application.Current.SavePropertiesAsync();
        }
    }
}