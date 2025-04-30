namespace AutomatedCar.SystemComponents.Packets
{
    using AutomatedCar.Models;

    /// <summary>
    /// Represents a read-only packet containing information about a <see cref="AutomatedCar.Models.WorldObject"/> as seen by the <see cref="AutomatedCar.Models.Camera"/>.
    /// </summary>
    public interface IReadOnlyCameraPacket
    {
        /// <summary>
        /// Gets the angle from which the <see cref="AutomatedCar.Models.WorldObject"/> is coming.
        /// </summary>
        double Angle { get; }

        /// <summary>
        /// Gets the distance of the <see cref="AutomatedCar.Models.WorldObject"/> from the <see cref="AutomatedCar.Models.Camera"/>.
        /// </summary>
        double Distance { get; }

        /// <summary>
        /// Gets the speed of the <see cref="AutomatedCar.Models.WorldObject"/> relative to the <see cref="AutomatedCar.Models.Camera"/>.
        /// </summary>
        double RelativeSpeed { get; }

        /// <summary>
        /// Gets the collision relevance of the <see cref="AutomatedCar.Models.WorldObject"/>.
        /// </summary>
        WorldObjectType ObjectType { get; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="AutomatedCar.Models.WorldObject"/> is collidable.
        /// </summary>
        bool Collideable { get; }
    }
}