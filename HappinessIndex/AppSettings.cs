using HappinessIndex.Models;
using System;
using System.IO;
using Xamarin.Forms;

namespace HappinessIndex
{
    public static class AppSettings
    {
        public static string EmailListKey { get; set; } = "email_list";
        public static string PreferredNotificationTimeKey { get; set; } = "preferred_notification_time";
        public static string EmailKey { get; set; } = "email";
        public static string NameKey { get; set; } = "name";
        public static string UserIDKey { get; set; } = "user_id";
        public static string StartDateKey { get; set; } = "start_date";
        public static string ReadPrivacyKey { get; set; } = "read_privacy";
        public static string NotificationRegisteredDateKey { get; set; } = "notification_registered";
        public static string IsEnabledLightBackgroundKey { get; set; } = "light_background_enabled";
        public static string AssetName { get; set; } = "HappinessIndex.db";
        public static string EnableBackupKey { get; set; } = "enable_backup";
        public static string ReportTypeKey { get; set; } = "report_type";
        public static string LanguageKey { get; set; } = "language";

        public static DateTime JournalDate { get; set; }
        public static providerFilter ProviderFilter { get; set; }
        public static string TherapyId { get; set; }

        //public static string LocalDBPath { get; set; } = Device.RuntimePlatform == "Android" ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "HappinessIndex.db") : "";

        public static string LocalDBPath { get; set; } = Device.RuntimePlatform == "Android" ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "HappinessIndex.db") :
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "HappinessIndex.db");
    }
}