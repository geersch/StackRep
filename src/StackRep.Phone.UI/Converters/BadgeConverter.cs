using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace StackRep.Phone.UI.Converters
{
    public class BadgeConverter : IValueConverter
    {
        private readonly BitmapImage _goldBadge;
        private readonly BitmapImage _silverBadge;
        private readonly BitmapImage _bronzeBadge;

        public BadgeConverter()
        {
            var uri = new Uri("/StackRep.Phone.UI;component/Images/gold-badge.png", UriKind.Relative);
            _goldBadge = new BitmapImage(uri);

            uri = new Uri("/StackRep.Phone.UI;component/Images/silver-badge.png", UriKind.Relative);
            _silverBadge = new BitmapImage(uri);

            uri = new Uri("/StackRep.Phone.UI;component/Images/bronze-badge.png", UriKind.Relative);
            _bronzeBadge = new BitmapImage(uri);
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var rank = (string)value;

            switch (rank)
            {
                case "gold":
                    return _goldBadge;

                case "silver":
                    return _silverBadge;

                default:
                case "bronze":
                    return _bronzeBadge;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}