using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace FlashCards.Converters
{
    public class EnumToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (targetType != typeof(Visibility))
            {
                throw new InvalidOperationException("The target must be a Visibility");
            }

            if (value == null)
            {
                return null;
            }

            if (parameter != null)
            {
                try
                {
                    if (value == Enum.Parse(value.GetType(), parameter.ToString()))
                    {
                        return Visibility.Visible;
                    }
                    else
                    {
                        return Visibility.Collapsed;
                    }
                }
                catch
                {
                    return Visibility.Collapsed;
                }
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}