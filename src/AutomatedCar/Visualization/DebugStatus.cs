namespace AutomatedCar.Visualization
{
    using ReactiveUI;

    public class DebugStatus : ReactiveObject
    {
        private bool enabled = true;

        private bool camera = true;

        private bool radar = true;

        private bool rotate = true;

        private bool ultrasonic = true;

        public bool Enabled { get => this.enabled; set => this.RaiseAndSetIfChanged(ref this.enabled, value); }

        public bool Camera { get => this.camera; set => this.RaiseAndSetIfChanged(ref this.camera, value); }

        public bool Radar { get => this.radar; set => this.RaiseAndSetIfChanged(ref this.radar, value); }

        public bool Rotate { get => this.rotate; set => this.RaiseAndSetIfChanged(ref this.rotate, value); }

        public bool Ultrasonic { get => this.ultrasonic; set => this.RaiseAndSetIfChanged(ref this.ultrasonic, value); }
    }
}
