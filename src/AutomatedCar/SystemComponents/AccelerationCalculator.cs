namespace AutomatedCar.SystemComponents
{
    using System;
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;

    internal class AccelerationCalculator : SystemComponent
    {

        //Data:
        private AccelerationPacket accelerationPacket;

        //Constants for calculation:
        public const double maxAccelerationAbs = 0.2;      //in pixel/tick. 
                                                           //the absolute value of max acceleration for throttle and brake . Depends on car capabilities.
                                                           //if throttle_acceleration reaches +maxAccelerationAbs pixel/tick, Throttle: 100 is on the Dashboard. 
                                                           //if brake_acceleration reaches -maxAccelerationAbs pixel/tick, Brake: 100 is on the Dashboard.
        private const double epsilon = 0.00001;            //for floating point comparison. 
        private const double deltaAccelerationAbs = maxAccelerationAbs / GameBase.TicksPerSecond; //if we want to reach max acceleration in 1 second while constantly pressing the throttle/brake pedal, we need to increase/decrease the acceleration by this value every tick.

        //ctor:
        public AccelerationCalculator(VirtualFunctionBus virtualFunctionBus)
            : base(virtualFunctionBus)
        {
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
            this.virtualFunctionBus.AccelerationPacket = this.accelerationPacket;   //we need this?
        }
        private void accelerationPacketInit()
        {
            this.accelerationPacket.AccelerationThrottle = 0;
            this.accelerationPacket.AccelerationBrake = 0;
        }
        private void UpdateAccelerationThrottle()
        {
            if (World.Instance.ControlledCar.ThrottleOn)    //if Up button is pressed down, ThrottleOn is true. We increase throttle acceleration, but we cant go over +maxAccelerationAbs.
            {
                this.accelerationPacket.AccelerationThrottle += deltaAccelerationAbs;
                if (this.accelerationPacket.AccelerationThrottle >= maxAccelerationAbs - epsilon)
                {
                    this.accelerationPacket.AccelerationThrottle = maxAccelerationAbs;
                }
            }
            else                                            //if Up button is not pressed down, ThrottleOn is false. We decrease throttle acceleration, but we cant go under 0.
            {
                this.accelerationPacket.AccelerationThrottle -= deltaAccelerationAbs;
                if (this.accelerationPacket.AccelerationThrottle <= 0 + epsilon)
                {
                    this.accelerationPacket.AccelerationThrottle = 0;
                }
            }
        }

        private void UpdateAccelerationBrake()                      //Likewise.
        {
            if (World.Instance.ControlledCar.BrakeOn)
            {
                this.accelerationPacket.AccelerationBrake -= deltaAccelerationAbs;
                if (this.accelerationPacket.AccelerationBrake <= -maxAccelerationAbs + epsilon)
                {
                    this.accelerationPacket.AccelerationBrake = -maxAccelerationAbs;
                }
            }
            else
            {
                this.accelerationPacket.AccelerationBrake += deltaAccelerationAbs;
                if (this.accelerationPacket.AccelerationBrake >= 0 - epsilon)
                {
                    this.accelerationPacket.AccelerationBrake = 0;
                }
            }
        }

        private void UpdateAcceleration()
        {
            //a három erő eredője:
            this.accelerationPacket.Acceleration = this.accelerationPacket.AccelerationThrottle + this.accelerationPacket.AccelerationBrake + IReadOnlyAccelerationPacket.AccelerationFriction;
        }
    }
}
