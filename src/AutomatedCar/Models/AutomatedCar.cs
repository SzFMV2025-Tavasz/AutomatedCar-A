namespace AutomatedCar.Models
{
    using Avalonia.Media;
    using SystemComponents;

    public class AutomatedCar : Car
    {
        private VirtualFunctionBus virtualFunctionBus;

        public AutomatedCar(int x, int y, string filename)
            : base(x, y, filename)
        {
            this.virtualFunctionBus = new VirtualFunctionBus();
            this.ZIndex = 10;
        }

        public VirtualFunctionBus VirtualFunctionBus { get => this.virtualFunctionBus; }        //UML: lásd a tantárgy jegyzete.

        public int Revolution { get; set; }

        public int Velocity { get; set; }           //akkor különítsük el a következõ fogalmakat: Revolution, Velocity, Speed... ->(?)

        public PolylineGeometry Geometry { get; set; }      //egy plusz geometria, a Geometries és RawGeometries geometriák mellett. -> MineK?

        public void Start()
        {
            this.virtualFunctionBus.Start();
        }

        public void Stop()
        {
            this.virtualFunctionBus.Stop();
        }
    }
}