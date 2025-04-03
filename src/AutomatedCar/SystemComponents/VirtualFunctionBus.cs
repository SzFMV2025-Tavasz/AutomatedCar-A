namespace AutomatedCar.SystemComponents
{
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;
    using System.Collections.Generic;

    public class VirtualFunctionBus : GameBase
    {
        private List<SystemComponent> components = new List<SystemComponent>();
        private List<Npc> npcs = new List<Npc>();

        public IReadOnlyDummyPacket DummyPacket { get; set; }

        public void RegisterComponent(SystemComponent component)
        {
            this.components.Add(component);
        }

        protected override void Tick()
        {
            foreach (SystemComponent component in this.components)
            {
                component.Process();
            }
            foreach (Npc npc in this.npcs)
            {
                npc.Update();
            }
        }
    }
}