using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace HappinessIndex.Converters
{
    public class strToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;
            if (string.IsNullOrEmpty(value.ToString()))
                return false;
            else
                return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return true;
            if (string.IsNullOrEmpty(value.ToString()))
                return false;
            else
                return true;
        }
    }
}
