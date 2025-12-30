using System.Collections;
using System.Globalization;

namespace Triumph.SMS.Remote.App.Convertors;

public class EnumerableToBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is IEnumerable enumerable)
        {
            foreach (var _ in enumerable)
            {
                return true; // has at least one item
            }
            return false;
        }

        // support single-string error scenario
        if (value is string s)
        {
            return !string.IsNullOrWhiteSpace(s);
        }

        return value != null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}