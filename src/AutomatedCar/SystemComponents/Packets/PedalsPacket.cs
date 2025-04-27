namespace AutomatedCar.SystemComponents.Packets
{
    using ReactiveUI;

    internal class PedalsPacket : ReactiveObject, IReadOnlyPedalsPacket
    {
        private int throttle = 0;
        private int brake = 0;

        public int Throttle
        {
            get => this.throttle;
            set => this.RaiseAndSetIfChanged(ref this.throttle, value);
        }

        public int Brake
        {
            get => this.brake;
            set => this.RaiseAndSetIfChanged(ref this.brake, value);
        }

    }
}