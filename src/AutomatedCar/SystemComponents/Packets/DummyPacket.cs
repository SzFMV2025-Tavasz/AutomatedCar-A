namespace AutomatedCar.SystemComponents.Packets
{
    using ReactiveUI;

    public class DummyPacket : ReactiveObject, IReadOnlyDummyPacket
    {
        private int distanceX;
        private int distanceY;

        public int DistanceX
        {
            get => this.distanceX;
            set => this.RaiseAndSetIfChanged(ref this.distanceX, value);
        }

        public int DistanceY
        {
            get => this.distanceY;
            set => this.RaiseAndSetIfChanged(ref this.distanceY, value);
        }
    }
}