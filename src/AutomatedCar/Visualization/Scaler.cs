namespace AutomatedCar.Visualization
{
    using System;
    using System.Globalization;
    using Avalonia.Data.Converters;

    public class Scaler : IValueConverter
    {
        public static Scaler Instance { get; } = new Scaler();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value * 1; // keep it the same size
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}