namespace AutomatedCar.Visualization
{
    using System;
    using System.Globalization;
    using Avalonia.Data.Converters;

    public class Inverter : IValueConverter
    {
        public static Inverter Instance { get; } = new Inverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double)
            {
                return (double)value * -1;
            }
            else
            {
                return (int)value * -1;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}