namespace AutomatedCar.Helpers
{
    using System.Collections.Generic;

    public class RawWorld   //forr�s:Assets/ test_world.json
    {
        public int Width { get; set; }

        public int Height { get; set; }

        public List<RawWorldObject> Objects { get; set; }       //RawWorldObject-ek list�ja
    }
}
