namespace AutomatedCar.Models
{
    using Avalonia.Controls;
    using Avalonia.Controls.Shapes;
    using Avalonia.Media;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
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
        public event EventHandler<PropertyChangedEventArgs> PropertyChangedEvent;

        private int x;
        private int y;
        private double rotation;

        public WorldObject(int x, int y, string filename, int zindex = 1, bool collideable = false, WorldObjectType worldObjectType = WorldObjectType.Other)
        {
            this.X = x;
            this.Y = y;
            this.Filename = filename;
            this.ZIndex = zindex;
            this.Collideable = collideable;
            this.WorldObjectType = worldObjectType;
        }

        public int ZIndex { get; set; }

        public double Rotation
        {
            get => this.rotation;
            set
            {
                this.rotation = value % 360;
                this.PropertyChangedEvent?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Rotation)));
                TransformGeometries();
            }
        }

        public int X
        {
            get => this.x;
            set
            {
                this.x = value;
                this.PropertyChangedEvent?.Invoke(this, new PropertyChangedEventArgs(nameof(this.X)));
                TransformGeometries();
            }
        }

        public int Y
        {
            get => this.y;
            set
            {
                this.y = value;
                this.PropertyChangedEvent?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Y)));
                TransformGeometries();
            }
        }

        /// <summary>
        /// List that contains the transformed points of each geometry of a world object.
        /// Example: GlobalPoints[i][j] contains the global equivalent for the point Geometries[i].Points[j].
        /// </summary>
        public List<IList<Avalonia.Point>> GlobalPoints { get; set; } = new ();
        public Point RotationPoint { get; set; }

        public string RenderTransformOrigin { get; set; }

        public List<PolylineGeometry> Geometries { get; set; } = new ();

        public List<PolylineGeometry> RawGeometries { get; set; } = new ();

        public string Filename { get; set; }

        public bool Collideable { get; set; }

        public WorldObjectType WorldObjectType { get; set; }

        public void TransformGeometries()
        {
            var tg = new TransformGroup();
            tg.Children.Add(new RotateTransform(Rotation, RotationPoint.X, RotationPoint.Y));
            tg.Children.Add(new TranslateTransform(X - RotationPoint.X, Y - RotationPoint.Y));
            for (int i = 0; i < Geometries.Count; i++)
            {
                Geometries[i].Transform = tg;
                TransformGlobalPoints(i, tg);
            }
        }
        void TransformGlobalPoints(int n, TransformGroup t)
        {
            if (GlobalPoints.Count <= 0)
            {
                for (int i = 0; i < Geometries.Count; i++)
                {
                    GlobalPoints.Add(new List<Avalonia.Point>());
                    for (int j = 0; j < Geometries[i].Points.Count; j++)
                    {
                        GlobalPoints[i].Add(new Avalonia.Point(Geometries[i].Points[j].X, Geometries[i].Points[j].Y));
                    }
                }
            }

            var ps = GlobalPoints[n];
            for (int j = 0; j < ps.Count; j++)
            {
                ps[j] = Geometries[n].Points[j];
                for (int k = 0; k < t.Children.Count; k++)
                {
                    ps[j] = ps[j].Transform(t.Children[k].Value);
                }
            }
        }
    }
}