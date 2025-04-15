using AutomatedCar.Helpers;
using AutomatedCar.SystemComponents;
using Avalonia.Media;
using AvaloniaEdit.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedCar.Models
{
#pragma warning disable SA1101 // Prefix local calls with this
#pragma warning disable SA1600 // Elements should be documented
    public class Npc : WorldObject
    {
        public string WorldName { get; set; }

        public NpcBus NpcBus { get => this.npcBus; }

        public double Speed { get; set; }

        private NpcBus npcBus;

        private NpcPath Path { get; set; }

        private NpcPathPoint CurrentPoint { get; set; }

        // Fields used for movement
        private NpcPathPoint checkpoint;
        private double trueX;
        private double trueY;

        private double deltaX;
        private double deltaY;

        private double stintLength;
        private double trueSpeed;

        private double changeX;
        private double changeY;

        private int ticksPerStint;
        private int tickCounter = 1;

        // Until here
        public Npc(NpcPath path, string pictureFilename, WorldObjectType worldObjectType, string worldName)
            : base(path.Points[0].X, path.Points[0].Y, pictureFilename, 1, false, worldObjectType)
        {
            this.WorldName = worldName;
            this.Path = path;
            this.CurrentPoint = this.GetStartingPoint();
            this.ApplyPoint(this.CurrentPoint);
            this.ZIndex = 1000;

            this.checkpoint = GetNextPoint();
            this.trueX = this.X;
            this.trueY = this.Y;
            this.deltaX = this.checkpoint.X - this.trueX;
            this.deltaY = this.checkpoint.Y - this.trueY;
            this.stintLength = Math.Sqrt(Math.Pow(this.deltaX, 2) + Math.Pow(this.deltaY, 2));

            SpeedHelper helper = SpeedHelper.FromKmPerHour(Speed);
            double pixelsPerTick = helper.InPixelsPerTick();

            this.ticksPerStint = (int)(stintLength / pixelsPerTick);
            if (deltaX == 0)
            {
                changeY = pixelsPerTick * (deltaY / Math.Abs(deltaY));
                changeX = 0;
            }
            else if (deltaY == 0)
            {
                changeY = 0;
                changeX = pixelsPerTick * (deltaX / Math.Abs(deltaX));
            }
            else
            {
                double YtoXRatio = deltaY / deltaX;
                double LengthRatio = Math.Abs(deltaY / stintLength);
                changeY = (pixelsPerTick * LengthRatio) * (deltaY / Math.Abs(deltaY));
                changeX = changeY / YtoXRatio;
            }

            // Until here
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

        public void ApplyCheckpoint(NpcPathPoint point)
        {
            this.X = point.X;
            this.Y = point.Y;
            this.Rotation = point.Rotation;
            this.Speed = point.Speed;
            CurrentPoint = point;
            this.checkpoint = GetNextPoint();

            this.tickCounter = 1;
            this.trueX = this.X;
            this.trueY = this.Y;
            this.deltaX = this.checkpoint.X - this.trueX;
            this.deltaY = this.checkpoint.Y - this.trueY;
            this.stintLength = Math.Sqrt(Math.Pow(this.deltaX, 2) + Math.Pow(this.deltaY, 2));

            SpeedHelper helper = SpeedHelper.FromKmPerHour(Speed);
            double pixelsPerTick = helper.InPixelsPerTick();

            this.ticksPerStint = (int)(stintLength / pixelsPerTick);
            if (deltaX == 0)
            {
                changeY = pixelsPerTick * (deltaY / Math.Abs(deltaY));
                changeX = 0;
            }
            else if (deltaY == 0)
            {
                changeY = 0;
                changeX = pixelsPerTick * (deltaX / Math.Abs(deltaX));
            }
            else
            {
                double YtoXRatio = deltaY / deltaX;
                double LengthRatio = Math.Abs(deltaY / stintLength);
                changeY = (pixelsPerTick * LengthRatio) * (deltaY / Math.Abs(deltaY));
                changeX = changeY / YtoXRatio;
            }
        }

        public void Update()
        {
            if (tickCounter == ticksPerStint - 1)
            {
                //tickCounter = 0;
                ApplyCheckpoint(this.checkpoint);
            }
            else
            {
                trueX += changeX;
                trueY += changeY;
                this.X = (int)(Math.Round(trueX));
                this.Y = (int)(Math.Round(trueY));
                tickCounter++;
            }
#pragma warning restore SA1101 // Prefix local calls with this
#pragma warning restore SA1600 // Elements should be documented
        }

        public PolylineGeometry Geometry { get; set; }
    }
}
