using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HappinessIndex.Models;
using SQLite;
using System.Linq;
using BuildHappiness.Core.Helpers;
using Xamarin.Essentials;
using HappinessIndex.DependencyService;

namespace HappinessIndex.Data.LocalData
{
    public class LocalDataService : IDataService
    {
        SQLiteAsyncConnection db;

        public LocalDataService()
        {
            //Init();
        }

        public async Task InvalidateConnection()
        {
            if (db == null)
            {
                db = new SQLiteAsyncConnection(AppSettings.LocalDBPath);
                await Init();
            }
        }

        private async Task Init()
        {
            try
            {
                //TODO: Access internal static property instead of preferences everytime.
                //TODO: Try to improve performance by checking whether the table alreayd exist;
                db.CreateTableAsync<User>().Wait();
                db.CreateTableAsync<Journal>().Wait();
                db.CreateTableAsync<OveralScore>().Wait();
                db.CreateTableAsync<Highlights>().Wait();
                db.CreateTableAsync<NegativeFactor>().Wait();
                db.CreateTableAsync<PersonalNote>().Wait();
                db.CreateTableAsync<Wishlist>().Wait();
                 db.CreateTableAsync<MicroWorkout>().Wait();
                db.CreateTableAsync<Providers>().Wait();
                db.CreateTableAsync<provider_specialities>().Wait();
                db.CreateTableAsync<providerLanguage>().Wait();

                var result = await db.CreateTableAsync<Factor>();

                if (result.ToString() == "Created")
                {
                    List<Factor> defaultFactors = new List<Factor>()
                    {
                        new Factor() { Name = "Exercise", Icon = "Exercise.png"},
                        new Factor() { Name = "Family", Icon = "Family.png"},
                        new Factor() { Name = "Food", Icon = "Food.png"},
                        new Factor() { Name = "Social Contact", Icon = "Social.png"},
                        new Factor() { Name = "Love", Icon = "Romance.png"},
                        new Factor() { Name = "Work", Icon = "Work.png"},
                        new Factor() { Name = "Tranquility" ,Description = "MeditationPeaceSpirituality", Icon = "Tranquility.png"},
                        new Factor() { Name = "Money", Icon = "Money.png"},
                        new Factor() { Name = "Travelling", Icon = "Travelling.png"},
                        new Factor() { Name = "Curiosity",Description = "SearchForKnowledgeIntellectual", Icon = "Curiosity.png"},
                        new Factor() { Name = "Order", Description="OrganizationCleanliness", Icon = "Order.png"},
                        new Factor() { Name = "Music/Movies 123", Icon = "Movie.png"},
                        new Factor() { Name = "House Pets", Icon = "Pets.png"}
                    };
                    await db.InsertAllAsync(defaultFactors);
                }
                else
                {
                    var factors = await db.Table<Factor>().ToListAsync();
                    var factor = factors.Where(item => item.Name == "House Pets").FirstOrDefault();
                    if (factor == null)
                    {
                        await db.InsertAsync(new Factor() { Name = "House Pets", Icon = "Pets.png" });
                    }
                }


              
            }
            catch (Exception ex)
            {

            }
        }

        //GET
        public async Task<List<Journal>> GetAllJounralsAsync(DateTime start, DateTime end)
        {
            await InvalidateConnection();

            var userID = Preferences.Get(AppSettings.UserIDKey, 0);

            var journalTable = db.Table<Journal>();

            var dataList = await journalTable.ToListAsync();

            var result = dataList.Where
                (item => item.Date.IsInside(start, end) && item.UserID == userID);

            return result.ToList();
        }

        public async Task<User> GetUserByAppleIDAsync(string appleID)
        {
            await InvalidateConnection();

            var data = db.Table<User>();

            var user = await data.Where(x => x.AppleID == appleID).FirstOrDefaultAsync();

            if (user != null)
            {
                return user;
            }
            else
            {
                return null;
            }

        }

        //GET
        public async Task<User> GetUserAsync(string email)
        {
            await InvalidateConnection();

            var data = db.Table<User>();

            var user = await data.Where(x => x.Email == email).FirstOrDefaultAsync();

            if (user != null)
            {
                return user;
            }
            else
            {
                return null;
            }
        }

        //GET
        public async Task<User> Login(string email, string password)
        {
            await InvalidateConnection();

            var user = await GetUserAsync(email);

            if (user != null && user.Password == password)
            {
                return user;
            }

            return null;
        }

        //GET
        public async Task<bool> IsRegisteredUser(string email)
        {
            await InvalidateConnection();

            var user = await GetUserAsync(email);

            if (user != null)
            {
                return !string.IsNullOrEmpty(user.Password);
            }
            else
            {
                return false;
            }
        }

        //POST
        public async Task<bool> RegisterUserAsync(User user)
        {
            await InvalidateConnection();

            await InvalidateConnection();

            var data = db.Table<User>();

            var userInfo = await data.Where(x => x.Email == user.Email).FirstOrDefaultAsync();

            if (userInfo == null)
            {
                await db.InsertAsync(user);

                Xamarin.Forms.DependencyService.Resolve<IBackUp>().BackUp();

                return true;
            }
            else
            {
                return false;
            }
        }

        //POST
        public async Task<int> UpdateUserAsync(User user)
        {
            await InvalidateConnection();

            var result = await db.UpdateAsync(user);

            Xamarin.Forms.DependencyService.Resolve<IBackUp>().BackUp();

            return result;
        }

        //public async Task<IList<Factor>> GetSelectedFactors()
        //{
        //    var data = db.Table<Factor>();

        //    var selectedFactors = await data.Where(x => x.IsSelected).ToListAsync();

        //    if (selectedFactors != null)
        //    {
        //        return selectedFactors;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        //GET
        public async Task<List<Journal>> GetJournalsAsync(DateTime date)
        {
            await InvalidateConnection();

            var journalTable = db.Table<Journal>();

            var userID = Preferences.Get(AppSettings.UserIDKey, 0);

            try
            {
                var dataList = await journalTable.ToListAsync();

                var result = dataList.Where
                    (item => item.Date.CompareDate(date) && item.UserID == userID);

                return result.ToList();
            }
            catch
            {
                return null;
            }
        }

        //POST
        public async Task<int> UpdateJounralsAsync(List<Journal> journals)
        {
            await InvalidateConnection();

            var journalTable = db.Table<Journal>();
            var dataList = await journalTable.ToListAsync();

            foreach (var journal in journals)
            {
                var existingItem = dataList.Where(
                    item => item.Date.CompareDate(journal.Date)
                    && item.FactorID == journal.FactorID && item.UserID == journal.UserID);

                if (existingItem.Count() > 0)
                {
                    var preItem = existingItem.FirstOrDefault();
                    preItem.ActualValue = journal.ActualValue;
                    preItem.Date = journal.Date;
                    //TODO: Add few more properities for best thing.

                    await db.UpdateAsync(preItem);
                }
                else
                {
                    await db.InsertAsync(journal);
                }
            }

            Xamarin.Forms.DependencyService.Resolve<IBackUp>().BackUp();

            return 1;
        }

        //GET
        public async Task<List<Factor>> GetAllFactors(User user)
        {
            await InvalidateConnection();

            var data = db.Table<Factor>();

            var result = await data.ToListAsync();

            MergeCustomFactors(user, result);

            return result;
        }

        //GET
        public async Task<List<MicroWorkout>> GetAllMicroWorkout(User user)
        {
            await InvalidateConnection();

            var data =await db.Table<MicroWorkout>().Where(x => x.WorkoutDate == AppSettings.JournalDate.Date.Date).ToListAsync();
            if (data ==null || data!=null && data.Count < 1)
            {
                var WorkoutDurationMin = 0;
                var WorkoutDurationSec = 30;
                List<MicroWorkout> defaultFactors = new List<MicroWorkout>()
            {
                new MicroWorkout() { Name = "Squats", Description="Squats Desc", WorkoutIcon = "squats.png", YoutubeLink="https://www.youtube.com/watch?v=J8_39BIxqdk", IsSelected=true, WorkoutDurationMin=0,WorkoutDurationSec=30, WorkoutReminder=new TimeSpan(7,30,0)},
                new MicroWorkout() { Name = "Lunges", Description="Lunges Desc", WorkoutIcon = "lunges.png", YoutubeLink="", IsSelected=true,WorkoutDurationMin=0,WorkoutDurationSec=30, WorkoutReminder=new TimeSpan(4,30,0)},
                new MicroWorkout() { Name = "Push Ups", Description="Push Ups Desc", WorkoutIcon = "pushups.png", YoutubeLink="", IsSelected=true,WorkoutDurationMin=0,WorkoutDurationSec=30, WorkoutReminder=new TimeSpan(5,30,0)},
                new MicroWorkout() { Name = "Clam Shells", Description="clamshells Desc",  WorkoutIcon = "calmshells.png", YoutubeLink="",IsSelected=true,WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Straight Leg Raises", Description = "MeditationPeaceSpirituality", WorkoutIcon = "calmshells.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30,BreakDurationSec=50},
                new MicroWorkout() { Name = "Side Leg Raises", Description="Workout6 Desc", WorkoutIcon = "calmshells.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Chin Ups", Description = "SearchForKnowledgeIntellectual", WorkoutIcon = "chinups.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Half Crunch", Description="halfcrunch Desc", WorkoutIcon = "halfcrunch.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Straight Plank", Description="halfcrunch Desc", WorkoutIcon = "halfcrunch.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Half Crunch", Description="halfcrunch Desc", WorkoutIcon = "halfcrunch.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Side Plank", Description="halfcrunch Desc", WorkoutIcon = "halfcrunch.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Hydrate", Description="Hydrate Desc", WorkoutIcon = "hydrate.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Vocabulary", Description="Vocabulary Desc", WorkoutIcon = "meditation.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Play Music", Description="Play Music Desc", WorkoutIcon = "meditation.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Listen to Music", Description="Listen to Music Desc", WorkoutIcon = "meditation.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Meditation", Description="meditation Desc", WorkoutIcon = "meditation.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},


                new MicroWorkout() { Name = "Boat", Description="Boat Desc", WorkoutIcon = "Boat.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Both Toes", Description="BothToes Desc", WorkoutIcon = "BothToes.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Bound Angle", Description="BoundAngle Desc", WorkoutIcon = "BoundAngle.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Bow", Description="Bow Desc", WorkoutIcon = "Bow.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Bridge", Description="Bridge Desc", WorkoutIcon = "Bridge.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Buddhist", Description="Buddhist Desc", WorkoutIcon = "Buddhist.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Camel I", Description="CamelI Desc", WorkoutIcon = "CamelI.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Camel II", Description="CamelII Desc", WorkoutIcon = "CamelII.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Cat", Description="Cat Desc", WorkoutIcon = "Cat.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Chair", Description="Chair Desc", WorkoutIcon = "Chair.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Chair Straight Arms", Description="Chair Straight Arms Desc", WorkoutIcon = "ChairStraightArms.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Cobra", Description="Cobra Desc", WorkoutIcon = "Cobra.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "CowPose", Description="CowPose Desc", WorkoutIcon = "CowPose.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Downward Facing Dog", Description="Downward Facing Dog Desc", WorkoutIcon = "DownwardFacingDog.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Extended", Description="Extended Desc", WorkoutIcon = "Extended.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Fish", Description="Fish Desc", WorkoutIcon = "Fish.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Forward Bend Pose", Description="Forward Bend Pose Desc", WorkoutIcon = "ForwardBendPose.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Greeting", Description="Greeting Desc", WorkoutIcon = "Greeting.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Half Boat", Description="Half Boat Desc", WorkoutIcon = "HalfBoat.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "HalfMoonPose", Description="HalfMoonPose Desc", WorkoutIcon = "HalfMoonPose.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Half Moon Pose Right", Description="Half Moon Pose Right Desc", WorkoutIcon = "HalfMoonPoseRight.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Hare", Description="Hare Desc", WorkoutIcon = "Hare.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Knee Press", Description="Knee Press Desc", WorkoutIcon = "KneePress.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Lord of Dance Pose", Description="LordofDancePose Desc", WorkoutIcon = "LordofDancePose.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Low Boat", Description="LowBoat Desc", WorkoutIcon = "LowBoat.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Low Lunge Left", Description="Low Lunge Left Desc", WorkoutIcon = "LowLungeLeft.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Low Lunge Right", Description="Low Lunge Right Desc", WorkoutIcon = "LowLungeRight.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Moon Left", Description="MoonLeft Desc", WorkoutIcon = "MoonLeft.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Mountain Pose Upward", Description="Mountain Pose Upward Desc", WorkoutIcon = "MountainPoseUpward.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "One Arm Bend Left", Description="OneArmBendLeft Desc", WorkoutIcon = "OneArmBendLeft.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "One Arm Bend Right", Description="One Arm Bend Right Desc", WorkoutIcon = "OneArmBendRight.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Perfect Pose", Description="Perfect Pose Desc", WorkoutIcon = "PerfectPose.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Plank Pose", Description="Plank Pose Desc", WorkoutIcon = "PlankPose.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Plow Pose", Description="PlowPose Desc", WorkoutIcon = "PlowPose.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Supported Shoulder stand 1", Description="Supported Shoulderstand1 Desc", WorkoutIcon = "SupportedShoulderstand1.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Supported Shoulder stand", Description="Supported Shoulder stand Desc", WorkoutIcon = "SupportedShoulderstand.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Support Head stand", Description="Support Head stand Desc", WorkoutIcon = "SupportHeadstand.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Table Pose", Description="Table Pose Desc", WorkoutIcon = "TablePose.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Tree Pose Left", Description="TreePoseLeft Desc", WorkoutIcon = "TreePoseLeft.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Tree Pose Right", Description="TreePoseRight Desc", WorkoutIcon = "TreePoseRight.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Triangle Left", Description="TriangleLeft Desc", WorkoutIcon = "TriangleLeft.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Triangle Right", Description="TriangleRight Desc", WorkoutIcon = "TriangleRight.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Upward Bow Pose", Description="UpwardBowPose Desc", WorkoutIcon = "UpwardBowPose.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Upward Extended Feet", Description="UpwardExtendedFeet Desc", WorkoutIcon = "UpwardExtendedFeet.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Upward Facing Dog", Description="Upward Facing Dog Desc", WorkoutIcon = "UpwardFacingDog.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "UpwardPlankPose", Description="UpwardPlankPose Desc", WorkoutIcon = "UpwardPlankPose.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "UpwardTablePose", Description="UpwardTablePose Desc", WorkoutIcon = "UpwardTablePose.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Warrior Pose", Description="Warrior Pose Desc", WorkoutIcon = "WarriorPose.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Warrior Pose Left", Description="Warrior Pose Left Desc", WorkoutIcon = "WarriorPoseLeft.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},
                new MicroWorkout() { Name = "Warrior Pose Right", Description="Warrior Pose Right Desc", WorkoutIcon = "WarriorPoseRight.png", YoutubeLink="",WorkoutDurationMin=0,WorkoutDurationSec=30},

            };
             await db.InsertAllAsync(defaultFactors); 
            }
            var result = await db.Table<MicroWorkout>().Where(x => x.WorkoutDate == AppSettings.JournalDate.Date.Date).ToListAsync();

            return result;
        }

        private static void MergeCustomFactors(User user, List<Factor> factors)
        {
            if (user != null && user.CustomFactors != null)
            {
                var customFactors = user.CustomFactors.Split('±');
                if (customFactors == null) return;

                var customFactorStart = 1000;

                int customFactorIndex = 1;

                for (int i = 0; i < customFactors.Count(); i = i + 2)
                {
                    Factor newFactor = new Factor();
                    newFactor.IsCustomFactor = true;
                    newFactor.Name = customFactors[i];
                    newFactor.Description = customFactors[i + 1];
                    newFactor.ID = ++customFactorStart;
                    newFactor.Icon = "CF" + customFactorIndex++ + ".png";

                    factors.Add(newFactor);
                }
            }
        }

        //GET
        public async Task<Journal> GetJounrnalAsync(int id, DateTime date)
        {
            await InvalidateConnection();

            var userID = Preferences.Get(AppSettings.UserIDKey, 0);

            var data = await db.Table<Journal>().ToListAsync();

            return data.Where(item => item.Date.CompareDate(date) && item.ID == id && item.UserID == userID).FirstOrDefault();
        }

        //POST
        public async Task<int> UpdateJournalAsync(Journal journal)
        {
            await InvalidateConnection();

            var result = await db.InsertOrReplaceAsync(journal);

            Xamarin.Forms.DependencyService.Resolve<IBackUp>().BackUp();

            return result;
        }

        //GET
        public async Task<List<Journal>> GetJounrnalAsync(int id, DateTime start, DateTime end)
        {
            await InvalidateConnection();

            var userID = Preferences.Get(AppSettings.UserIDKey, 0);

            var journalTable = db.Table<Journal>();

            var dataList = await journalTable.ToListAsync();

            var result = dataList.Where
                (item => item.ID == id && item.Date.IsInside(start, end) && item.UserID == userID);

            return result.ToList();
        }

        //GET
        public async Task<OveralScore> GetOverallScoreAsync(DateTime date)
        {
            await InvalidateConnection();

            var userID = Preferences.Get(AppSettings.UserIDKey, 0);

            var data = await db.Table<OveralScore>().ToListAsync();

            return data.Where(item => item.Date.CompareDate(date) && item.UserID == userID).FirstOrDefault();
        }

        //GET
        public async Task<List<OveralScore>> GetOverallScoreAsync(DateTime start, DateTime end)
        {
            await InvalidateConnection();

            var userID = Preferences.Get(AppSettings.UserIDKey, 0);

            var journalTable = db.Table<OveralScore>();

            var dataList = await journalTable.ToListAsync();

            var result = dataList.Where
                (item => item.Date.IsInside(start, end) && item.UserID == userID);

            return result.ToList();
        }

        //POST
        public async Task<int> UpdateOverallScoreAsync(OveralScore overalScore)
        {
            var result = 0;
            try
            {

                await InvalidateConnection();

                var userID = Preferences.Get(AppSettings.UserIDKey, 0);

                var tb = db.Table<OveralScore>();
                var dataList = await tb.ToListAsync();

                var existingItem = dataList.Where(
                            item => item.Date.CompareDate(overalScore.Date) && item.UserID == userID);
                
                if (existingItem.Count() > 0)
                {
                    var preItem = existingItem.FirstOrDefault();
                    preItem.ActualValue = overalScore.ActualValue;
                    preItem.Date = overalScore.Date;
                    //TODO: Add few more properities for best thing.

                    result = await db.UpdateAsync(preItem);
                }
                else
                {
                    result = await db.InsertAsync(overalScore);
                }

                Xamarin.Forms.DependencyService.Resolve<IBackUp>().BackUp();

                return result;


            }
            catch (Exception e)
            {
                return result;
            }

        }

        //GET
        public async Task<Highlights> GetHighlightsAsync(DateTime date)
        {
            await InvalidateConnection();

            var userID = Preferences.Get(AppSettings.UserIDKey, 0);

            var data = await db.Table<Highlights>().ToListAsync();

            return data.Where(item => item.Date.CompareDate(date) && item.UserID == userID).FirstOrDefault();
        }

        //POST
        public async Task<int> UpdateHighlightsAsync(Highlights highlights)
        {
            await InvalidateConnection();

            var userID = Preferences.Get(AppSettings.UserIDKey, 0);

            var tb = db.Table<Highlights>();
            var dataList = await tb.ToListAsync();

            var existingItem = dataList.Where(
                        item => item.Date.CompareDate(highlights.Date) && item.UserID == userID);

            var result = 0;

            if (existingItem.Count() > 0)
            {
                var preItem = existingItem.FirstOrDefault();

                preItem.BestThing = highlights.BestThing;
                preItem.Challenge = highlights.Challenge;
                preItem.Lesson = highlights.Lesson;
                preItem.Date = highlights.Date;
                preItem.Photo = highlights.Photo;
                //TODO: Add few more properities for best thing.

                result = await db.UpdateAsync(preItem);
            }
            else
            {
                result = await db.InsertAsync(highlights);
            }

            Xamarin.Forms.DependencyService.Resolve<IBackUp>().BackUp();

            return result;
        }

        //GET
        public async Task<List<Highlights>> GetHighlightsAsync()
        {
            await InvalidateConnection();

            var userID = Preferences.Get(AppSettings.UserIDKey, 1);

            var journalTable = db.Table<Highlights>();

            var data = await journalTable.ToListAsync();

            return data.Where(item => item.UserID == userID).ToList();
        }

        //POST
        public async Task<int> UpdateUserDataAsync(User user)
        {
            await InvalidateConnection();

            var result = await db.UpdateAsync(user);

            Xamarin.Forms.DependencyService.Resolve<IBackUp>().BackUp();

            return result;
        }

        public async Task<PersonalNote> GetPersonalNoteAsync(DateTime date)
        {
            await InvalidateConnection();

            var userID = Preferences.Get(AppSettings.UserIDKey, 0);

            var personalNoteTable = db.Table<PersonalNote>();
            var data = await personalNoteTable.ToListAsync();

            return data.Where(item => item.UserID == userID && date.CompareDate(item.Date)).FirstOrDefault();
        }

        //POST
        public async Task<int> SetPersonalNoteAsync(DateTime date, string note)
        {
            await InvalidateConnection();

            var userID = Preferences.Get(AppSettings.UserIDKey, 0);

            var tb = db.Table<PersonalNote>();
            var dataList = await tb.ToListAsync();

            var existingItem = dataList.Where(item => item.UserID == userID && date.CompareDate(item.Date));

            var result = 0;

            if (existingItem.Count() > 0)
            {
                var preItem = existingItem.FirstOrDefault();

                preItem.Note = note;

                result = await db.UpdateAsync(preItem);
            }
            else
            {
                result = await db.InsertAsync(new PersonalNote { Date = date, Note = note, UserID = userID });
            }

            Xamarin.Forms.DependencyService.Resolve<IBackUp>().BackUp();

            return result;
        }

        //GET
        public async Task<List<NegativeFactor>> GetNegativeFactorAsync(DateTime date)
        {
            await InvalidateConnection();

            var userID = Preferences.Get(AppSettings.UserIDKey, 0);

            var table = db.Table<NegativeFactor>();

            var data = await table.ToListAsync();

            var result = data.Where(item => item.UserID == userID && item.Date.CompareDate(date)).ToList();

            result.ForEach(item => item.initCompleted = true);

            return result;
        }

        //GET
        public async Task<List<NegativeFactor>> GetNegativeFactorAsync(DateTime start, DateTime end)
        {
            await InvalidateConnection();

            var userID = Preferences.Get(AppSettings.UserIDKey, 0);

            var table = db.Table<NegativeFactor>();

            var data = await table.ToListAsync();

            var result = data.Where(item => item.UserID == userID && item.Date.IsInside(start, end)).ToList();

            result.ForEach(item => item.initCompleted = true);

            return result;
        }


        //POST
        public async Task<int> SetNegativeFactorAsync(List<NegativeFactor> negativeFactors)
        {
            await InvalidateConnection();

            var date = AppSettings.JournalDate;
            foreach (var negativeJournal in negativeFactors)
            {
                if (negativeJournal.IsValid())
                {
                    negativeJournal.Date = date;

                    await db.InsertAsync(negativeJournal);
                }
            }

            Xamarin.Forms.DependencyService.Resolve<IBackUp>().BackUp();

            return 1;
        }

        //POST
        public async Task<int> DeleteJournalAsync(Journal journal)
        {
            await InvalidateConnection();

            var result = await db.DeleteAsync(journal);

            Xamarin.Forms.DependencyService.Resolve<IBackUp>().BackUp();

            return result;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            await InvalidateConnection();

            return await db.Table<User>().ToListAsync();
        }

        public async Task<int> AddWishlistAsync(List<Wishlist> wishlist)
        {
            await InvalidateConnection();

            var table = db.Table<Wishlist>();
            var dataList = await table.ToListAsync();

            foreach (var item in wishlist)
            {
                var existingItem = dataList.Where(e => e.ID == item.ID && e.AssociatedID == item.AssociatedID && e.Name == item.Name);

                if (existingItem.Count() > 0)
                {
                    await db.UpdateAsync(item);
                }
                else
                {
                    await db.InsertAsync(item);
                }
            }

            return 1;
        }

        public async Task<List<Wishlist>> GetWishlistAsync(int factorID)
        {
            await InvalidateConnection();

            var table = db.Table<Wishlist>();
            var dataList = await table.ToListAsync();

            return dataList.Where(item => item.AssociatedID == factorID).ToList();
        }


        //public async Task<int> AddWishlistMicroAsync(List<WishlistMicro> wishlist)
        //{
        //    await InvalidateConnection();

        //    var table = db.Table<WishlistMicro>();
        //    var dataList = await table.ToListAsync();

        //    foreach (var item in wishlist)
        //    {
        //        var existingItem = dataList.Where(e => e.ID == item.ID && e.AssociatedID == item.AssociatedID && e.Name == item.Name);

        //        if (existingItem.Count() > 0)
        //        {
        //            await db.UpdateAsync(item);
        //        }
        //        else
        //        {
        //            await db.InsertAsync(item);
        //        }
        //    }

        //    return 1;
        //}

        //public async Task<List<WishlistMicro>> GetWishlistMicroAsync(int factorID)
        //{
        //    await InvalidateConnection();

        //    var table = db.Table<WishlistMicro>();
        //    var dataList = await table.ToListAsync();

        //    return dataList.Where(item => item.AssociatedID == factorID).ToList();
        //}

        public async Task<bool> ProvidersSave(Providers Providers, provider_specialities provider_Specialities, providerLanguage language)
        {
            await InvalidateConnection();

            var table = db.Table<Providers>();
            var dataList = await table.ToListAsync();


            var existingItem = dataList.Where(e => e.id == Providers.id);

            if (existingItem.Count() > 0)
            {
                await db.UpdateAsync(Providers);
                await db.UpdateAsync(language);
                await db.UpdateAsync(provider_Specialities);
            }
            else
            {
                await db.InsertAsync(Providers);
                await db.InsertAsync(language);
                await db.InsertAsync(provider_Specialities);
            }
            return true;
        }

        public async Task<List<Providers>> GetAllProvidersByTypeAsync(string provider_type)
        {
            await InvalidateConnection();

            var data = await db.Table<Providers>().Where(s => s.provider_type == provider_type).ToListAsync();

            return data.ToList();
        }

        public async Task<Providers> GetProviderAsync(string id)
        {
            await InvalidateConnection();

            var data = await db.Table<Providers>().FirstOrDefaultAsync(s => s.id == id);

            return data;
        }

        public async Task<provider_specialities> Getprovider_specialitiesAsync(string id)
        {
            await InvalidateConnection();
            var data = await db.Table<provider_specialities>().FirstOrDefaultAsync(s => s.ProviderId == id);
            return data;
        }

        public async Task<providerLanguage> Getprovider_languageAsync(string id)
        {
            await InvalidateConnection();
            var data = await db.Table<providerLanguage>().FirstOrDefaultAsync(s => s.ProviderId == id);
            return data;
        }

        public async Task<int> UpdateAllMicroWoroutAsync(List<MicroWorkout> microWorkouts)
        {
            return await db.UpdateAllAsync(microWorkouts);
        }


        public async Task<int> InsertAllMicroWoroutAsync(List<MicroWorkout> microWorkouts)
        {
            return await db.InsertAllAsync(microWorkouts);
        }

        public async Task<int> AddMicroWoroutAsync(MicroWorkout microWorkout)
        {
           return await db.InsertAsync(microWorkout);
        }

        public async Task<int> UpdateMicroWoroutAsync(MicroWorkout microWorkout)
        {
            return await db.UpdateAsync(microWorkout);
        }

        public async Task<DateTime> GetLastWorkoutDate()
        {
          var lastWorkout =await db.Table<MicroWorkout>().OrderByDescending(x => x.WorkoutDate).FirstOrDefaultAsync();
            return lastWorkout.WorkoutDate;
        }


        public async void deleteAllUsersAsync()
        {
            await InvalidateConnection();

            await db.DeleteAllAsync<User>();
           // await db.DropTableAsync<User>();

            // Table<User>().ToListAsync();
        }
    }
}