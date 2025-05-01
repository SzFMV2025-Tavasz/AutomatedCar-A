namespace AutomatedCar.SystemComponents.Packets
{
    public class AccelerationPacket : IReadOnlyAccelerationPacket       // Determines the current Acceleration of the car.
    {
        // Datas:
        // All Acceleration values are in pixel/tick
        // Set-tel bővítjük az interfészt, követjük a DummyPacket mintát. De értesítést nem küldünk. 
        public double AccelerationThrottle { get; set; }    // 0 or positive real number

        public double AccelerationBrake { get; set; }       // 0 or negative real number

        // eredő gyorsulás = a három gyorsulás összege = AccelerationThrottle + AccelerationBrake + IReadOnlyAccelerationPacket.AccelerationFriction;
        public double Acceleration { get; set; }
    }
}