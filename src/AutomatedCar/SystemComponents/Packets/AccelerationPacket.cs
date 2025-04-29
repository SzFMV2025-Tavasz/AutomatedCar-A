namespace AutomatedCar.SystemComponents.Packets
{
    public class AccelerationPacket : IReadOnlyAccelerationPacket       // Determines the current Acceleration of the car.
    {
        // Datas:
        // All Acceleration values are in pixel/tick
        // Set-tel bővítjük az interfészt, követjük a DummyPacket mintát. De értesítést nem küldünk. 
        public double AccelerationThrottle { get; set; }    // 0 or positive real number

        public double AccelerationBrake { get; set; }       // 0 or negative real number

        // In IReadOnlyAccelerationPacket: public const double AccelerationFriction = 0;      //0 or negative real number // can be set to other value. For easy debugging set to 0.

        // eredő gyorsulás: az előző három összege:
        public double Acceleration { get; set; }
    }
}