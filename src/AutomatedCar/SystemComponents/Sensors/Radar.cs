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
        Vector2 dir;
        Vector2 offset;
        Vector2 anchor;
        public Vector2 Position
        {
            get => anchor + (offset * dir);
        }
        public Vector2 Direction
        {
            get => dir;
        }

        public override void Process()
        {
            throw new NotImplementedException();
        }
        public Radar(VirtualFunctionBus virtualFunctionBus, AutomatedCar car, Vector2 offset) : base(virtualFunctionBus)
        {
            this.offset = offset;
            this.anchor = new Vector2(car.X, car.Y);
            this.dir = new Vector2((float)Math.Cos(car.Rotation * (Math.PI/180)), (float)Math.Sin(car.Rotation * (Math.PI / 180)));
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
                        dir.X = (float)Math.Cos(car.Rotation * (Math.PI / 180));
                        dir.Y = (float)Math.Sin(car.Rotation * (Math.PI / 180));
                        break;
                    default:
                        break;
                }
            };
        }
    }
}
