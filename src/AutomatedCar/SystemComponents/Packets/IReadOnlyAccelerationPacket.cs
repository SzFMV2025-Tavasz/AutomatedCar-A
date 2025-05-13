namespace AutomatedCar.SystemComponents.Packets
{
    public interface IReadOnlyAccelerationPacket    //Determines the current Acceleration of the car.
    {
        // Datas:
        // All Acceleration values are in pixel/tick
        public double AccelerationThrottle { get; }     //0 or positive real number

        public double AccelerationBrake { get; }        //0 or negative real number

        public const double AccelerationFriction = -0.02;   //0 or negative real number // can be set to other value. For easy debugging set to 0.  

        //eredő gyorsulás: az előző három összege:
        public double Acceleration { get; }

    }
}
