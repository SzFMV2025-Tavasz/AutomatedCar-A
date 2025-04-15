namespace AutomatedCar.SystemComponents
{
    using AutomatedCar.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class NpcBus : GameBase
    {
        private Npc npc;
        public NpcBus(Npc npc)
        {
            this.npc = npc;
        }
        protected override void Tick()
        {
            npc.Update();
        }
    }
}
