using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HappinessIndex.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyJournalTabbedPage : TabbedPage
    {
        public MyJournalTabbedPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            var dayName = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedDayName(AppSettings.JournalDate.DayOfWeek);

            Title = $"{dayName}, {AppSettings.JournalDate:MMMM dd yyyy}".ToUpper();

            base.OnAppearing();
        }
    }
}
