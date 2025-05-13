namespace AutomatedCar.SystemComponents.Packets
{
    using AutomatedCar.Models;
    using System.Collections.Generic;
    using System.Numerics;

    public interface IReadOnlyRadarPacket
    {
        WorldObjectType Type { get; }
        double Angle { get; }
        double Distance { get; }
        int WillCollideInTicks { get; }
        bool IsInSameLane { get; }
    }
}
