namespace AutomatedCar.Helpers
{
    public class RotationPoint      //forrás: Assets/ reference_points.json //Minden png pontosan egyszer található meg ott.
    {
        public string Type { get; set; }        //png fájl neve. Megadja hogy a png kép bal felsõ sarkától számítva hol legyen a forgáspont. Ezeket töltjük X-be és Y-ba.

        public int X { get; set; }

        public int Y { get; set; }
    }
}
