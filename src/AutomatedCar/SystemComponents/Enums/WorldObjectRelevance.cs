namespace AutomatedCar.SystemComponents.Enums
{
    /// <summary>
    /// Enum representing the relevance of different world objects in the automated car system.
    /// </summary>
    public enum WorldObjectRelevance
    {
        

        /// <summary>
        /// Represents a pedestrian object.
        /// </summary>
        Pedestrian = 0,

        /// <summary>
        /// Represents a car object.
        /// </summary>
        Car = 1,

        /// <summary>
        /// Represents a lane object.
        /// </summary>
        Lane = 2,

        /// <summary>
        /// Represents other collidable objects.
        /// </summary>
        OtherCollideble = 3,

        /// <summary>
        /// Represents other non-collidable objects.
        /// </summary>
        OtherNonCollideble = 4,
    }
}
