using System;
using System.Threading.Tasks;
using Android.App;
using Android.App.Backup;
using HappinessIndex.DependencyService;
using HappinessIndex.Droid.DependencyService;
using Xamarin.Essentials;

[assembly: Xamarin.Forms.Dependency(typeof(BackUp))]
namespace HappinessIndex.Droid.DependencyService
{
    public class BackUp : IBackUp
    {
        RestoreObserverExt observer = new RestoreObserverExt();
        BackupManager backupManager = new BackupManager(Application.Context);

        public void DeleteBackup()
        {

        }

        public async Task Fetch()
        {
            if (!Preferences.Get(AppSettings.EnableBackupKey, false)) return;
            if (!Preferences.Get(AppSettings.EnableBackupKey, true)) ;
            //backupManager.RequestRestore(observer);
        }

        void IBackUp.BackUp()
        {
            if (!Preferences.Get(AppSettings.EnableBackupKey, false)) return;

            BackupManager.DataChanged("com.sanhabits.app");
        }
    }

    class RestoreObserverExt : RestoreObserver
    {
        public override void RestoreStarting(int numPackages)
        {
            base.RestoreStarting(numPackages);
        }

        public override void RestoreFinished(int error)
        {
            base.RestoreFinished(error);
        }
    }
}