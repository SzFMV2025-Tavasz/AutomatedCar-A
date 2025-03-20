namespace AutomatedCar.SystemComponents.Packets
{
    using AutomatedCar.SystemComponents.Enums;
    using System;

    /// <summary>
    /// Represents a read-only packet containing information about a <see cref="AutomatedCar.Models.WorldObject"/> as seen by the <see cref="AutomatedCar.Models.Camera"/>.
    /// </summary>
    public interface IReadOnlyCameraPacket
    {
        /// <summary>
        /// Gets the angle from which the <see cref="AutomatedCar.Models.WorldObject"/> is coming.
        /// </summary>
        float Angle { get; }

        /// <summary>
        /// Gets the distance of the <see cref="AutomatedCar.Models.WorldObject"/> from the <see cref="AutomatedCar.Models.Camera"/>.
        /// </summary>
        float Distance { get; }

        /// <summary>
        /// Gets the speed of the <see cref="AutomatedCar.Models.WorldObject"/> relative to the <see cref="AutomatedCar.Models.Camera"/>.
        /// </summary>
        float RelativeSpeed { get; }

        /// <summary>
        /// Gets the collision relevance of the <see cref="AutomatedCar.Models.WorldObject"/>.
        /// </summary>
        WorldObjectRelevance WorldObjectType { get; }
    }
}