using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedCar.Models
{
    class NpcPedestrian : Npc
    {
        public NpcPedestrian(NpcPath path)
           : base(path, path.Filename, WorldObjectType.Pedestrian, path.World)
        {

        }
    }
}
