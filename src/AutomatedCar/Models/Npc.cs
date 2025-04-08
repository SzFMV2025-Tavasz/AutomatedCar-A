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
        private int pointIndex = 3;
        private NpcBus npcBus;
        public NpcBus NpcBus { get => this.npcBus; }
        public double Speed { get; set; }

        private NpcPath Path { get; set; }

        private NpcPathPoint CurrentPoint { get; set; }
        public string WorldName { get; set; }
        private NpcPathPoint checkpoint;
        private int delta_X;
        private int delta_Y;

        public Npc(NpcPath path, string pictureFilename, WorldObjectType worldObjectType, string worldName)
            : base(path.Points[0].X, path.Points[0].Y, pictureFilename, 1, false, worldObjectType)
        {
            this.WorldName = worldName;
            this.Path = path;
            this.CurrentPoint = this.GetStartingPoint();
            this.ApplyPoint(this.CurrentPoint);
            this.npcBus = new NpcBus(this);
            this.checkpoint = Path.Points[pointIndex];
            this.delta_X = checkpoint.X - this.X;
            this.delta_Y = checkpoint.Y - this.Y;
        }

        public void Start()
        {
            this.npcBus.Start();
        }
        public void Stop()
        {
            this.npcBus.Stop();
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
            if (pointIndex < Path.Points.Count)
            {
                
                if (delta_X == 0)
                {
                    this.Y += (int)(delta_Y/Speed);
                    //hosszPerTick számítás
                }
                else if(delta_Y == 0)
                {
                    this.X += (int)(delta_X / Speed);
                    //hosszPerTick számítás
                }
                else
                {
                    double meredekseg = (double)delta_Y / delta_X; //Annak az értéke, hogy egy pixel elmozdulás a vízszintes(x) tengelyen hány pixel elmozdulást jelent a függőleges(y) tengelyen
                    double trueLength = Math.Sqrt(Math.Pow(delta_X, 2) + Math.Pow(delta_Y, 2)); //Jelenlegi és a checkpoint közötti egyenes hosszának kiszámolása
                    this.X += 1;
                    this.Y += X * (int)Math.Round(meredekseg);
                }
               
                

            }
 
        }
    }
}
