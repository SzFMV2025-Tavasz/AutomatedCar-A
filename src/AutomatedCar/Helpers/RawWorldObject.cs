namespace AutomatedCar.Helpers
{
    public class RawWorldObject     //forr�s:Assets/ test_world.json
    {
        public string Type { get; set; }        //png f�jl neve

        public int X { get; set; }              //(x,y) = hely

        public int Y { get; set; }

        public float M11 { get; set; }          //orig� k�r�l forgat� 2x2-es val�s m�trix. (Kisz�molhat� bel�le a forgat�s sz�ge. Ezt meg is fogjuk tenni a World oszt�ly PopulateFromJson() f�ggv�ny�ben.)

        public float M12 { get; set; }

        public float M21 { get; set; }

        public float M22 { get; set; }
    }
}
