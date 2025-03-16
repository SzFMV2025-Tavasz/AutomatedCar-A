namespace AutomatedCar.SystemComponents.Sensors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;
    using System.Threading.Tasks;
    using AutomatedCar.Models;

    class Sensor : SystemComponent
    {
        Vector2 dir;
        Vector2 offset;
        Vector2 anchor;
        public Vector2 Position
        {
            get => anchor + offset;
        }
        public Vector2 Direction
        {
            get => dir;
        }

        public override void Process()
        {
            throw new NotImplementedException();
        }
        public Sensor(VirtualFunctionBus virtualFunctionBus, AutomatedCar car, Vector2 offset) : base(virtualFunctionBus)
        {
            this.offset = offset;
            this.anchor = new Vector2(car.X, car.Y);
            this.dir = new Vector2((float)Math.Cos(car.Rotation), (float)Math.Sin(car.Rotation));
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
                        dir.X = (float)Math.Cos(car.Rotation);
                        dir.Y = (float)Math.Sin(car.Rotation);
                        break;
                    default:
                        break;
                }
            };
        }
    }
}
