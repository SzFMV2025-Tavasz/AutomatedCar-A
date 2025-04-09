namespace AutomatedCar.Helpers
{
    using System.Collections.Generic;

    public class RawWorldObjectPolygon     //forr�s: Assets/ worldobject_poligons.json.  //Minden png pontosan egyszer tal�lhat� meg ott.
    {
        public string Type { get; set; }   //az objektumhoz tartoz� png f�jl neve. A png-k az Assets mapp�ban tal�lhat�ak.

        public List<RawPolygon> Polys { get; set; }     //RawPolygon-ok list�ja. Megadja, milyen poligonokb�l �lljon �ssze az objektum bels� reprezent�ci�ja. 
                                                        //(A k�ls� reprezent�ci�t maga a png f�jl adja.)
    }
}
