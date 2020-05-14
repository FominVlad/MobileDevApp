using System;
using System.IO;
using Xamarin.Forms;

namespace MobileDevApp.Helpers
{
    public class ByteArrayToImageSourceConverter : IValueConverter
    {
        public object Convert(object value,
               Type targetType,
               object parameter,
               System.Globalization.CultureInfo culture)
        {
            ImageSource retSource = null;
            if (value != null)
            {
                byte[] imageAsBytes = (byte[])value;
                retSource = ImageSource.FromStream(() => new MemoryStream(imageAsBytes));
            }
            return retSource;
        }

        public object ConvertBack(object value,
               Type targetType,
               object parameter,
               System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
