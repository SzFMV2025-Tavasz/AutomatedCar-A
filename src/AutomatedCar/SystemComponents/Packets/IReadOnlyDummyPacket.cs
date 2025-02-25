namespace AutomatedCar.SystemComponents.Packets
    {
    public interface IReadOnlyDummyPacket
    {
        int DistanceX { get; }

        int DistanceY { get; }
    }
}