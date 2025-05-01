using System.Collections.ObjectModel;
using AutomatedCar.Models;
using System.Linq;

using ReactiveUI;

namespace AutomatedCar.ViewModels
{
    using Avalonia.Controls;
    using Models;
    using System;
    using Visualization;

    public class CourseDisplayViewModel : ViewModelBase
    {
        public ObservableCollection<WorldObjectViewModel> WorldObjects { get; } = new ObservableCollection<WorldObjectViewModel>();

        private Avalonia.Vector offset;

        public ScrollViewer ScrollViewer { get; set; }

        public CourseDisplayViewModel(World world)
        {
            this.WorldObjects = new ObservableCollection<WorldObjectViewModel>(world.WorldObjects.Select(wo => new WorldObjectViewModel(wo)));
            this.Width = world.Width;
            this.Height = world.Height;

            // Feliratkozás a ControlledCar pozícióváltozásaira
            World.Instance.ControlledCar.PropertyChangedEvent += this.OnControlledCarPositionChanged;
        }

        public int Width { get; set; }

        public int Height { get; set; }

        public Avalonia.Vector Offset
        {
            get => this.offset;
            set => this.RaiseAndSetIfChanged(ref this.offset, value);
        }

        private DebugStatus debugStatus = new DebugStatus();

        public DebugStatus DebugStatus
        {
            get => this.debugStatus;
            set => this.RaiseAndSetIfChanged(ref this.debugStatus, value);
        }

        internal void ThrottleOnSet(bool value)
        {
            World.Instance.ControlledCar.ThrottleOn = value;
        }

        internal void BrakeOnSet(bool value)
        {
            World.Instance.ControlledCar.BrakeOn = value;
        }

        public void ToggleDebug()
        {
            this.debugStatus.Enabled = !this.debugStatus.Enabled;
        }

        public void ToggleCamera()
        {
            this.DebugStatus.Camera = !this.DebugStatus.Camera;
        }

        public void ToggleRadar()
        {
            // World.Instance.DebugStatus.Radar = !World.Instance.DebugStatus.Radar;
        }

        public void ToggleUltrasonic()
        {
            //World.Instance.DebugStatus.Ultrasonic = !World.Instance.DebugStatus.Ultrasonic;
        }

        public void ToggleRotation()
        {
            //World.Instance.DebugStatus.Rotate = !World.Instance.DebugStatus.Rotate;
        }

        internal void SetSteeringLeft(bool v)
        {
            World.Instance.ControlledCar.SteeringLeft = v;
        }

        internal void SetSteeringRight(bool v)
        {
            World.Instance.ControlledCar.SteeringRight = v;
        }
        private void OnControlledCarPositionChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(WorldObject.X) || e.PropertyName == nameof(WorldObject.Y))
            {
                // Hívjuk meg a FocusCar metódust, amikor az X vagy Y változik
                this.FocusCar();
            }
        }

        public void UpdateControlledCarEvents()
        {
            // Távolítsuk el az eseménykezelõt a jelenlegi autóról (ha van)
            // Ez biztosítja, hogy ne legyen duplikált feliratkozás
            //if (World.Instance.ControlledCar != null)

            World.Instance.ControlledCar.PropertyChangedEvent -= this.OnControlledCarPositionChanged;


            // Ha történt autóváltás, akkor iratkozzunk fel az új autóra
            //if (World.Instance.ControlledCar != null)


            World.Instance.ControlledCar.PropertyChangedEvent += this.OnControlledCarPositionChanged;

            // Azonnali fókusz az új autóra
            this.FocusCar();

        }
        public void FocusCar()
        {
            var offsetX = World.Instance.ControlledCar.X - (this.ScrollViewer.Viewport.Width / 2);
            var offsetY = World.Instance.ControlledCar.Y - (this.ScrollViewer.Viewport.Height / 2);
            this.Offset = new Avalonia.Vector(offsetX, offsetY);
        }
    }
}