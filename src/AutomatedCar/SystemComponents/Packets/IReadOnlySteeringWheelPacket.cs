namespace AutomatedCar.SystemComponents.Packets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IReadOnlySteeringWheelPacket
    {
        /// <summary>
        /// The state of the steering wheel. A number between -450 and 450, where 0 is the straight state.
        /// </summary>
        int SteeringWheelState { get; }

        /// <summary>
        /// The angle of the front wheels of the car in degrees. A number between -60 and 60, where 0 is the straight state.
        /// </summary>
        int FrontWheelState { get; }
    }
}
