namespace AutomatedCar.Helpers
{
    public class RawWorldObject     //forrás:Assets/ test_world.json
    {
        public string Type { get; set; }        //png fájl neve

        public int X { get; set; }              //(x,y) = hely

        public int Y { get; set; }

        public float M11 { get; set; }          //origó körül forgató 2x2-es valós mátrix. (Kiszámolható belõle a forgatás szöge. Ezt meg is fogjuk tenni a World osztály PopulateFromJson() függvényében.)

        public float M12 { get; set; }

        public float M21 { get; set; }

        public float M22 { get; set; }
    }
}
