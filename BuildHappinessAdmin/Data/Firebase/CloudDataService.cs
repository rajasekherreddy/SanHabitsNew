using System;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Auth;
using System.Collections.Generic;
using BuildHappiness.Core.Models;
using BuildHappiness.Core.Helpers;
using Firebase.Database.Query;

namespace BuildHappinessAdmin.Data.Firebase
{
    public class CloudDataService : IDataService
    {
        FirebaseClient firebase;

        FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyAhsfDMHP1ywZledu6Ed5P01C2-QnNj3oc"));

        private readonly string SPRequestDB = "sprequest";
        private readonly string ApprovedSBDB = "approvedsp";
        private readonly string RejectedSBDB = "rejectedsp";

        public CloudDataService()
        {

        }

        public async Task<int> Login(string username, string password)
        {
            try
            {
                var result = await authProvider.SignInWithEmailAndPasswordAsync(username, password);
                firebase = new FirebaseClient("https://build-happiness.firebaseio.com/", new FirebaseOptions { AuthTokenAsyncFactory = async () => { return result.FirebaseToken; } });
                return 1;
            }
            catch (Exception e)
            {

            }

            return 0;
        }

        public async Task<List<ServiceProvider>> GetPendingServiceProviderRequest()
        {
            try
            {
                var items = await firebase.Child(SPRequestDB).OnceAsync<ServiceProvider>();

                var serviceProviders = new List<ServiceProvider>();
                foreach (var item in items)
                {
                    serviceProviders.Add(item.Object);
                }

                return serviceProviders;
            }
            catch (Exception e)
            {

            }
            return null;
        }

        public async Task<int> ApproveServiceProvider(ServiceProvider serviceProvider)
        {
            try
            {
                var emailKey = serviceProvider.Email.RemoveSpecialCharacters().ToLower();

                firebase.Child(SPRequestDB).Child(emailKey).DeleteAsync();

                firebase.Child(ApprovedSBDB).Child(emailKey).PutAsync(serviceProvider);
            }
            catch (Exception e)
            {

            }
            return 0;
        }

        public async Task<int> DeclineServiceProvider(ServiceProvider serviceProvider)
        {
            try
            {
                var emailKey = serviceProvider.Email.RemoveSpecialCharacters().ToLower();

                firebase.Child(SPRequestDB).Child(emailKey).DeleteAsync();

                firebase.Child(RejectedSBDB).Child(emailKey).PutAsync(serviceProvider);
            }
            catch (Exception e)
            {

            }
            return 0;
        }
    }
}