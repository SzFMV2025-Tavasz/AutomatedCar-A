namespace AutomatedCar.SystemComponents.Packets
{
    using AutomatedCar.Models;

    public interface IReadOnlyRadarPacket
    {
        double Angle { get; }
        double Distance { get; }
        double RelativeSpeed { get; }
        WorldObjectType Type { get; }
    }
}
