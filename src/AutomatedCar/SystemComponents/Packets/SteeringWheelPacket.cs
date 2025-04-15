namespace AutomatedCar.SystemComponents.Packets
{
    using ReactiveUI;

    internal class SteeringWheelPacket : ReactiveObject, IReadOnlySteeringWheelPacket
    {
        private int steeringWheelState = 0;
        private int frontWheelState = 0;

        public int SteeringWheelState
        {
            get => this.steeringWheelState;
            set
            {
                this.FrontWheelState = (int)this.ScaleNumber((double)value, -450.0, 450.0, -60.0, 60.0);
                this.RaiseAndSetIfChanged(ref this.steeringWheelState, value);
            }
        }

        public int FrontWheelState
        {
            get => this.frontWheelState;
            private set => this.RaiseAndSetIfChanged(ref this.frontWheelState, value);
        }

        private double ScaleNumber(double value, double fromMin, double fromMax, double toMin, double toMax)
        {
            return ((value - fromMin) / (fromMax - fromMin) * (toMax - toMin)) + toMin;
        }
    }
}
