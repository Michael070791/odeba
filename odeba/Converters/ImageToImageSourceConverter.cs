using System;
using System.Globalization;
using System.IO;
using Xamarin.Forms;

namespace odeba.Converters
{
    public class ImageToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string base64 && base64.StartsWith("data:image"))
            {
                var fileArray = base64.Substring(base64.IndexOf(',') + 1);
                var bytes = System.Convert.FromBase64String(fileArray);
                return ImageSource.FromStream(() => new MemoryStream(bytes));
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

