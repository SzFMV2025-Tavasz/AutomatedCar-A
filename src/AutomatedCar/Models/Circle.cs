namespace AutomatedCar.Models
{
    using System;
    using Avalonia;
    using Avalonia.Media;

    /// <summary>This is a dummy object for demonstrating the codebase.</summary>
    public class Circle : WorldObject
    {
        public Circle(int x, int y, string filename, int radius)
            : base(x, y, filename)
        {
            this.Radius = radius;
            var geom = new PolylineGeometry();
            geom.Points.Add(new Point(radius, 0));
            geom.Points.Add(new Point(2 * radius, radius));
            geom.Points.Add(new Point(radius, 2 * radius));
            geom.Points.Add(new Point(0, radius));
            geom.Points.Add(new Point(radius, 0));
            this.Geometries.Add(geom);
            this.RawGeometries.Add(geom);
        }

        public int Radius { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public double CalculateArea()
        {
            return Math.PI * this.Radius * this.Radius;
        }
    }
}