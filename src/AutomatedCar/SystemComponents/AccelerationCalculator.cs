namespace AutomatedCar.SystemComponents
{
    using System;
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;

    internal class AccelerationCalculator : SystemComponent
    {

        //Data:
        private AccelerationPacket accelerationPacket;

        private readonly AutomatedCar car;

        //Constants for calculation:
        public const double maxAccelerationAbs = 0.1;      //in pixel/tick. 
                                                           //the absolute value of max acceleration for throttle and brake . Depends on car capabilities.
                                                           //if throttle_acceleration reaches +maxAccelerationAbs pixel/tick, Throttle: 100 is on the Dashboard. 
                                                           //if brake_acceleration reaches -maxAccelerationAbs pixel/tick, Brake: 100 is on the Dashboard.
        private const double deltaAccelerationAbs = maxAccelerationAbs / GameBase.TicksPerSecond; //if we want to reach max acceleration in 1 second while constantly pressing the throttle/brake pedal, we need to increase/decrease the acceleration by this value every tick.

        //ctor:
        public AccelerationCalculator(VirtualFunctionBus virtualFunctionBus, AutomatedCar car)
            : base(virtualFunctionBus)
        {
            this.car = car;
            this.accelerationPacket = new AccelerationPacket();
            this.accelerationPacketInit();
            this.virtualFunctionBus.AccelerationPacket = this.accelerationPacket;
        }

        //Methods:
        public override void Process()
        {
            this.UpdateAccelerationThrottle();
            this.UpdateAccelerationBrake();
            this.UpdateAcceleration();
            this.virtualFunctionBus.AccelerationPacket = this.accelerationPacket;
        }
        private void accelerationPacketInit()
        {
            this.accelerationPacket.AccelerationThrottle = 0;
            this.accelerationPacket.AccelerationBrake = 0;
        }
        private void UpdateAccelerationThrottle()
        {
            var accelerationThrottle = this.accelerationPacket.AccelerationThrottle;
            if (World.Instance.ControlledCar.ThrottleOn)    //if Up button is pressed down, ThrottleOn is true. We increase throttle acceleration, but we cant go over +maxAccelerationAbs.
            {
                accelerationThrottle += deltaAccelerationAbs;
                accelerationThrottle = Math.Min(accelerationThrottle, maxAccelerationAbs);
            }
            else                                            //if Up button is not pressed down, ThrottleOn is false. We decrease throttle acceleration, but we cant go under 0.
            {
                accelerationThrottle -= deltaAccelerationAbs;
                accelerationThrottle = Math.Max(accelerationThrottle, 0);
            }
            this.accelerationPacket.AccelerationThrottle = accelerationThrottle;
        }

        private void UpdateAccelerationBrake()                      //Likewise.
        {
            var accelerationBrake = this.accelerationPacket.AccelerationBrake;
            if (World.Instance.ControlledCar.BrakeOn)
            {
                accelerationBrake -= deltaAccelerationAbs;
                accelerationBrake = Math.Max(accelerationBrake, -maxAccelerationAbs);
            }
            else{
                accelerationBrake += deltaAccelerationAbs;
                accelerationBrake = Math.Min(accelerationBrake, 0);
            }
            this.accelerationPacket.AccelerationBrake = accelerationBrake;
        }

        private void UpdateAcceleration()
        {
            //a három gyorsulás összege:
            this.accelerationPacket.Acceleration = this.accelerationPacket.AccelerationThrottle + this.accelerationPacket.AccelerationBrake + IReadOnlyAccelerationPacket.AccelerationFriction;
        }
    }
}
