using System;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using BuildHappiness.Core.Helpers;
using BuildHappiness.Core.Models;
using System.Collections.Generic;
using BuildHappiness.Core.Common;
using Firebase.Auth;

namespace HappinessIndex.Data.CloudData
{
    public class FirebaseService : ICloudService
    {
        FirebaseClient _firebase;

        FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyAhsfDMHP1ywZledu6Ed5P01C2-QnNj3oc"));

        private readonly string SPRequestDB = "sprequest";
        private readonly string ApprovedSBDB = "approvedsp";
        private readonly string RejectedSBDB = "rejectedsp";
        private readonly string UserDataDB = "userdatasp";

        private async Task<FirebaseClient> GetFirebaseClient()
        {
            if(_firebase == null)
            {
                var result = await authProvider.SignInAnonymouslyAsync();

                _firebase = new FirebaseClient("https://build-happiness.firebaseio.com/", new FirebaseOptions { AuthTokenAsyncFactory = async () => { return result.FirebaseToken; } });
            }

            return _firebase;
        }

        public async Task<ServiceProvider> GetServiceProviderForEdit(string email)
        {
            try
            {
                var emailKey = email.RemoveSpecialCharacters().ToLower();
                var firebaseClient = await GetFirebaseClient();

                var requestDB = await firebaseClient.Child(SPRequestDB).Child(emailKey).OnceSingleAsync<ServiceProvider>();
                if (requestDB != null)
                {
                    return requestDB;
                }

                var approvedDB = await firebaseClient.Child(ApprovedSBDB).Child(emailKey).OnceSingleAsync<ServiceProvider>();
                if (approvedDB != null)
                {
                    return approvedDB;
                }

                var rejectDB = await firebaseClient.Child(RejectedSBDB).Child(emailKey).OnceSingleAsync<ServiceProvider>();
                if (rejectDB != null)
                {
                    return rejectDB;
                }
            }
            catch (Exception e)
            {

            }
            return null;
        }

        public async Task<List<ServiceProvider>> GetUserDataWithServiceProvider(string email)
        {
            try
            {
                var firebaseClient = await GetFirebaseClient();

                var userData = await GetUserData(email);

                var serviceProviders = new List<ServiceProvider>();

                foreach (var item in userData.ServiceProviders)
                {
                    serviceProviders.Add(await firebaseClient.Child(ApprovedSBDB).Child(item.RemoveSpecialCharacters().ToLower()).OnceSingleAsync<ServiceProvider>());
                }
                return serviceProviders;
            }
            catch (Exception e)
            {

            }

            return null;
        }

        public async Task<UserData> GetUserData(string email)
        {
            try
            {
                var firebaseClient = await GetFirebaseClient();

                var userData = await firebaseClient.Child(UserDataDB).Child(email.RemoveSpecialCharacters().ToLower()).OnceSingleAsync<UserData>();

                return userData;
            }
            catch (Exception e)
            {

            }

            return null;
        }

        public async Task<int> SaveUserDataWithServiceProvider(string email, UserData data)
        {
            try
            {
                var firebaseClient = await GetFirebaseClient();

                await firebaseClient.Child(UserDataDB).Child(email.RemoveSpecialCharacters().ToLower()).PutAsync(data);

                return 1;
            }
            catch (Exception e)
            {
                GlobalClass.ShowToastMessage("Unauthorized");
            }

            return 0;
        }

        public async Task<List<ServiceProvider>> GetValidServiceProvidersByCountry(string country)
        {
            try
            {
                var firebaseClient = await GetFirebaseClient();

                var approvedProviders = await firebaseClient.Child(ApprovedSBDB).OrderBy("Country").EqualTo("India").OnceAsync<ServiceProvider>();

                var list = new List<ServiceProvider>();

                foreach (var item in approvedProviders)
                {
                    list.Add(item.Object);
                }

                return list;
            }
            catch (Exception e)
            {

            }
            return null;
        }

        public async Task<List<ServiceProvider>> GetValidServiceProviders()
        {
            try
            {
                var firebaseClient = await GetFirebaseClient();

                var approvedProviders = await firebaseClient.Child(ApprovedSBDB).OnceAsync<ServiceProvider>();

                var list = new List<ServiceProvider>();

                foreach (var item in approvedProviders)
                {
                    list.Add(item.Object);
                }

                return list;
            }
            catch (Exception e)
            {

            }
            return null;
        }

        public async Task<int> SubmitServiceProviderForReview(ServiceProvider data)
        {
            try
            {
                var firebaseClient = await GetFirebaseClient();

                var emailKey = data.Email.RemoveSpecialCharacters();

                await firebaseClient.Child(SPRequestDB).Child(emailKey).PutAsync(data);

                return 1;
            }
            catch (Exception e)
            {
                GlobalClass.ShowToastMessage("Unauthorized");
            }
            return 0;
        }
    }
}