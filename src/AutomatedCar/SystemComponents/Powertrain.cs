using AutomatedCar.Helpers;
using AutomatedCar.Models;
using ReactiveUI;
using System;
using Avalonia;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedCar.SystemComponents
{
    public class Powertrain : ReactiveObject
    {
        //Datas:-------------------------------------------------------------------------------
        public Transmission Transmission { get; set; } = new Transmission();
        private double accelerationBrake = 0;                 //in pixel/tick   //negative real number
        private double accelerationThrottle = 0;              //in pixel/tick   //non negative real number
        private int tickCounter = 0;                          //for smoother appereance
        private int tickCounterMax = 20;

        //used constants:
        private const double epsilon = 0.00001;                  //for floating point comparison
        private const double maxAcceleration = 0.2;              //in pixel/tick   //max acceleration for throttle and brake: 0.2 pixel/tick
        private const double accelerationFriction = 0;           //in pixel/tick     //negative real number  //Can be set.
        private const double maxVelocityForwardInKmPerHour = 130;
        private const double maxVelocityBackwardInKmPerHour = 20;
        private const int ticksPerSecond = GameBase.TicksPerSecond;     //currently 60  
        private const double meterToPixels = Speed.MeterToPixels;       //currently 50  //this means that 1 meter = 50 pixels.    
        private const double deltaAcceleration = maxAcceleration / ticksPerSecond; //in pixel/tick
        private const double maxVelocityForwardInPixelPerTick = maxVelocityForwardInKmPerHour * meterToPixels / (GameBase.TicksPerSecond * 3.6); //in pixel/tick   //max sebesség előremenetben: 130 km/h
        private const double maxVelocityBackwardInPixelPerTick = maxVelocityBackwardInKmPerHour * meterToPixels / (GameBase.TicksPerSecond * 3.6); //in pixel/tick   //max sebesség tolatáskor: 20 km/h
        
        //Buttons:--------------------------------------------------------------------------------------
        public static bool ThrottleOn { get; set; } = false;
        public static bool BrakeOn { get; set; } = false;
        public static bool ReverseOn { get; set; } = false;

        //Notifcations for the Dashboard: -----------------------------------------------------------------------------------

        private double velocityDashboard = 0;     //in km/h

        private int throttleDashboard = 0;        //an integer on [0,100] interval

        private int brakeDashboard = 0;           //an integer on [0,100] interval

        public double VelocityDashboard
        {
            get => this.velocityDashboard;
            set => this.RaiseAndSetIfChanged(ref this.velocityDashboard, value);
        }

        public int ThrottleDashboard
        {
            get => this.throttleDashboard;
            set => this.RaiseAndSetIfChanged(ref this.throttleDashboard, value);
        }

        public int BrakeDashboard
        {
            get => this.brakeDashboard;
            set => this.RaiseAndSetIfChanged(ref this.brakeDashboard, value);
        }

        //Functions:---------------------------------------------------------------------------------------------
        public void Process()
        {   
            this.tickCounter++;
            this.Transmission.UpdateStateAndGear();
            this.UpdateAccelerationThrottle();
            this.UpdateAccelerationBrake();
            this.UpdateCarAcceleration();
            this.UpdateCarVelocity();
            this.UpdateReverseOn();
            this.UpdateDashboard();

            Vector velocity = this.CalculateVelocityVector();   //in pixel/tick
            this.UpdateTickCounterMax();

            if(this.tickCounter >= this.tickCounterMax)
            {
                World.Instance.ControlledCar.X += (int)( this.tickCounterMax * velocity.X);
                World.Instance.ControlledCar.Y += (int)( this.tickCounterMax * velocity.Y);
                this.tickCounter = 0;
            }
        }

        private void UpdateAccelerationThrottle()
        {
            if (ThrottleOn)
            {
                this.accelerationThrottle += deltaAcceleration;
                if (this.accelerationThrottle >= maxAcceleration - epsilon)
                {
                    this.accelerationThrottle = maxAcceleration;
                }
            }
            else
            {
                this.accelerationThrottle -= deltaAcceleration;
                if (this.accelerationThrottle <= 0 + epsilon)
                {
                    this.accelerationThrottle = 0;
                }
            }
        }

        private void UpdateAccelerationBrake()
        {
            if (BrakeOn)
            {
                this.accelerationBrake -= deltaAcceleration;
                if (this.accelerationBrake <= -maxAcceleration + epsilon)
                {
                    this.accelerationBrake = -maxAcceleration;
                }
            }
            else
            {
                this.accelerationBrake += deltaAcceleration;
                if (this.accelerationBrake >= 0 - epsilon)
                {
                    this.accelerationBrake = 0;
                }
            }
        }

        private void UpdateCarAcceleration()
        {
            if(this.Transmission.GearState == GearState.N)
            {
                World.Instance.ControlledCar.Acceleration = accelerationFriction;
            }
            else{
                World.Instance.ControlledCar.Acceleration = accelerationFriction + this.accelerationThrottle + this.accelerationBrake;
            }
        }

        private void UpdateCarVelocity()
        {
            if(this.Transmission.MovingState == MovingState.Stay){
                World.Instance.ControlledCar.Velocity = 0;
            }
            else{
                double v = Math.Max(0, World.Instance.ControlledCar.Velocity + World.Instance.ControlledCar.Acceleration);
                if(!ReverseOn && v > maxVelocityForwardInPixelPerTick)
                {
                    v = maxVelocityForwardInPixelPerTick;
                }
                else if (ReverseOn && v > maxVelocityBackwardInPixelPerTick)
                {
                    v = maxVelocityBackwardInPixelPerTick;
                }
                World.Instance.ControlledCar.Velocity = v;
            }
        }

        private void UpdateReverseOn()
        {
            if (this.Transmission.MovingState == MovingState.MoveBackward || this.Transmission.MovingState == MovingState.NeutralBackward)
            {
                ReverseOn = true;
            }
            else
            {
                ReverseOn = false;
            }
        }

        private Vector CalculateVelocityVector()
        {
            //előremenet esetén:
            //a sebességvektor irányvektora = i=(cos(Rotation - 90°), sin(Rotation - 90°)). Ebbe az irányba néz az autó. (pl. ha i=(1,0) akkor kelet felé néz.)
            //Jelenleg úgy vannak inicializáva az autók (piros és fehér), hogy 90 fokkal kompenzálni kell a kezdeti elforgatást.
            // és sebességvektor = World.Instance.ControlledCar.Velocity * i
            double x = World.Instance.ControlledCar.Velocity * Math.Cos((World.Instance.ControlledCar.Rotation * Math.PI / 180) - (Math.PI / 2));
            double y = World.Instance.ControlledCar.Velocity * Math.Sin((World.Instance.ControlledCar.Rotation * Math.PI / 180) - (Math.PI / 2));
            if(!ReverseOn) return new Vector(x, y);

            //Ha tolatunk akkor viszont megfordítjuk a sebességvektort:
            return new Vector(-x, -y);
        }

        private void UpdateTickCounterMax()
        {
            switch(World.Instance.ControlledCar.Velocity)
            {
                case <= 1:
                    this.tickCounterMax = 20;
                    break;
                case <= 2:
                    this.tickCounterMax = 15;
                    break;
                case <= 3:
                    this.tickCounterMax = 7;
                    break;
                case <= 4:
                    this.tickCounterMax = 5;
                    break;
                case <= 5:
                    this.tickCounterMax = 3;
                    break;
                case <= 6:
                    this.tickCounterMax = 2;
                    break;
                case <= 8:
                    this.tickCounterMax = 2;
                    break;
                case > 8:
                    this.tickCounterMax = 1;
                    break;
            }
        }
        private void UpdateDashboard()
        {
            this.VelocityDashboard = 3.6 * World.Instance.ControlledCar.Velocity * GameBase.TicksPerSecond / meterToPixels; //in km/h
            this.ThrottleDashboard = (int)(this.accelerationThrottle / (maxAcceleration / 100));
            this.BrakeDashboard = -1 * (int)(this.accelerationBrake / (maxAcceleration / 100));
        }
    }
}
