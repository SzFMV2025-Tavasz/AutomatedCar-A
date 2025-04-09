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
        //privát adattagok:
        private int x;
        private int y;

        private double rotation;        //0 fok = KELET. Óramutató járásával megegyezõ irányban növekszik.


        //Publikus adattagok: -> Amikrõl értesítést küldünk:

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

        //Publikus adattagok: -> Amikrõl NEM küldünk értesítést:

        public event EventHandler<PropertyChangedEventArgs> PropertyChangedEvent;
        public int ZIndex { get; set; }                     //objektum láthatósági rétege a megjelenítés során. Minnél magasabb annál feljebb látszódik.
                                                            //Ha azt akarjuk, hogy ne takarja ki semmi, legyen minden más objektum ZIndex-énél nagyobb.
        public Point RotationPoint { get; set; }            //megadja a forgatás középpontját. Az objektumhoz tartozó (Filename nevû) png kép bal felsõ sarkától számítjuk.
                                                            //forrás: Assets/ reference_points.json -ben a Filename nevû png-t kell kikeresni. Minden png pontosan egyszer található meg ott.

        public string RenderTransformOrigin { get; set; }       //szintén a forgatás középpontját adja meg, csak egy aránnyal kifejezve.
                                                                //Ezzel lehetõvé tesszük, hogy a képet átméretezve a forgatási középpont arányosan ugyanott maradjon.
                                                                //pl. ("30%, 50%") = a kép szélességének 30%-ánál és a kép magasságának 50%-ánál legyen a forgatási középpont.
                                                                //Habár ezt végül nem fogjuk kihasználni, mert nem fogunk képet nagyítani, se kicsinyíteni.
                                                                //Miért van szükség erre a tulajdonságra, ha nem fogunk nagyítani, és már tároljuk amúgy is a RotationPoint tulajdonságot?

        public List<PolylineGeometry> Geometries { get; set; } = new ();    //forrás: Assets/ worldobject_poligons.json -ben a Filename nevû png-t kell kikeresni. Minden png pontosan egyszer található meg ott.
                                                                            //Az objektum belsõ reprezentációját adja. Külsõ reprezentációt a png fájl ad
                                                                            //PolylineGeometry osztály leírása lásd: RawPolygon-ban

        public List<PolylineGeometry> RawGeometries { get; set; } = new (); //Kezdetben megegyezik a Geometries -el,
                                                                            //de míg a Geometries-t folyamatosan transzformáljuk a program futása közben (eltolás, forgatás, stb.)
                                                                            //addig a RawGeometries a kezdeti állapotot tükrözi.

        public string Filename { get; set; }            //Az objektumhoz tartozó png fájl neve

        public bool Collideable { get; set; }           //ütközhet-e

        public WorldObjectType WorldObjectType { get; set; }    //tág értelemben vett típus. Pontos típust a png fájl neve adja.

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