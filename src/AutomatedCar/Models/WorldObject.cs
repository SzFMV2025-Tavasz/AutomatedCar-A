namespace AutomatedCar.Models
{
    using Avalonia.Media;
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    public class PropertyChangedEventArgs : EventArgs
    {
        public PropertyChangedEventArgs(string propertyName)
        {
            PropertyName = propertyName;
        }

        public string PropertyName { get; set; }
    }

    public class WorldObject
    {
        //priv�t adattagok:
        private int x;
        private int y;

        private double rotation;        //0 fok = KELET. �ramutat� j�r�s�val megegyez� ir�nyban n�vekszik.


        //Publikus adattagok: -> Amikr�l �rtes�t�st k�ld�nk:

        public double Rotation
        {
            get => this.rotation;
            set
            {
                this.rotation = value % 360;
                this.PropertyChangedEvent?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Rotation)));
            }
        }

        public int X
        {
            get => this.x;
            set
            {
                this.x = value;
                this.PropertyChangedEvent?.Invoke(this, new PropertyChangedEventArgs(nameof(this.X)));
            }
        }

        public int Y
        {
            get => this.y;
            set
            {
                this.y = value;
                this.PropertyChangedEvent?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Y)));
            }
        }

        //Publikus adattagok: -> Amikr�l NEM k�ld�nk �rtes�t�st:

        public event EventHandler<PropertyChangedEventArgs> PropertyChangedEvent;
        public int ZIndex { get; set; }                     //objektum l�that�s�gi r�tege a megjelen�t�s sor�n. Minn�l magasabb ann�l feljebb l�tsz�dik.
                                                            //Ha azt akarjuk, hogy ne takarja ki semmi, legyen minden m�s objektum ZIndex-�n�l nagyobb.
        public Point RotationPoint { get; set; }            //megadja a forgat�s k�z�ppontj�t. Az objektumhoz tartoz� (Filename nev�) png k�p bal fels� sark�t�l sz�m�tjuk.
                                                            //forr�s: Assets/ reference_points.json -ben a Filename nev� png-t kell kikeresni. Minden png pontosan egyszer tal�lhat� meg ott.

        public string RenderTransformOrigin { get; set; }       //szint�n a forgat�s k�z�ppontj�t adja meg, csak egy ar�nnyal kifejezve.
                                                                //Ezzel lehet�v� tessz�k, hogy a k�pet �tm�retezve a forgat�si k�z�ppont ar�nyosan ugyanott maradjon.
                                                                //pl. ("30%, 50%") = a k�p sz�less�g�nek 30%-�n�l �s a k�p magass�g�nak 50%-�n�l legyen a forgat�si k�z�ppont.
                                                                //Hab�r ezt v�g�l nem fogjuk kihaszn�lni, mert nem fogunk k�pet nagy�tani, se kicsiny�teni.
                                                                //Mi�rt van sz�ks�g erre a tulajdons�gra, ha nem fogunk nagy�tani, �s m�r t�roljuk am�gy is a RotationPoint tulajdons�got?

        public List<PolylineGeometry> Geometries { get; set; } = new ();    //forr�s: Assets/ worldobject_poligons.json -ben a Filename nev� png-t kell kikeresni. Minden png pontosan egyszer tal�lhat� meg ott.
                                                                            //Az objektum bels� reprezent�ci�j�t adja. K�ls� reprezent�ci�t a png f�jl ad
                                                                            //PolylineGeometry oszt�ly le�r�sa l�sd: RawPolygon-ban

        public List<PolylineGeometry> RawGeometries { get; set; } = new (); //Kezdetben megegyezik a Geometries -el,
                                                                            //de m�g a Geometries-t folyamatosan transzform�ljuk a program fut�sa k�zben (eltol�s, forgat�s, stb.)
                                                                            //addig a RawGeometries a kezdeti �llapotot t�kr�zi.

        public string Filename { get; set; }            //Az objektumhoz tartoz� png f�jl neve

        public bool Collideable { get; set; }           //�tk�zhet-e

        public WorldObjectType WorldObjectType { get; set; }    //t�g �rtelemben vett t�pus. Pontos t�pust a png f�jl neve adja.

        //1 db konstruktor:
        public WorldObject(int x, int y, string filename, int zindex = 1, bool collideable = false, WorldObjectType worldObjectType = WorldObjectType.Other)
        {
            this.X = x;
            this.Y = y;
            this.Filename = filename;
            this.ZIndex = zindex;
            this.Collideable = collideable;
            this.WorldObjectType = worldObjectType;
        }
    }
}