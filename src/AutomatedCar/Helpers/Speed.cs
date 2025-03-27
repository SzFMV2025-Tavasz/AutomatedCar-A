namespace AutomatedCar.Helpers
{
    internal class Speed
    {
        private double PixelsPerTick { get; set; }

        private Speed(double pixelsPerTick)
        {
            this.PixelsPerTick = pixelsPerTick;
        }

        public static Speed FromKmPerHour(double kmPerHour)
        {
            return new Speed(kmPerHour);
        }

        public static Speed FromMetersPerSecond(double metersPerSecond)
        {
            return new Speed(metersPerSecond);
        }

        public static Speed FromPixelsPerTick(double pixelsPerTick)
        {
            return new Speed(pixelsPerTick);
        }

        public double InKmPerHour()
        {
            return PixelsPerTick;
        }

        public double InMetersPerSecond()
        {
            return PixelsPerTick;
        }

        public double InPixelsPerTick()
        {
            return PixelsPerTick;
        }
    }
}
