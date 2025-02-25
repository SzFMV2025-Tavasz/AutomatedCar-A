namespace AutomatedCar.Helpers
{
    using System.Collections.Generic;

    public class RawWorld
    {
        public int Width { get; set; }

        public int Height { get; set; }

        public List<RawWorldObject> Objects { get; set; }
    }
}
