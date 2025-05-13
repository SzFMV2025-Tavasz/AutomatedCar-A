namespace AutomatedCar.Helpers
{
    public class SpeedHelper
    {
        // Calculated by comparing the average road widh in Hungary to the road_2lane_straight.png road width ~= 49.69
        // Calculated by comparing the Skoda Octavia length to the car_1_white.png height ~= 51.09
        public const double MeterToPixels = 50;

        private double PixelsPerTick { get; set; }

        private SpeedHelper(double pixelsPerTick)
        {
            this.PixelsPerTick = pixelsPerTick;
        }

        public static SpeedHelper FromKmPerHour(double kmPerHour)
        {
            return FromMetersPerSecond(kmPerHour / 3.6);
        }

        public static SpeedHelper FromMetersPerSecond(double metersPerSecond)
        {
            return FromPixelsPerTick(metersPerSecond * (MeterToPixels / GameBase.TicksPerSecond));
        }

        public static SpeedHelper FromPixelsPerTick(double pixelsPerTick)
        {
            return new SpeedHelper(pixelsPerTick);
        }

        public static SpeedHelper FromPixelsPerSecond(double pixelsPerSecond)
        {
            return FromPixelsPerTick(pixelsPerSecond / GameBase.TicksPerSecond);
        }

        public double InKmPerHour()
        {
            return this.InMetersPerSecond() * 3.6;
        }

        public double InMetersPerSecond()
        {
            return this.PixelsPerTick * (GameBase.TicksPerSecond / MeterToPixels);
        }

        public double InPixelsPerTick()
        {
            return this.PixelsPerTick;
        }

        public double InPixelsPerSecond()
        {
            return this.PixelsPerTick * GameBase.TicksPerSecond;
        }
    }
}
