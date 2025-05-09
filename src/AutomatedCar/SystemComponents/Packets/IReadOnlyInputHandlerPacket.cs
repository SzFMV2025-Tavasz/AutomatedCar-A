namespace AutomatedCar.SystemComponents.Packets
{
    using AutomatedCar.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IReadOnlyInputHandlerPacket
    {
        /// <summary>
        /// Gets a value indicating whether is the car throttling.
        /// </summary>
        bool Throttling { get; }

        /// <summary>
        /// Gets a value indicating whether is the car braking.
        /// </summary>
        public bool Braking { get; }

        /// <summary>
        /// Gets a value indicating whether is the car steering left.
        /// </summary>
        public bool SteeringLeft { get; }

        /// <summary>
        /// Gets a value indicating whether is the car steering right.
        /// </summary>
        public bool SteeringRight { get; }
    }
}
