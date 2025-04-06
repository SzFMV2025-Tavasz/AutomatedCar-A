using AutomatedCar.SystemComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedCar.Models
{
    public class Npc : WorldObject
    {
        
        public double Speed { get; set; }

        private NpcPath Path { get; set; }

        private NpcPathPoint CurrentPoint { get; set; }
        public string WorldName { get; set; }

        public Npc(NpcPath path, string pictureFilename, WorldObjectType worldObjectType, string worldName)
            : base(path.Points[0].X, path.Points[0].Y, pictureFilename, 1, false, worldObjectType)
        {
            this.WorldName = worldName;
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
        public void Update()
        {
            
            //foreach (var point in Path.Points)
            //{
            //    var checkPoint = point;
            //    int i = 0;
            //    while (CurrentPoint != checkPoint)
            //    {
            //        var nextPoint
            //        ApplyPoint(point);
            //    }

            //}
            //Path.Points
            //NpcPathPoint? nextPoint = this.GetNextPoint();
            //if (nextPoint != null)
            //{
            //    this.CurrentPoint = nextPoint;
            //    this.ApplyPoint(this.CurrentPoint);
            //}
        }
    }
}
