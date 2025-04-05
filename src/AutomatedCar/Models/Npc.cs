using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedCar.Models
{
    class Npc : WorldObject
    {
        public double Speed { get; set; }

        private NpcPath Path { get; set; }

        private NpcPathPoint CurrentPoint { get; set; }

        public Npc(NpcPath path, string worldFilename, WorldObjectType worldObjectType)
            : base(path.Points[0].X, path.Points[0].Y, worldFilename, 1, false, worldObjectType)
        {
            this.Path = path;
            this.CurrentPoint = this.GetStartingPoint();
            this.ApplyPoint(this.CurrentPoint);
        }

        public NpcPathPoint GetStartingPoint()
        {
            return this.Path.Points[0];
        }

        public NpcPathPoint? GetNextPoint()
        {
            int currentIndex = this.Path.Points.IndexOf(this.CurrentPoint);
            if (currentIndex + 1 < this.Path.Points.Count)
            {
                return this.Path.Points[currentIndex + 1];
            }
            else if (this.Path.Loop)
            {
                return this.Path.Points[0];
            }
            else
            {
                return null;
            }
        }

        public void ApplyPoint(NpcPathPoint point)
        {
            this.X = point.X;
            this.Y = point.Y;
            this.Rotation = point.Rotation;
            this.Speed = point.Speed;
        }
    }
}
