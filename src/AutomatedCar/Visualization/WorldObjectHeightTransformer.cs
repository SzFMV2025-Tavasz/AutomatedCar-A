namespace AutomatedCar.Visualization
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Reflection;
    using Avalonia.Data.Converters;
    using Avalonia.Media.Imaging;

    public class WorldObjectHeightTransformer : IValueConverter
    {
        private static Dictionary<string, double> cache = new Dictionary<string, double>();

        public static WorldObjectHeightTransformer Instance { get; } = new WorldObjectHeightTransformer();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            GetCachedValue((string)value);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        private static double GetCachedValue(string filename)
        {
            if (!cache.ContainsKey(filename))
            {
                var img = new Bitmap(Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream($"AutomatedCar.Assets.WorldObjects.{filename}"));
                cache.Add(filename, img.Size.Height);
            }

            return cache[filename];
        }
    }
}