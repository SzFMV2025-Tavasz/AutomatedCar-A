namespace AutomatedCar.SystemComponents.Packets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class InputHandlerPacket : IReadOnlyInputHandlerPacket
    {
        public bool Throttling { get; set; }

        public bool Braking { get; set; }

        public bool SteeringLeft { get; set; }

        public bool SteeringRight { get; set; }
    }
}
