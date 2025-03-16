namespace AutomatedCar.SystemComponents.Sensors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;
    using System.Threading.Tasks;
    using AutomatedCar.Models;

    class Radar : SystemComponent
    {
        Circle c;
        int offsetLength;
        Vector2 dir;
        Vector2 anchor;
        public Vector2 Position
        {
            get => anchor - (dir * offsetLength);
        }
        public Vector2 Direction
        {
            get => dir;
        }
        public override void Process()
        {
            MoveDebugCircle();
        }
        void MoveDebugCircle()
        {
            if (c == null)
                return;

            c.X = (int)Position.X;
            c.Y = (int)Position.Y;
        }
        double CorrectRotation(double rotation)
        {
            if (rotation < 0)
                rotation += 360;
            rotation += 90;
            rotation %= 360;
            return rotation;
        }
        public Radar(VirtualFunctionBus virtualFunctionBus, AutomatedCar car, int offset) : base(virtualFunctionBus)
        {
            this.c = World.Instance.WorldObjects.FirstOrDefault(wo => wo is Circle) as Circle;
            this.offsetLength = offset;
            this.anchor = new Vector2(car.X, car.Y);
            this.dir = new Vector2((float)Math.Cos(CorrectRotation(car.Rotation) * (Math.PI/180)), (float)Math.Sin(CorrectRotation(car.Rotation) * (Math.PI / 180)));
            car.PropertyChangedEvent += (s, e) =>
            {
                switch (e.PropertyName)
                {
                    case "X":
                        anchor.X = car.X;
                        break;
                    case "Y":
                        anchor.Y = car.Y;
                        break;
                    case "Rotation":
                        dir.X = (float)Math.Cos(CorrectRotation(car.Rotation) * (Math.PI / 180));
                        dir.Y = (float)Math.Sin(CorrectRotation(car.Rotation) * (Math.PI / 180));
                        break;
                    default:
                        break;
                }
            };
        }
    }
}
