namespace AutomatedCar.SystemComponents
{
    // Váltó fokozatok (P, R, N, D)
    public enum GearState
    {
        P,  // Park
        R,  // Reverse
        N,  // Neutral
        D   // Drive
    }

    // Belső fokozatok Drive állásban (D1-D5)
    public enum DriveGearState
    {
        None,
        D1,
        D2,
        D3,
        D4,
        D5
    }

    // Váltó működési állapotai
    public enum MovingState
    {
        Stay,              // Álló helyzet
        MoveForward,       // Előre mozgás
        NeutralForward,    // Előre gurulás üresben
        MoveBackward,      // Hátra mozgás
        NeutralBackward    // Hátra gurulás üresben
    }
}