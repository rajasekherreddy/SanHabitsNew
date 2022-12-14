using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;

namespace HappinessIndex.Views
{
    public partial class MicroWorkout : ContentPage
    {
        public MicroWorkout()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            var dayName = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedDayName(AppSettings.JournalDate.DayOfWeek);

           // Title = $"{dayName}, {AppSettings.JournalDate:MMMM dd yyyy}".ToUpper();

            base.OnAppearing();
        }
    }
}

