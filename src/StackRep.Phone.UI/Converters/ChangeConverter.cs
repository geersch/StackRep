using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace StackRep.Phone.UI.Converters
{
    public class ChangeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var change = (int)value;

            if (change < 0)
                return new SolidColorBrush(Colors.Red);


            return new SolidColorBrush(new Color {A = 255, R = 0, G = 255, B = 0});
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}