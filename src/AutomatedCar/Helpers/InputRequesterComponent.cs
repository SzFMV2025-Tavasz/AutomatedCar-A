namespace AutomatedCar.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Enum for the system components that can request input from the automated car.
    /// </summary>
    public enum InputRequesterComponent
    {
        /// <summary>
        /// The driver (user) of the car (software).
        /// </summary>
        Driver = 0,

        /// <summary>
        /// The emergency braking system.
        /// </summary>
        EmergencyBrake = 1,

        /// <summary>
        /// The lane keeping assist system.
        /// </summary>
        LaneKeepingAssist = 2,

        /// <summary>
        /// The cruise control system.
        /// </summary>
        CruiseControl = 3,
    }
}
