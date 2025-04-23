using System.Collections.ObjectModel;
using AutomatedCar.Models;
using System.Linq;

using ReactiveUI;

namespace AutomatedCar.ViewModels
{
    using AutomatedCar.SystemComponents;
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

        // Q billentyű: váltás felfelé
        public void ShiftGearUp()
        {
            Transmission.ShiftGearUp = true;
        }
        
        // A billentyű: váltás lefelé
        public void ShiftGearDown()
        {
            Transmission.ShiftGearDown = true;
        }
        
        public void ShiftGearUp_Off()
        {
           Transmission.ShiftGearUp = false;
        }
        public void ShiftGearDown_Off()
        {
            Transmission.ShiftGearDown = false;
        }

        public void Throttle_ON()
        {
            Powertrain.Throttle_ON = true;
        }

        public void Throttle_OFF()
        {
            Powertrain.Throttle_ON = false; 
        }

        public void Brake_ON()
        {
            Powertrain.Brake_ON = true; 
        }

        public void Brake_OFF()
        {
            Powertrain.Brake_ON = false; 
        }
        public void KeyLeft()
        {
            World.Instance.ControlledCar.Rotation -= 5;
        }

        public void KeyRight()
        {
            World.Instance.ControlledCar.Rotation += 5;
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
            // Távolítsuk el az eseménykezelőt a jelenlegi autóról (ha van)
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