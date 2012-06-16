using System;
using System.Windows.Data;

namespace StackRep.Phone.UI.Converters
{
    public class DaysConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var days = (int) value;
            return String.Format("{0} days", days);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var selectedItem = value.ToString();
            var days = Int32.Parse(selectedItem.Substring(0, selectedItem.IndexOf(" ") - 1));
            return days;
        }
    }
}