using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CloudKit;
using Foundation;
using HappinessIndex.DependencyService;
using HappinessIndex.iOS.DependencyService;
using Microsoft.AppCenter.Crashes;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(BackUp))]
namespace HappinessIndex.iOS.DependencyService
{
    public class BackUp : IBackUp
    {
        CKDatabase privateDB;

        private const string ReferenceItemRecordName = "Items";
        CKRecordID recordID = new CKRecordID(AppSettings.AssetName);

        bool isInQueue;

        async void IBackUp.BackUp()
        {
            try
            {
                if (!Preferences.Get(AppSettings.EnableBackupKey, false)) return;

                if (isInQueue) return;

                if (privateDB == null)
                {
                    privateDB = CKContainer.DefaultContainer.PrivateCloudDatabase;
                }

                isInQueue = true;
                Device.BeginInvokeOnMainThread(async () =>
                {
                    try
                    {
                        var fileUrl = new NSUrl(AppSettings.LocalDBPath, false);
                        var asset = new CKAsset(fileUrl);

                        await privateDB.DeleteRecordAsync(recordID);

                        var newRecord = new CKRecord(ReferenceItemRecordName, recordID);
                        newRecord["asset"] = asset;

                        await privateDB.SaveRecordAsync(newRecord);

                        isInQueue = false;
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.ToLower().Contains("not authenticated"))
                        {
                            var result = await App.Current.MainPage.DisplayAlert(
                                 App.GetString("BackupFailed"), App.GetString("PleaseSigningUsingYouriCloudAccount"),
                                 App.GetString("Ok"), App.GetString("DisableBackup"));

                            if (!result)
                            {
                                Preferences.Set(AppSettings.EnableBackupKey, false);
                            }
                        }

                        var properties = new Dictionary<string, string>
                                {
                                { "User", Preferences.Get(AppSettings.EmailKey, "") },
                                };
                        Crashes.TrackError(ex, properties);

                        isInQueue = false;
                    }
                });
            }

            catch (Exception ex)
            {
                var properties = new Dictionary<string, string>
                        {
                         { "User", Preferences.Get(AppSettings.EmailKey, "") },
                         };
                Crashes.TrackError(ex, properties);

                //Preferences.Set(AppSettings.EnableBackupKey, false);
                isInQueue = false;
            }
        }

        public async Task Fetch()
        {
            try
            {
                if (privateDB == null)
                {
                    privateDB = CKContainer.DefaultContainer.PrivateCloudDatabase;
                }

                var result = await privateDB.FetchRecordAsync(recordID);

                var db = result["asset"] as CKAsset;

                if (File.Exists(AppSettings.LocalDBPath))
                {
                    File.Delete(AppSettings.LocalDBPath);
                }

                File.Copy(db.FileUrl.Path, AppSettings.LocalDBPath);
            }
            catch (Exception exception)
            {
                var properties = new Dictionary<string, string>
                        {
                         { "User", Preferences.Get(AppSettings.EmailKey, "") },
                         };
                Crashes.TrackError(exception, properties);

                //await App.Current.MainPage.DisplayAlert(ex.Message, "", "OK");
                //Preferences.Set(AppSettings.EnableBackupKey, false);
            }
        }

        public async void DeleteBackup()
        {
            try
            {
                if (!Preferences.Get(AppSettings.EnableBackupKey, false)) return;

                if (privateDB == null)
                {
                    privateDB = CKContainer.DefaultContainer.PrivateCloudDatabase;
                }

                if (isInQueue) return;

                isInQueue = true;

                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    var fileUrl = new NSUrl(AppSettings.LocalDBPath, false);
                    var asset = new CKAsset(fileUrl);

                    privateDB.FetchRecord(recordID, async (record, error) =>
                    {
                        if (record != null)
                        {
                            await privateDB.DeleteRecordAsync(recordID);
                        }
                        isInQueue = false;
                    });
                });
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string>
                        {
                         { "User", Preferences.Get(AppSettings.EmailKey, "") },
                         };

                Crashes.TrackError(ex, properties);

                Preferences.Set(AppSettings.EnableBackupKey, false);
                isInQueue = false;
            }
        }
    }
}