using AutomatedCar.SystemComponents.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedCar.SystemComponents
{
    class Pedals : SystemComponent
    {
        public PedalsPacket PedalsPacket { get; }
        public Pedals(VirtualFunctionBus virtualFunctionBus) : base(virtualFunctionBus)
        {
            this.PedalsPacket = new PedalsPacket();
            virtualFunctionBus.PedalsPacket = this.PedalsPacket;
        }


        public override void Process()
        {
            throw new NotImplementedException();
        }
    }
}
