namespace AutomatedCar.SystemComponents.Packets
{
    using AutomatedCar.SystemComponents.Enums;
    using ReactiveUI;
    using System;

    /// <summary>
    /// Represents a packet containing information about a <see cref="AutomatedCar.Models.WorldObject"/> as seen by the <see cref="AutomatedCar.Models.Camera"/>.
    /// </summary>
    public class CameraPacket : ReactiveObject, IReadOnlyCameraPacket
    {
        private float angle;
        private float distance;
        private float relativeSpeed;
        private WorldObjectRelevance worldObjectType;

        /// <summary>
        /// Gets or sets the angle from which the <see cref="AutomatedCar.Models.WorldObject"/> is coming.
        /// </summary>
        public float Angle
        {
            get => this.angle;
            set => this.RaiseAndSetIfChanged(ref this.angle, value);
        }

        /// <summary>
        /// Gets or sets the distance of the <see cref="AutomatedCar.Models.WorldObject"/> from the <see cref="AutomatedCar.Models.Camera"/>.
        /// </summary>
        public float Distance
        {
            get => this.distance;
            set => this.RaiseAndSetIfChanged(ref this.distance, value);
        }

        /// <summary>
        /// Gets or sets the speed of the <see cref="AutomatedCar.Models.WorldObject"/> relative to the <see cref="AutomatedCar.Models.Camera"/>.
        /// </summary>
        public float RelativeSpeed
        {
            get => this.relativeSpeed;
            set => this.RaiseAndSetIfChanged(ref this.relativeSpeed, value);
        }

        /// <summary>
        /// Gets or sets the collision relevance of the <see cref="AutomatedCar.Models.WorldObject"/>.
        /// </summary>
        public WorldObjectRelevance WorldObjectType
        {
            get => this.WorldObjectType;
            set => this.RaiseAndSetIfChanged(ref this.worldObjectType, value);
        }
    }
}