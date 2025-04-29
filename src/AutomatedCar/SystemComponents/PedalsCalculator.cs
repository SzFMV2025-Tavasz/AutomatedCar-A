using AutomatedCar.SystemComponents.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedCar.SystemComponents
{
    class PedalsCalculator : SystemComponent        //Process() függvényét azután hívjuk miután a gyorsulások update-elve vannak, azaz az AccelerationCalculator process() függvénye már lefutott.
    {
        public PedalsPacket PedalsPacket { get; }
        public PedalsCalculator(VirtualFunctionBus virtualFunctionBus) : base(virtualFunctionBus)
        {
            this.PedalsPacket = new PedalsPacket();
            virtualFunctionBus.PedalsPacket = this.PedalsPacket;
        }


        public override void Process()
        {
            this.UpdateThrottle();
            this.UpdateBrake();
            this.virtualFunctionBus.PedalsPacket = this.PedalsPacket;   //we need this?
        }

        private void UpdateThrottle()
        {
            double accelerationThrottle = this.virtualFunctionBus.AccelerationPacket.AccelerationThrottle;
            double maxAccelerationAbs = AccelerationCalculator.maxAccelerationAbs;
            this.PedalsPacket.Throttle = (int)( (accelerationThrottle / maxAccelerationAbs ) * 100);  //accelerationThrottle hány százaléka maxAccelerationAbs-nak.
        }
        private void UpdateBrake()
        {
            double accelerationBrake = this.virtualFunctionBus.AccelerationPacket.AccelerationBrake;
            double maxAccelerationAbs = AccelerationCalculator.maxAccelerationAbs;
            this.PedalsPacket.Throttle = (int)( (-accelerationBrake / maxAccelerationAbs ) * 100);  //-accelerationBrake hány százaléka maxAccelerationAbs-nak. //negatív előjel kell mert az accelerationBrake negatív vagy 0.
        }
    }
}
