namespace AutomatedCar.Helpers
{
    using System.Collections.Generic;

    public class RawPolygon         
    {
        public string Polytype { get; set; }        //megadja a poligon típusát. az összes elõforduló típus a worldobject_poligons.json fájlban:  standalone, lane, curve, circle


        public List<List<int>> Points { get; set; }

        //gyakorlatban List<(int, int)> -ként, azaz 2D pontok listájaként használjuk. 
        //(Az elsõ int a pont x koordinátáját, a második int pedig a pont y koordinátáját reprezentálja.)
        //Késõbb, a World osztályban úgynevezett PolylineGeometry feltöltésére használjuk.

        //Röviden a PolylineGeometry-rõl:
        //Ez egy beépített osztály, amely pontok sorozatát definiálja. A pontok a felsorolás sorrendjében kerülnek összekötésre.
        //Alapvetõen nem zárt alakzatot hoznak létre, kivéve ha zártként definiáljuk. 
        //A pontokat a PolylineGeometry.Points tulajdonságában tároljuk, melynek típusa Avalonia.Points. Az Avalonia.Points pedig Avaloni.Point-okból áll.
        //Egy Avalonia.Point egy (double,double) párost tárol.
        //További tulajdonságok pl: PolylineGeometry.Closed, PolylineGeometry.IsFilled.
        //Ezek alapértelemzetten false értéket kapnak.
    }
}
