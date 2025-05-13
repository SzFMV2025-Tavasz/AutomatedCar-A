namespace AutomatedCar.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// The type of inputs the different system components can request.
    /// </summary>
    public enum InputRequestType
    {
        /// <summary>
        /// Steer the steering wheel left.
        /// </summary>
        SteerLeft,

        /// <summary>
        /// Steer the steering wheel right.
        /// </summary>
        SteerRight,

        /// <summary>
        /// Push the throttle pedal down.
        /// </summary>
        Throttle,

        /// <summary>
        /// Push the brake pedal down.
        /// </summary>
        Brake,
    }
}
