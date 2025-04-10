namespace AutomatedCar.SystemComponents.Packets
{
    using AutomatedCar.Models;
    using System.Numerics;

    public interface IReadOnlyRadarPacket
    {
        double Angle { get; }
        double Distance { get; }
        Vector2 RelativeVelocity { get; }
        WorldObjectType Type { get; }
    }
}
