using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Xamarin.Forms;
using FFImageLoading.Forms.Platform;
using Plugin.LocalNotification;
using Android.Content;
using Android.App.Backup;
using Plugin.FacebookClient;
using Plugin.GoogleClient;
using Acr.UserDialogs;
using Plugin.FirebasePushNotification;

namespace HappinessIndex.Droid
{
    [Activity(Label = "@string/App_name", Icon = "@drawable/Icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        internal static MainActivity Instance { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            
            FacebookClientManager.Initialize(this);
            GoogleClientManager.Initialize(this, null, "292712146644-jk87n6q4c7teiri9ts4kcc0k5doslq85.apps.googleusercontent.com");

            Instance = this;

            Forms.SetFlags("IndicatorView_Experimental", "Expander_Experimental");
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Forms.Init(this, savedInstanceState);
            Forms.Init(this, savedInstanceState);
            UserDialogs.Init(this);

            CachedImageRenderer.Init(enableFastRenderer: true);
            CachedImageRenderer.InitImageViewHandler();
            NotificationCenter.CreateNotificationChannel();

            LoadApplication(new App());
            FirebasePushNotificationManager.Initialize(this, true, true);

            NotificationCenter.NotifyNotificationTapped(Intent);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent intent)
        {
            base.OnActivityResult(requestCode, resultCode, intent);
            if (App.LoginType == "Google")
            {
                GoogleClientManager.OnAuthCompleted(requestCode, Result.Ok, intent);
            }
            else
            {
                FacebookClientManager.OnActivityResult(requestCode, resultCode, intent);
            }
        }

        protected override void OnNewIntent(Intent intent)
        {
            NotificationCenter.NotifyNotificationTapped(intent);
            base.OnNewIntent(intent);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    public class MyBackupAgent : BackupAgentHelper
    {
        public override void OnCreate()
        {
            base.OnCreate();

            try
            {
                FileBackupHelper database = new FileBackupHelper(this, AppSettings.LocalDBPath);
                AddHelper("DATABASE_backup", database);
            }
            catch
            {

            }
        }
    }
}