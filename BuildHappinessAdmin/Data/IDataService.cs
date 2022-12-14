using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BuildHappiness.Core.Models;

namespace BuildHappinessAdmin.Data
{
    public interface IDataService
    {
        Task<int> Login(string username, string password);

        Task<List<ServiceProvider>> GetPendingServiceProviderRequest();

        Task<int> ApproveServiceProvider(ServiceProvider serviceProvider);

        Task<int> DeclineServiceProvider(ServiceProvider serviceProvider);
    }
}
