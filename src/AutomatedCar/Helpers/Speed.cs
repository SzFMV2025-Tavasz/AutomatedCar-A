namespace AutomatedCar.Helpers
{
    public class Speed
    {
        // Calculated by comparing the average road widh in Hungary to the road_2lane_straight.png road width ~= 49.69
        // Calculated by comparing the Skoda Octavia length to the car_1_white.png height ~= 51.09
        public const double MeterToPixels = 50;

        private double PixelsPerTick { get; set; }

        private Speed(double pixelsPerTick)
        {
            this.PixelsPerTick = pixelsPerTick;
        }

        public static Speed FromKmPerHour(double kmPerHour)
        {
            return FromMetersPerSecond(kmPerHour / 3.6);
        }

        public static Speed FromMetersPerSecond(double metersPerSecond)
        {
            double pixelsPerSecond = metersPerSecond * MeterToPixels;
            double pixelsPerTick = pixelsPerSecond / GameBase.TicksPerSecond;
            return FromPixelsPerTick(pixelsPerTick);
        }

        public static Speed FromPixelsPerTick(double pixelsPerTick)
        {
            return new Speed(pixelsPerTick);
        }

        public double InKmPerHour()
        {
            // TODO Implement unit conversion
            return PixelsPerTick;
        }

        public double InMetersPerSecond()
        {
            return this.PixelsPerTick * (GameBase.TicksPerSecond / MeterToPixels);
        }

        public double InPixelsPerTick()
        {
            return this.PixelsPerTick;
        }
    }
}
