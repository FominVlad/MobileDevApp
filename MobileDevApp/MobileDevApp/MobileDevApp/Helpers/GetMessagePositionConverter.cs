using System;
using System.Globalization;
using Xamarin.Forms;

namespace MobileDevApp.Helpers
{
    public class GetMessagePositionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Int32.TryParse(value.ToString(), out int userID))
            {
                return userID == App.UserInfo.UserID ? LayoutOptions.End : LayoutOptions.Start;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
