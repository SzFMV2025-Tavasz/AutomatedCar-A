namespace AutomatedCar.SystemComponents.Sensors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AutomatedCar.Models;

    class Sensor : SystemComponent
    {
        public Sensor(VirtualFunctionBus virtualFunctionBus) : base(virtualFunctionBus)
        {
        }

        public override void Process()
        {
            throw new NotImplementedException();
        }
    }
}
