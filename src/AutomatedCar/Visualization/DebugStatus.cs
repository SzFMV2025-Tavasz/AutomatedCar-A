namespace AutomatedCar.Visualization
{
    using ReactiveUI;

    public class DebugStatus : ReactiveObject
    {
        private bool enabled = false;

        private bool camera = false;

        private bool radar = false;

        private bool rotate = false;

        private bool ultrasonic = false;

        public bool Enabled { get => this.enabled; set => this.RaiseAndSetIfChanged(ref this.enabled, value); }

        public bool Camera { get => this.camera; set => this.RaiseAndSetIfChanged(ref this.camera, value); }

        public bool Radar { get => this.radar; set => this.RaiseAndSetIfChanged(ref this.radar, value); }

        public bool Rotate { get => this.rotate; set => this.RaiseAndSetIfChanged(ref this.rotate, value); }

        public bool Ultrasonic { get => this.ultrasonic; set => this.RaiseAndSetIfChanged(ref this.ultrasonic, value); }
    }
}
