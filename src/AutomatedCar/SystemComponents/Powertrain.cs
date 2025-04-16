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

        private const double acceleration_friction = 0;        //in pixel/tick  

        private int tick_counter = 0; //in ticks        //for smoother appereance

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
            this.Update_Accelerations_debug();
            this.tick_counter++;

            var a = this.acceleration_throttle + this.acceleration_brake + acceleration_friction;
            World.Instance.ControlledCar.Acceleration = a;

            var v = World.Instance.ControlledCar.Velocity;
            v = Math.Max(0,  v + a);
            World.Instance.ControlledCar.Velocity = v;

            if(this.tick_counter == 10)
            {
                double radian = (World.Instance.ControlledCar.Rotation * Math.PI / 180) - (Math.PI / 2);   //90 fokkal kompenzáljuk a kezdeti elforgatást
                int incx = (int)( 10 * v * Math.Cos(radian));
                int incy = (int)( 10 * v * Math.Sin(radian));
                if(!this.Reverse_ON)
                {
                    World.Instance.ControlledCar.X += incx;
                    World.Instance.ControlledCar.Y += incy;
                }
                else
                {
                    World.Instance.ControlledCar.X -= incx;
                    World.Instance.ControlledCar.Y -= incy;
                }
                this.tick_counter = 0;
            }

            this.Acceleration_Dashboard = a * GameBase.TicksPerSecond / 50;   //MeterToPixels = 50
            this.Velocity_Dashboard = v * GameBase.TicksPerSecond / 50;
        }

        private void Update_Accelerations()
        {
            if (this.Throttle_ON && this.Throttle_Dashboard < 100)
            {
                ++this.Throttle_Dashboard;
            }

            if (!this.Throttle_ON && this.Throttle_Dashboard > 0)
            {
                --this.Throttle_Dashboard;
            }

            if (this.Brake_ON && this.Brake_Dashboard > -100)
            {
                --this.Brake_Dashboard;
            }

            if (!this.Brake_ON && this.Brake_Dashboard < 0)
            {
                ++this.Brake_Dashboard;
            }

            this.acceleration_throttle = this.Throttle_Dashboard * delta_acceleration; //10 pixel/tick max
            this.acceleration_brake = this.Brake_Dashboard * delta_acceleration; //10 pixel/tick max
        }

        private void Update_Accelerations_debug()
        {
            const double a_debug = 0.2;
            if (this.Throttle_ON)
            {
                this.Throttle_Dashboard = 1;
                this.acceleration_throttle = a_debug;
                ;
            }
            if (!this.Throttle_ON)
            {
                this.Throttle_Dashboard = 0;
                this.acceleration_throttle = 0;
            }
            if (this.Brake_ON)
            {
                this.Brake_Dashboard = 1;
                this.acceleration_brake = (-1) * a_debug;
            }
            if (!this.Brake_ON)
            {
                this.Brake_Dashboard = 0;
                this.acceleration_brake = 0;
            }
        }
    }
}
