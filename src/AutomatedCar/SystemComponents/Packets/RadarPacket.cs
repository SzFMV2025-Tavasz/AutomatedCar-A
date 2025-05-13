namespace AutomatedCar.SystemComponents.Packets
{
    using AutomatedCar.Models;
    using ReactiveUI;
    using System.Numerics;

    public class RadarPacket : ReactiveObject, IReadOnlyRadarPacket
    {
        WorldObjectType type;
        double angle;
        double distance;
        int willCollideInTicks;
        bool isInSameLane;
        public WorldObjectType Type
        {
            get => this.type;
            set => this.RaiseAndSetIfChanged(ref this.type, value);
        }
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

        public int WillCollideInTicks
        {
            get => this.willCollideInTicks;
            set => this.RaiseAndSetIfChanged(ref this.willCollideInTicks, value);
        }

        public bool IsInSameLane
        {
            get => this.isInSameLane;
            set => this.RaiseAndSetIfChanged(ref this.isInSameLane, value);
        }
    }
}
