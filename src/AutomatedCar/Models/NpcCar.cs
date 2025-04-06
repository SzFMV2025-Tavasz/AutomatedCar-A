using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedCar.Models
{
    class NpcCar : Npc
    {
        public NpcCar(NpcPath path) 
            : base(path, path.Filename, WorldObjectType.Car)
        {
            
        }
    }
}
