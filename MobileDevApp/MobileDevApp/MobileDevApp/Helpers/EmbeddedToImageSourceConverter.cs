using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace MobileDevApp.Helpers
{
    public class EmbeddedToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string fileName && parameter is String assemblyName)
            {
                try
                {
                    var imageSource = ImageSource.FromResource(assemblyName + "." + fileName, typeof(EmbeddedToImageSourceConverter).GetTypeInfo().Assembly);
                    return imageSource;
                }
                catch (Exception)
                {
                    return value;
                }
            }
            else
                return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
