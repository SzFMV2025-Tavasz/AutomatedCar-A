namespace AutomatedCar.SystemComponents.Packets
{
    using AutomatedCar.Models;
    using ReactiveUI;
    using System.Numerics;

    public class RadarPacket : ReactiveObject, IReadOnlyRadarPacket
    {
        double angle;
        double distance;
        Vector2 relativeVelocity;
        WorldObjectType type;
        public double Angle
        {
            get => this.angle;
            set => this.RaiseAndSetIfChanged(ref this.angle, value);
        }
        public double Distance
        {
            get => this.distance;
            set => this.RaiseAndSetIfChanged(ref this.distance, value);
        }
        public Vector2 RelativeVelocity
        {
            get => this.relativeVelocity;
            set => this.RaiseAndSetIfChanged(ref this.relativeVelocity, value);
        }
        public WorldObjectType Type
        {
            get => this.type;
            set => this.RaiseAndSetIfChanged(ref this.type, value);
        }
    }
}
