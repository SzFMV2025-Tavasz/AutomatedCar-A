namespace AutomatedCar.SystemComponents.Packets
{
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class SteeringWheelPacket : ReactiveObject, IReadOnlySteeringWheelPacket
    {
        private int state = 0;

        public int State
        {
            get => this.state;
            set => this.RaiseAndSetIfChanged(ref this.state, value);
        }
    }
}
