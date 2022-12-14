using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Xamarin.Essentials;

namespace BuildHappiness.Core.Common
{
    public class GlobalClass
    {
        public static string BaseUrl = "http://ec2-3-135-225-41.us-east-2.compute.amazonaws.com:3000/";

        public static void ShowLoadingBar()
        {
            UserDialogs.Instance.ShowLoading("Processing...", MaskType.Gradient);
        }

        public static void HideLoadingBar()
        {
            UserDialogs.Instance.HideLoading();
        }
        public static void ShowToastMessage(string Message)
        {
            UserDialogs.Instance.Toast(Message);
        }
        public static void ShowAlertMessage(string Message)
        {
            UserDialogs.Instance.Alert(Message);
        }
        public static async Task<bool> LocationPermission()
        {
            var locationPermissionStatus = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (locationPermissionStatus != PermissionStatus.Granted)
                locationPermissionStatus = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

            bool permission;
            if (locationPermissionStatus == PermissionStatus.Granted)
                permission = true;
            else
                permission = false;

            return permission;
        }

        public static async Task<bool> NetworkPermission()
        {
            var networkStatePermissionStatus = await Permissions.CheckStatusAsync<Permissions.NetworkState>();
            if (networkStatePermissionStatus != PermissionStatus.Granted)
                networkStatePermissionStatus = await Permissions.RequestAsync<Permissions.NetworkState>();

            bool permission;
            if (networkStatePermissionStatus == PermissionStatus.Granted)
                permission = true;
            else
                permission = false;

            return permission;
        }

    }
}