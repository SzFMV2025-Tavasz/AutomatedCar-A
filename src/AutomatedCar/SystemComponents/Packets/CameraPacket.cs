namespace AutomatedCar.SystemComponents.Packets
{
    using AutomatedCar.Models;
    using ReactiveUI;

    /// <summary>
    /// Represents a packet containing information about a <see cref="AutomatedCar.Models.WorldObject"/> as seen by the <see cref="AutomatedCar.Models.Camera"/>.
    /// </summary>
    public class CameraPacket : ReactiveObject, IReadOnlyCameraPacket
    {
        private double angle;
        private double distance;
        private double relativeSpeed;
        private WorldObjectType objectType;
        private bool collideable;

        /// <summary>
        /// Gets or sets the angle from which the <see cref="AutomatedCar.Models.WorldObject"/> is coming.
        /// </summary>
        public double Angle
        {
            get => this.angle;
            set => this.RaiseAndSetIfChanged(ref this.angle, value);
        }

        /// <summary>
        /// Gets or sets the distance of the <see cref="AutomatedCar.Models.WorldObject"/> from the <see cref="AutomatedCar.Models.Camera"/>.
        /// </summary>
        public double Distance
        {
            get => this.distance;
            set => this.RaiseAndSetIfChanged(ref this.distance, value);
        }

        /// <summary>
        /// Gets or sets the speed of the <see cref="AutomatedCar.Models.WorldObject"/> relative to the <see cref="AutomatedCar.Models.Camera"/>.
        /// </summary>
        public double RelativeSpeed
        {
            get => this.relativeSpeed;
            set => this.RaiseAndSetIfChanged(ref this.relativeSpeed, value);
        }

        /// <summary>
        /// Gets or sets the collision relevance of the <see cref="AutomatedCar.Models.WorldObject"/>.
        /// </summary>
        public WorldObjectType ObjectType
        {
            get => this.objectType;
            set => this.RaiseAndSetIfChanged(ref this.objectType, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="AutomatedCar.Models.WorldObject"/> is collidable.
        /// </summary>
        public bool Collideable
        {
            get => this.collideable;
            set => this.RaiseAndSetIfChanged(ref this.collideable, value);
        }
    }
}