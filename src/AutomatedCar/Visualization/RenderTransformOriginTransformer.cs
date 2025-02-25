namespace AutomatedCar.Visualization
{
    using System;
    using System.Drawing;
    using System.Globalization;
    using Avalonia.Data.Converters;

    public class RenderTransformOriginTransformer : IValueConverter
    {
        public static RenderTransformOriginTransformer Instance { get; } = new RenderTransformOriginTransformer();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // with the absolute unit, the relative point defines the rotation center as coordinated within the image coordinate system
            // This is Avalonia specific, WPF needs a ratio between 0 and 1 (rotationPoint.X/image.Width, rotationPoint.Y/image.Height)
            // TODO apply caching
            var relativePoint = new Avalonia.RelativePoint(new Avalonia.Point(((Point)value).X, ((Point)value).Y), Avalonia.RelativeUnit.Absolute);

            return relativePoint;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}