namespace AutomatedCar
{
    using System;
    using Avalonia.Threading;

    public abstract class GameBase
    {
        public const int TicksPerSecond = 60;

        private readonly DispatcherTimer timer =
            new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 0, 1000 / TicksPerSecond) };

        protected GameBase()
        {
            this.timer.Tick += delegate { this.DoTick(); };
        }

        public long CurrentTick { get; private set; }

        public void Start() => this.timer.IsEnabled = true;

        public void Stop() => this.timer.IsEnabled = false;

        protected abstract void Tick();

        private void DoTick()
        {
            this.Tick();
            this.CurrentTick++;
        }
    }
}