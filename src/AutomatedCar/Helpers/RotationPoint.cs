namespace AutomatedCar.Helpers
{
    public class RotationPoint      //forr�s: Assets/ reference_points.json //Minden png pontosan egyszer tal�lhat� meg ott.
    {
        public string Type { get; set; }        //png f�jl neve. Megadja hogy a png k�p bal fels� sark�t�l sz�m�tva hol legyen a forg�spont. Ezeket t�ltj�k X-be �s Y-ba.

        public int X { get; set; }

        public int Y { get; set; }
    }
}
