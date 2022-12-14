using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HappinessIndex.Models;

namespace HappinessIndex.Data
{
    public interface IDataService
    {
        Task InvalidateConnection();

        Task<bool> RegisterUserAsync(User user);

        Task<User> GetUserAsync(string email);

        Task<User> GetUserByAppleIDAsync(string email);

        Task<List<User>> GetAllUsersAsync();

        Task<User> Login(string email, string password);

        Task<bool> IsRegisteredUser(string email);

        Task<int> UpdateUserAsync(User user);

        Task<List<Journal>> GetAllJounralsAsync(DateTime start, DateTime end);

        Task<int> UpdateJounralsAsync(List<Journal> journals);

        Task<List<Factor>> GetAllFactors(User user);

        Task<List<MicroWorkout>> GetAllMicroWorkout(User user);

        Task<List<Journal>> GetJournalsAsync(DateTime date);

        Task<int> DeleteJournalAsync(Journal journal);

        Task<OveralScore> GetOverallScoreAsync(DateTime date);

        Task<List<OveralScore>> GetOverallScoreAsync(DateTime start, DateTime end);

        Task<int> UpdateOverallScoreAsync(OveralScore overalScore);

        Task<Highlights> GetHighlightsAsync(DateTime date);

        Task<List<Highlights>> GetHighlightsAsync();

        Task<int> UpdateHighlightsAsync(Highlights highlights);

        Task<int> UpdateUserDataAsync(User user);

        Task<PersonalNote> GetPersonalNoteAsync(DateTime date);

        Task<int> SetPersonalNoteAsync(DateTime date, string content);

        Task<List<NegativeFactor>> GetNegativeFactorAsync(DateTime date);

        Task<List<NegativeFactor>> GetNegativeFactorAsync(DateTime start, DateTime end);

        Task<int> SetNegativeFactorAsync(List<NegativeFactor> negativeFactors);

        Task<int> AddWishlistAsync(List<Wishlist> wishlist);

        Task<List<Wishlist>> GetWishlistAsync(int factorID);

        //Task<int> AddWishlistMicroAsync(List<WishlistMicro> wishlist);

        //Task<List<WishlistMicro>> GetWishlistMicroAsync(int factorID);

        Task<bool> ProvidersSave(Providers Providers, provider_specialities provider_Specialities, providerLanguage language);

        Task<List<Providers>> GetAllProvidersByTypeAsync(string provider_type);
        Task<Providers> GetProviderAsync(string id);
        Task<provider_specialities> Getprovider_specialitiesAsync(string id);
        Task<providerLanguage> Getprovider_languageAsync(string id);

        Task<int> UpdateAllMicroWoroutAsync(List<MicroWorkout> microWorkouts);

        Task<int> InsertAllMicroWoroutAsync(List<MicroWorkout> microWorkouts);

        Task<int> AddMicroWoroutAsync(MicroWorkout microWorkout );

        public Task<int> UpdateMicroWoroutAsync(MicroWorkout microWorkout);

        public Task<DateTime> GetLastWorkoutDate();

        void deleteAllUsersAsync();


    }
}