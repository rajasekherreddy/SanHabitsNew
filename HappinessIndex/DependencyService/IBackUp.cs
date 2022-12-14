using System;
using System.Threading.Tasks;

namespace HappinessIndex.DependencyService
{
    public interface IBackUp
    {
        Task Fetch();

        void BackUp();

        void DeleteBackup();
    }
}
