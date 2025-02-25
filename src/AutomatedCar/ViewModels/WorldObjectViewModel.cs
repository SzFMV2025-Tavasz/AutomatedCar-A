namespace AutomatedCar.ViewModels
{
    using AutomatedCar.Models;
    using Avalonia.Media;
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class WorldObjectViewModel : ViewModelBase
    {
        private WorldObject worldObject;

        public WorldObjectViewModel(WorldObject worldObject)
        {
            this.worldObject = worldObject;
            this.worldObject.PropertyChangedEvent += this.OnPropertyChanged;
        }


        private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            this.RaisePropertyChanged(args.PropertyName);
        }

        public int X
        {
            get => this.worldObject.X;
        }

        public int Y
        {
            get => this.worldObject.Y;
        }

        public double Rotation
        {
            get => this.worldObject.Rotation;
        }

        public int ZIndex 
        {
            get => this.worldObject.ZIndex;
        }

        public string Filename
        {
            get => this.worldObject.Filename;
        }

        public Point RotationPoint 
        {
            get => this.worldObject.RotationPoint;
        }

        public string RenderTransformOrigin
        {
            get => this.worldObject.RenderTransformOrigin;
        }

        public List<PolylineGeometry> Geometries
        {
            get => this.worldObject.Geometries;
        }

        public List<PolylineGeometry> RawGeometries
        {
            get => this.worldObject.RawGeometries;
        }


        public bool Collideable
        {
            get => this.worldObject.Collideable;
        }

        public WorldObjectType WorldObjectType
        {
            get => this.worldObject.WorldObjectType;
        }
    }
}
