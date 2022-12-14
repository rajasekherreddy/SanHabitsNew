using System;
using System.Diagnostics;
using System.Threading.Tasks;
using HappinessIndex.DependencyService;
using HappinessIndex.ViewModels;
using Newtonsoft.Json.Linq;
using Plugin.FacebookClient;
using Plugin.GoogleClient;
using Plugin.GoogleClient.Shared;
using Xamarin.Essentials;

namespace HappinessIndex.Helpers
{
    public static class SocialLogin
    {
        public static async Task LoginAppleAsync(IAppleSignInService appleSignInService)
        {
            try
            {
                var account = await appleSignInService.SignInAsync();
                if (account != null)
                {
                    App.LoginType = "Apple";
                    await SecureStorage.SetAsync("AppleUserIdKey", account.UserId);
                    await SignupPageViewModel.Signup(
                        account.Email,
                        account.Name,
                        forceLogin: true,
                        profilePhoto: "",
                        loginMedium: "Apple",
                        appleID:account.UserId);
                }
            }
            catch (Exception)
            {

            }
        }

        public static async Task LoginFacebookAsync(IFacebookClient facebookService)
        {
            try
            {
                App.LoginType = "Facebook";

                if (facebookService.IsLoggedIn)
                {
                    facebookService.Logout();
                }

                EventHandler<FBEventArgs<string>> userDataDelegate = null;

                userDataDelegate = async (object sender, FBEventArgs<string> e) =>
                {
                    if (e == null) return;

                    switch (e.Status)
                    {
                        case FacebookActionStatus.Completed:
                            var data = await Task.Run(() => JObject.Parse(e.Data));
                            await SignupPageViewModel.Signup(data["email"].ToString(), data["name"].ToString(), forceLogin: true, profilePhoto: "http://graph.facebook.com/" + data["id"] + "/picture?type=large", loginMedium: "Facebook");
                            break;
                        case FacebookActionStatus.Canceled:
                            break;
                    }

                    facebookService.OnUserData -= userDataDelegate;
                };

                facebookService.OnUserData += userDataDelegate;

                string[] fbRequestFields = { "email", "name", "id" };
                string[] fbPermisions = { "email" };
                await facebookService.RequestUserDataAsync(fbRequestFields, fbPermisions);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: ", ex.Message);
            }
        }

        public static async Task LoginGoogleAsync(IGoogleClientManager googleService)
        {
            try
            {
                App.LoginType = "Google";

                if (!string.IsNullOrEmpty(googleService.AccessToken))
                {
                    //Always require user authentication
                    googleService.Logout();
                }

                EventHandler<GoogleClientResultEventArgs<GoogleUser>> userLoginDelegate = null;
                userLoginDelegate = async (object sender, GoogleClientResultEventArgs<GoogleUser> e) =>
                {
                    switch (e.Status)
                    {
                        case GoogleActionStatus.Completed:
                            await SignupPageViewModel.Signup(e.Data.Email, e.Data.Name,
                                forceLogin: true, profilePhoto: e.Data.Picture.AbsoluteUri, loginMedium: "Google");
                            break;
                        case GoogleActionStatus.Canceled:
                            await App.Current.MainPage.DisplayAlert("Google Auth", "Canceled", "Ok");
                            break;
                        case GoogleActionStatus.Error:
                            await App.Current.MainPage.DisplayAlert("Google Auth", "Error", "Ok");
                            break;
                        case GoogleActionStatus.Unauthorized:
                            await App.Current.MainPage.DisplayAlert("Google Auth", "Unauthorized", "Ok");
                            break;
                    }

                    googleService.OnLogin -= userLoginDelegate;
                };

                googleService.OnLogin += userLoginDelegate;

                await googleService.LoginAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }
}