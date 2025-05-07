namespace AutomatedCar.SystemComponents
{
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;
    using System.Collections.Generic;

    public class VirtualFunctionBus : GameBase
    {
        private List<SystemComponent> components = new List<SystemComponent>();
       

        public IReadOnlyDummyPacket DummyPacket { get; set; }

        /// <summary>
        /// Gets or sets the collection of read-only packets containing information from the camera.
        /// </summary>
        public IList<IReadOnlyCameraPacket> CameraPackets { get; set; }
        public IList<IReadOnlyRadarPacket> RadarPackets { get; set; }

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
           
        }
    }
}