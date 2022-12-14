using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BuildHappiness.Core.Models;
using HappinessIndex.Models;
using HappinessIndex.ViewModels;

namespace HappinessIndex.Data
{
    public interface ICloudService
    {
        Task<int> SubmitServiceProviderForReview(ServiceProvider data);

        Task<ServiceProvider> GetServiceProviderForEdit(string email);

        Task<List<ServiceProvider>> GetValidServiceProvidersByCountry(string country);

        Task<List<ServiceProvider>> GetValidServiceProviders();

        Task<int> SaveUserDataWithServiceProvider(string email, UserData data);

        Task<List<ServiceProvider>> GetUserDataWithServiceProvider(string email);

        Task<UserData> GetUserData(string email);
    }
}