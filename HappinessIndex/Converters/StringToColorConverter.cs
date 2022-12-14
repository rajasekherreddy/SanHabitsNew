using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace HappinessIndex.Converters
{
    class StringToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string valueAsString = value.ToString();
            switch (valueAsString)
            {
                case ("White"):
                    {
                        return Color.White;
                    }
                case ("Green"):
                    {
                        return Color.Green;
                    }
                default:
                    {
                        return Color.Black;
                    }
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
