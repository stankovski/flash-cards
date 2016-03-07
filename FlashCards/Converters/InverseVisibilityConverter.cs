using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace FlashCards.Converters
{
    public class InverseVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (targetType != typeof(Visibility))
            {
                throw new InvalidOperationException("The target must be a Visibility");
            }

            if (value == null || !(value is Visibility))
            {
                return null;
            }

            var valueAsVisibility = (Visibility)value;
            return valueAsVisibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (targetType != typeof(Visibility))
            {
                throw new InvalidOperationException("The target must be a Visibility");
            }

            if (value == null || !(value is Visibility))
            {
                return null;
            }

            var valueAsVisibility = (Visibility)value;
            return valueAsVisibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}