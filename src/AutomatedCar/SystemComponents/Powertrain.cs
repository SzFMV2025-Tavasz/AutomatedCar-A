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
        //Datas:
        
        private double acceleration_brake = 0;     //in pixel/tick   //negative real number

        private double acceleration_throttle = 0;  //in pixel/tick   //non negative real number

        private const double acceleration_friction = 0;        //in pixel/tick      //Can be set.
        private int tick_counter = 0; //in ticks        //for smoother appereance
        public Transmission Transmission { get; set; } = new Transmission();

        public const double MaxVelocityForward = 130 * 50 / (GameBase.TicksPerSecond * 3.6); //in pixel/tick   //max sebesség előremenetben: 130 km/h

        public const double MaxVelocityBackward = 20 * 50 / (GameBase.TicksPerSecond * 3.6); //in pixel/tick   //max sebesség tolatáskor: 20 km/h

        //Buttons:
        public static bool Throttle_ON { get; set; } = false;
        public static bool Brake_ON { get; set; } = false;

        public static bool Reverse_ON { get; set; } = false;

        //Notifcations:

        private double velocity_dashboard = 0;     //in km/h

        private int throttle_dashboard = 0;        //an integer on [0,100] interval

        private int brake_dashboard = 0;           //an integer on [-100,0] interval

        public double Velocity_Dashboard
        {
            get => this.velocity_dashboard;
            set => this.RaiseAndSetIfChanged(ref this.velocity_dashboard, value);
        }

        public int Throttle_Dashboard
        {
            get => this.throttle_dashboard;
            set => this.RaiseAndSetIfChanged(ref this.throttle_dashboard, value);
        }

        public int Brake_Dashboard
        {
            get => this.brake_dashboard;
            set => this.RaiseAndSetIfChanged(ref this.brake_dashboard, value);
        }

        //Functions:
        public void Process()
        {   
            this.tick_counter++;
            this.Transmission.UpdateStateAndGear();
            this.Update_Acceleration_Throttle();
            this.Update_Acceleration_Brake();
            this.Update_Car_Acceleration();
            this.Update_Car_Velocity();
            this.Update_Reverse_On();

            if(this.tick_counter == 10)
            {
                Console.WriteLine($"Car position - X: {World.Instance.ControlledCar.X}, Y: {World.Instance.ControlledCar.Y}, Reverse: {Reverse_ON}, Throttle_ON: {Throttle_ON}, Brake_ON: {Brake_ON}, Velocity: {World.Instance.ControlledCar.Velocity}, Acceleration: {World.Instance.ControlledCar.Acceleration}");
                Console.WriteLine("State: "+ this.Transmission.State +" Gear: " + this.Transmission.Gear );
                Vector V = this.Calculate_Velocity_Vector();
                World.Instance.ControlledCar.X += (int)( 10 * V.X);
                World.Instance.ControlledCar.Y += (int)( 10 * V.Y);
                this.tick_counter = 0;
            }
            this.Velocity_Dashboard = 3.6 * World.Instance.ControlledCar.Velocity * GameBase.TicksPerSecond / 50;
        }

        //constants: 
        //60 = GameBase.TicksPerSecond,  
        //0,2 = max acceleration for throttle and brake, 
        //0.0001 = epsilon
        private void Update_Acceleration_Throttle()
        {
            if (Throttle_ON)
            {
                this.acceleration_throttle += 0.003;                         // 0.003 = 0.2 / 60
                if (this.acceleration_throttle >= 0.2 - 0.0001)
                {
                    this.acceleration_throttle = 0.2;
                }
            }
            else
            {
                this.acceleration_throttle -= 0.003;
                if (this.acceleration_throttle <= 0 + 0.0001)
                {
                    this.acceleration_throttle = 0;
                }
            }
            this.Throttle_Dashboard = (int)(this.acceleration_throttle / 0.002);    // 0.002 = 0.2 / 100
        }

        private void Update_Acceleration_Brake()
        {
            if (Brake_ON)
            {
                this.acceleration_brake -= 0.003;
                if (this.acceleration_brake <= -0.2 + 0.0001)
                {
                    this.acceleration_brake = -0.2;
                }
            }
            else
            {
                this.acceleration_brake += 0.003;
                if (this.acceleration_brake >= 0 - 0.0001)
                {
                    this.acceleration_brake = 0;
                }
            }
            this.Brake_Dashboard = -1 * (int)(this.acceleration_brake / 0.002);
        }

        private void Update_Car_Acceleration()
        {
            if(this.Transmission.Gear == "N")
            {
                World.Instance.ControlledCar.Acceleration = acceleration_friction;
            }
            else{
                World.Instance.ControlledCar.Acceleration = acceleration_friction + this.acceleration_throttle + this.acceleration_brake;
            }
        }

        private void Update_Car_Velocity()
        {
            if(this.Transmission.State == "Stay"){
                World.Instance.ControlledCar.Velocity = 0;
            }
            else{
                double v = Math.Max(0, World.Instance.ControlledCar.Velocity + World.Instance.ControlledCar.Acceleration);
                if(!Reverse_ON && v > MaxVelocityForward)
                {
                    v = MaxVelocityForward;
                }
                else if (Reverse_ON && v > MaxVelocityBackward)
                {
                    v = MaxVelocityBackward;
                }
                World.Instance.ControlledCar.Velocity = v;
            }
        }

        private void Update_Reverse_On()
        {
            if (this.Transmission.State == "Move_Backward" || this.Transmission.State == "Neutral_Backward")
            {
                Reverse_ON = true;
            }
            else
            {
                Reverse_ON = false;
            }
        }

        private Vector Calculate_Velocity_Vector()
        {
            //előremenet esetén:
            //a sebességvektor irányvektora = i=(cos(Rotation - 90°), sin(Rotation - 90°)). Ebbe az irányba néz az autó. (pl. ha i=(1,0) akkor kelet felé néz.)
            //Jelenleg úgy vannak inicializáva az autók (piros és fehér), hogy 90 fokkal kompenzálni kell a kezdeti elforgatást.
            // és sebességvektor = World.Instance.ControlledCar.Velocity * i
            double x = World.Instance.ControlledCar.Velocity * Math.Cos((World.Instance.ControlledCar.Rotation * Math.PI / 180) - (Math.PI / 2));
            double y = World.Instance.ControlledCar.Velocity * Math.Sin((World.Instance.ControlledCar.Rotation * Math.PI / 180) - (Math.PI / 2));
            if(!Reverse_ON) return new Vector(x, y);

            //Ha tolatunk akkor viszont megfordítjuk a sebességvektort:
            return new Vector(-x, -y);
        }
    }
}
