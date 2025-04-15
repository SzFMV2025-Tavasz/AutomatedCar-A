using AutomatedCar.Helpers;
using AutomatedCar.Models;
using ReactiveUI;
using System;
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

        private const double delta_acceleration = 0.035; //in pixel/tick   //non negative real number     //acceleration increment during one tick

        private const double acceleration_friction = -2;        //in pixel/tick  

        //Buttons:
        public bool Reverse_ON { get; set; } = false;
        public bool Throttle_ON { get; set; } = false;
        public bool Brake_ON { get; set; } = false;

        //Notifcations:
        private double acceleration_dashboard = 0; //in meter/second

        private double velocity_dashboard = 0;     //in meter/second

        private int throttle_dashboard = 0;        //an integer on [0,100] interval

        private int brake_dashboard = 0;           //an integer on [-100,0] interval
        public double Acceleration_Dashboard
        {
            get => this.acceleration_dashboard;
            set => this.RaiseAndSetIfChanged(ref this.acceleration_dashboard, value);
        }

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
            this.Update_Accelerations();

            var a = this.acceleration_throttle + this.acceleration_brake + acceleration_friction;
            World.Instance.ControlledCar.Acceleration = a;

            var v = World.Instance.ControlledCar.Velocity;
            v = Math.Max(0,  v + a);
            World.Instance.ControlledCar.Velocity = v;

            if(!this.Reverse_ON)
            {
                World.Instance.ControlledCar.X += (int)(v * Math.Cos( World.Instance.ControlledCar.Rotation));  //car_1_red-et használom
                World.Instance.ControlledCar.Y += (int)(v * Math.Sin( World.Instance.ControlledCar.Rotation));
            }
            else
            {
                World.Instance.ControlledCar.X -= (int)(v * Math.Cos(World.Instance.ControlledCar.Rotation));
                World.Instance.ControlledCar.Y -= (int)(v * Math.Sin(World.Instance.ControlledCar.Rotation));
            }

            this.Acceleration_Dashboard = a * GameBase.TicksPerSecond / 50;   //MeterToPixels = 50
            this.Velocity_Dashboard = v * GameBase.TicksPerSecond / 50;
        }

        private void Update_Accelerations()
        {
            if (this.Throttle_ON && this.Throttle_Dashboard < 100)
            {
                ++this.Throttle_Dashboard;
                this.acceleration_throttle += delta_acceleration;
            }

            if (!this.Throttle_ON && this.Throttle_Dashboard > 0)
            {
                --this.Throttle_Dashboard;
                this.acceleration_throttle -= delta_acceleration;
            }

            if (this.Brake_ON && this.Brake_Dashboard > -100)
            {
                --this.Brake_Dashboard;
                this.acceleration_brake -= delta_acceleration;
            }

            if (!this.Brake_ON && this.Brake_Dashboard < 0)
            {
                ++this.Brake_Dashboard;
                this.acceleration_brake += delta_acceleration;
            }
        }

    }
}
