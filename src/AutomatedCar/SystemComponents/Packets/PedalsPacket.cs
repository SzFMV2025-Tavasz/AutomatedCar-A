namespace AutomatedCar.SystemComponents.Packets
{
    using ReactiveUI;

    public class PedalsPacket : ReactiveObject, IReadOnlyPedalsPacket
    {
        //Set-tel bővítjük az interfészt, követjük a DummyPacket mintát. 
        //Értesítést küldünk, mert a Dashboard-on szeretnénk megjeleníteni az adatokat.
        private int throttle = 0;   // an integer on [0, 100]
        private int brake = 0;      // an integer on [0, 100]

        public int Throttle
        {
            get => this.throttle;
            set => this.RaiseAndSetIfChanged(ref this.throttle, value);
        }

        public int Brake
        {
            get => this.brake;
            set => this.RaiseAndSetIfChanged(ref this.brake, value);
        }

    }
}