namespace AutomatedCar.SystemComponents
{
    using System;
    using AutomatedCar.Helpers;
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;
    using Avalonia;

    internal class VelocityVectorCalculator : SystemComponent
    {
        //Data:
        private VelocityVectorPacket velocityVectorPacket;

        //Constants: //Velocity Limits based on the car's capabilities:
        private static readonly Speed maxVelocityForward = Speed.FromKmPerHour(130);   //max sebesség előremenetben: 130 km/h
        private static readonly Speed maxVelocityBackward = Speed.FromKmPerHour(20);   //max sebesség tolatáskor: 20 km/h

        //ctor:
        public VelocityVectorCalculator(VirtualFunctionBus virtualFunctionBus)
            : base(virtualFunctionBus)
        {
            this.velocityVectorPacket = new VelocityVectorPacket();
            this.virtualFunctionBus.VelocityVectorPacket = this.velocityVectorPacket;
        }

        //Methods:
        public override void Process()      //after we called the AccelerationCalculator's Process method, and the VirtualFunctionBus.AccelerationPacket is already updated.
        {
            this.UpdateRadianAngleOFView();
            this.UpdateRadianAngleOfMove();
            this.UpdateUnitVector();
            this.UpdateVelocityAbs();
            this.UpdateVelocityVector();
            this.virtualFunctionBus.VelocityVectorPacket = this.velocityVectorPacket;   //we need this?
        }

        private void UpdateRadianAngleOFView()
        {
            this.velocityVectorPacket.RadianAngleOfView = (World.Instance.ControlledCar.Rotation * Math.PI / 180) - (Math.PI / 2);
        }
        private void UpdateRadianAngleOfMove()
        {
            this.velocityVectorPacket.RadianAngleOfMove = this.velocityVectorPacket.RadianAngleOfView;
            if (World.Instance.ControlledCar.ReverseOn)
            {
                this.velocityVectorPacket.RadianAngleOfMove += Math.PI;
            }
        }
        private void UpdateUnitVector()
        {
            //Részletes levezetés:
            //legyen theta = velocityVectorPacket.RadianAngleOfMove. (Ahol theta = 0 a keleti irány.)

            //Descartes féle koordinátarendszerben a szög számítása a jól megszokott módon működik:
            //Óramutató járásával ellentétesen felmért szög a pozitív, 
            //és az óramutató járásával megegyezően felmért szög a negatív.
            //Így Descartes féle koordinátarendszerben a keresett vektor: UnitVector = (cos(theta), sin(theta)).

            //viszont az Avalonia féle szögszámítás pont fordítottja a Descartes féle szögszámításnak.
            //így a keresett vektor: UnitVector = (cos(-theta), sin(-theta))

            //ehhez hozzájön az, hogy az Avalonia féle koordinátarendszer = a Descartes féle koordinátarendszer tükrözve az x-tengelyre.
            //így a keresett vektor: UnitVector = (cos(-theta), -sin(-theta)). Ez pedig egyenlő (cos(theta), sin(theta))-val.
            //(Felhasznált tulajdonságok : cos(-theta) = cos(theta), sin(-theta) = -sin(theta).)
            //Tehát a keresett vektor: UnitVector = (cos(theta), sin(theta)).
            
            double x = Math.Cos(this.velocityVectorPacket.RadianAngleOfMove);
            double y = Math.Sin(this.velocityVectorPacket.RadianAngleOfMove);
            this.velocityVectorPacket.UnitVector = new Vector(x, y);
        }

        private void UpdateVelocityAbs()
        {
            double updatedAcceleration = World.Instance.ControlledCar.VirtualFunctionBus.AccelerationPacket.Acceleration;  //(AccelerationCalculator's Process() function already called!)
            
            //lekérdezem a sebességet:
            double velocityAbs = World.Instance.ControlledCar.Velocity.InPixelsPerTick();

            //módosítom a gyorsulás szerint:
            velocityAbs = Math.Max(0, velocityAbs + updatedAcceleration);

            bool reverseOn = World.Instance.ControlledCar.ReverseOn;

            if(!reverseOn && velocityAbs > maxVelocityForward.InPixelsPerTick())    //sebességet behatároljuk.
            {
                velocityAbs = maxVelocityForward.InPixelsPerTick();
            }
            else if (reverseOn && velocityAbs > maxVelocityBackward.InPixelsPerTick())
            {
                velocityAbs = maxVelocityBackward.InPixelsPerTick();
            }

            //beírom a változást a változókba:
            this.velocityVectorPacket.VelocityAbs = velocityAbs;
            World.Instance.ControlledCar.Velocity = Speed.FromPixelsPerTick(velocityAbs); //a sebesség abszolút értékét a ControlledCar.Velocity értékébe
                                                                                                                    //is beállítjuk.
        }

        private void UpdateVelocityVector()
        {
            this.velocityVectorPacket.VelocityVector = this.velocityVectorPacket.UnitVector * this.velocityVectorPacket.VelocityAbs; //a sebességvektor = UnitVector * VelocityAbs;
        }
    }
}