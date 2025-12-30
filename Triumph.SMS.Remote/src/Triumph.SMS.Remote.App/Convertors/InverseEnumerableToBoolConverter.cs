using System.Collections;
using System.Globalization;

namespace Triumph.SMS.Remote.App.Convertors;

public class InverseEnumerableToBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is IEnumerable enumerable)
        {
            foreach (var _ in enumerable)
            {
                return false; // there is at least one error -> form should be hidden
            }
            return true; // no items -> form visible
        }

        if (value is string s)
        {
            return string.IsNullOrWhiteSpace(s); // empty string -> visible
        }

        return value == null; // null -> visible
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
