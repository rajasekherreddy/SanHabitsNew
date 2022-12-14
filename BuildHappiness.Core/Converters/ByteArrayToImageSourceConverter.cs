using System;
using System.Globalization;
using System.IO;
using Xamarin.Forms;

namespace BuildHappiness.Core.Converters
{
    public class ByteArrayToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                byte[] imageAsBytes = (byte[])value;
                return ImageSource.FromStream(() => new MemoryStream(imageAsBytes));
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
