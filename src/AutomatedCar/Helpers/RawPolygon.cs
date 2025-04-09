namespace AutomatedCar.Helpers
{
    using System.Collections.Generic;

    public class RawPolygon         
    {
        public string Polytype { get; set; }        //megadja a poligon t�pus�t. az �sszes el�fordul� t�pus a worldobject_poligons.json f�jlban:  standalone, lane, curve, circle


        public List<List<int>> Points { get; set; }

        //gyakorlatban List<(int, int)> -k�nt, azaz 2D pontok list�jak�nt haszn�ljuk. 
        //(Az els� int a pont x koordin�t�j�t, a m�sodik int pedig a pont y koordin�t�j�t reprezent�lja.)
        //K�s�bb, a World oszt�lyban �gynevezett PolylineGeometry felt�lt�s�re haszn�ljuk.

        //R�viden a PolylineGeometry-r�l:
        //Ez egy be�p�tett oszt�ly, amely pontok sorozat�t defini�lja. A pontok a felsorol�s sorrendj�ben ker�lnek �sszek�t�sre.
        //Alapvet�en nem z�rt alakzatot hoznak l�tre, kiv�ve ha z�rtk�nt defini�ljuk. 
        //A pontokat a PolylineGeometry.Points tulajdons�g�ban t�roljuk, melynek t�pusa Avalonia.Points. Az Avalonia.Points pedig Avaloni.Point-okb�l �ll.
        //Egy Avalonia.Point egy (double,double) p�rost t�rol.
        //Tov�bbi tulajdons�gok pl: PolylineGeometry.Closed, PolylineGeometry.IsFilled.
        //Ezek alap�rtelemzetten false �rt�ket kapnak.
    }
}
