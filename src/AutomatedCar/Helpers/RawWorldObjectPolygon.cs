namespace AutomatedCar.Helpers
{
    using System.Collections.Generic;

    public class RawWorldObjectPolygon
    {
        public string Type { get; set; }

        public List<RawPolygon> Polys { get; set; }
    }
}
