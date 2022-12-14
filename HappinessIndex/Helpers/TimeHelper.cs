using System;
using HappinessIndex.Resx;

namespace HappinessIndex.Helpers
{
    public static class TimeHelper
    {
        public static string TimeToGreeting(int hour)
        {
            if (hour >= 5 && hour < 12)
            {
                return AppResources.GoodMorning;
            }
            if (hour >= 12 && hour < 16)
            {
                return AppResources.GoodAfternoon;
            }
            return AppResources.GoodEvening;
        }
    }
}