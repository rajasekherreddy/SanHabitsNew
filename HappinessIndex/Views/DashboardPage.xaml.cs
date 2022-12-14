using System;
using System.Linq;
using System.Globalization;
using System.IO;
using HappinessIndex.Helpers;
using HappinessIndex.Resx;
using HappinessIndex.ViewModels;
using Xamarin.Forms;
using HappinessIndex.Models;
using System.Collections.Generic;
using Syncfusion.SfCarousel.XForms;
using FFImageLoading.Forms;

namespace HappinessIndex.Views
{
    public partial class DashboardPage : ContentPage
    {
        public DashboardPage()
        {
            InitializeComponent();
            //DateLabel.Text = AppResources.ReadyMessage + DateTime.Now.ToString("MMM-dd");
            if (Device.RuntimePlatform == "iOS")
            {
                //List.EnableVirtualization = true;
            }
        }

        protected override void OnAppearing()
        {
            //AppSettings.JournalDate = DateTime.Now;
            base.OnAppearing();

            //List.SetBinding(SfCarousel.ItemsSourceProperty, "Highlights");
        }

        void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            if (BindingContext is DashboardPageViewModel viewModel && viewModel.Highlights != null)
            {
                var selectedDate = e.NewDate.Date;

                var item = viewModel.Highlights.Where(item => item.Date.Date == selectedDate).FirstOrDefault();
                if (item == null) return;

                int index = viewModel.Highlights.IndexOf(item);
                //List.SelectedItem = item;
                List.ScrollTo(index);
            }
        }

        void List_SelectionChanged(object sender, Syncfusion.SfCarousel.XForms.SelectionChangedEventArgs e)
        {
            var highlight = e.SelectedItem as Highlights;

            if (BindingContext is DashboardPageViewModel viewModel && viewModel != null && highlight != null)
            {
                viewModel.SelectedDate = highlight.Date.Date;
            }
        }
    }

    public class GreetingUserWithTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return TimeHelper.TimeToGreeting(DateTime.Now.Hour) + value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (bool.Parse(value.ToString()))
            {
                return AppResources.Edit;
            }
            else
            {
                return AppResources.View;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ImageToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}