namespace AutomatedCar.Visualization
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Reflection;
    using Avalonia.Data.Converters;
    using Avalonia.Media.Imaging;

    public class WorldObjectTransformer : IValueConverter
    {
        private static Dictionary<string, Bitmap> cache = new Dictionary<string, Bitmap>();

        public static WorldObjectTransformer Instance { get; } = new WorldObjectTransformer();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            GetCachedImage((string)value);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        private static Bitmap GetCachedImage(string filename)
        {
            if (!cache.ContainsKey(filename))
            {
                cache.Add(
                    filename,
                    new Bitmap(Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream($"AutomatedCar.Assets.WorldObjects.{filename}")));
            }

            return cache[filename];
        }
    }
}