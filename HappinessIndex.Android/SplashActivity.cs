using System;
using Android.App;
using Android.OS;
using Android.Support.V7.App;

namespace HappinessIndex.Droid
{
    [Activity(Label = "@string/App_name", Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true, Icon = "@drawable/Icon")]
    public class SplashActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.StartActivity(typeof(MainActivity));
        }
    }
}
