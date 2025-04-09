namespace AutomatedCar.Helpers
{
    using System.Collections.Generic;

    public class RawWorldObjectPolygon     //forrás: Assets/ worldobject_poligons.json.  //Minden png pontosan egyszer található meg ott.
    {
        public string Type { get; set; }   //az objektumhoz tartozó png fájl neve. A png-k az Assets mappában találhatóak.

        public List<RawPolygon> Polys { get; set; }     //RawPolygon-ok listája. Megadja, milyen poligonokból álljon össze az objektum belsõ reprezentációja. 
                                                        //(A külsõ reprezentációt maga a png fájl adja.)
    }
}
