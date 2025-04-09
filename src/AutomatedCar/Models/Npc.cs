using AutomatedCar.SystemComponents;
using AvaloniaEdit.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedCar.Models
{
    public class Npc : WorldObject
    {
        
        private NpcBus npcBus;
        public NpcBus NpcBus { get => this.npcBus; }
        public double Speed { get; set; }

        private NpcPath Path { get; set; }

        private NpcPathPoint CurrentPoint { get; set; }
        public string WorldName { get; set; }
        private NpcPathPoint checkpoint;
        private int delta_X;
        private int delta_Y;
        
        private int ticksPerStint;
        private int tickCounter = 0;

        public Npc(NpcPath path, string pictureFilename, WorldObjectType worldObjectType, string worldName)
            : base(path.Points[0].X, path.Points[0].Y, pictureFilename, 1, false, worldObjectType)
        {
            this.WorldName = worldName;
            this.Path = path;
            this.CurrentPoint = this.GetStartingPoint();
            this.ApplyPoint(this.CurrentPoint);

            this.checkpoint = GetNextPoint();
            this.delta_X = checkpoint.X - this.X;
            this.delta_Y = checkpoint.Y - this.Y;
            if (delta_X > delta_Y)
            {
                this.ticksPerStint = (int)(delta_X / (Speed / GameBase.TicksPerSecond)); //Hány tick alatt érjük el a checkpointot
            }
            else
            {
                this.ticksPerStint = (int)(delta_Y / (Speed / GameBase.TicksPerSecond));
            }
            this.npcBus = new NpcBus(this);
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
            if (tickCounter > ticksPerStint)
            {
                tickCounter = 0;
                ApplyPoint(this.checkpoint);
                CurrentPoint = this.checkpoint;
                this.checkpoint = GetNextPoint();
                if (this.checkpoint != null)
                {
                    this.delta_X = checkpoint.X - this.X;
                    this.delta_Y = checkpoint.Y - this.Y;
                    if (delta_X > delta_Y)
                    {
                        ticksPerStint = (int)(delta_X / (Speed / GameBase.TicksPerSecond)); //Hány tick alatt érjük el a checkpointot
                    }
                    else
                    {
                        ticksPerStint = (int)(delta_Y / (Speed / GameBase.TicksPerSecond)); //Hány tick alatt érjük el a checkpointot
                    }
                }
            }
            else if (delta_X == 0)
            {
                this.Y += (int)((Speed / GameBase.TicksPerSecond)) * (delta_Y / Math.Abs(delta_Y));
                tickCounter++;
            }
            else if (delta_Y == 0)
            {
                this.X += (int)((Speed / GameBase.TicksPerSecond)) * (delta_X / Math.Abs(delta_X));
                tickCounter++;
            }
            else
            {
                int xChange = 0;
                int yChange = 0;
                double meredekseg = (double)delta_Y / delta_X; //Annak az értéke, hogy egy pixel elmozdulás a vízszintes(x) tengelyen hány pixel elmozdulást jelent a függőleges(y) tengelyen
                if (Math.Abs(delta_Y) > Math.Abs(delta_X))
                {

                    yChange = (int)((Speed / GameBase.TicksPerSecond) * (delta_Y/Math.Abs(delta_Y)));
                    xChange = (int)(yChange / Math.Round(meredekseg));
                }
                else
                {
                    xChange = (int)((Speed / GameBase.TicksPerSecond) * (delta_X / Math.Abs(delta_X)));
                    yChange = (int)(xChange * Math.Round(meredekseg));
                }
                //double lengthRatio = Math.Abs(delta_Y / trueLength);
                this.Y += yChange;
                this.X += xChange;
                tickCounter++;

            }

        }
    }
}
