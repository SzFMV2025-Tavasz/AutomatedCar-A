namespace AutomatedCar.SystemComponents.Packets
{
    using AutomatedCar.Models;
    using ReactiveUI;
    public class RadarPacket : ReactiveObject, IReadOnlyRadarPacket
    {
        double angle;
        double distance;
        double relativeSpeed;
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
        public double RelativeSpeed
        {
            get => this.relativeSpeed;
            set => this.RaiseAndSetIfChanged(ref this.relativeSpeed, value);
        }
        public WorldObjectType Type
        {
            get => this.type;
            set => this.RaiseAndSetIfChanged(ref this.type, value);
        }
    }
}
