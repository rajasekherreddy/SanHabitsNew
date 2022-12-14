using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;

namespace HappinessIndex.Views
{	
	public partial class MircoWorkoutList : ContentPage
	{	
		public MircoWorkoutList()
		{
			InitializeComponent ();
		}

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            
        }

        void selector_SelectionChanged(System.Object sender, Syncfusion.XForms.Buttons.SelectionChangedEventArgs e)
        {
           if( e.Index == 1)
            {
                MicroWorkoutView.IsVisible = false;
                ExistingViewGrid.IsVisible = true;

            }
            else
            {
                MicroWorkoutView.IsVisible = true;
                ExistingViewGrid.IsVisible = false;
            }
        }

        protected override void OnAppearing()
        {
            var dayName = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedDayName(AppSettings.JournalDate.DayOfWeek);

            Title = $"{dayName}, {AppSettings.JournalDate:MMMM dd yyyy}".ToUpper();

            base.OnAppearing();
        }

    }

    public class BoolToStringConverterMicroWorkout : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (bool.Parse(value.ToString()))
            {
                return "completed";
            }
            else
            {
                return "playButton";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}

