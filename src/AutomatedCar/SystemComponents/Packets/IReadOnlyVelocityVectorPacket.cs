namespace AutomatedCar.SystemComponents.Packets
{
    using Avalonia;

    public interface IReadOnlyVelocityVectorPacket      //Determines the current VelocityVector of the car.
    {
        //Datas:
        public double RadianAngleOfView { get; }   //radiánban megadja mely irányba néz éppen az autó. Ha kelet felé néz, akkor ez az érték 0.
                                                   //A kezdeti 90 fokos elforgatások miatt: RadianAngleOfView  = Rad(Car.Rotation) - Pi/2.
                                                   //(Mert 90 fokkal kell kompenzálni mind két autót: white_car_1, red_car_1.  Lásd: App.xaml.cs) 
        public double RadianAngleOfMove {get;}     //előrementben: = RadianAngleOfView .
                                                   //hátramenetben: = RadianAngleOfView + Pi .
                                                   //Mj.: a két radián a Car.Rotation értékét NEM állítja, csupán lekérdezi, és számol vele. (!)

        public Vector UnitVector { get; }   // Unit vector of the velocity vector, normalized to length 1. 
                                            // (a sebességvektor egységnyi hosszú irányvektora) 
                                            //RadianAngleOfMove-ból egyszerű cos,sin függvényekkel kiszámítható.
        public double VelocityAbs { get; }   //a sebesség abszolút értéke, pixel/tick-ben. 
                                             //A gyorsulás függvényében módosul. Emiatt minden tickben előbb a gyorsulást kell kalkulálni, aztán a sebességet.
                                             //World.Instance.ControlledCar.Velocity értékét állítja.
        public Vector VelocityVector { get; }   //a sebességvektor = UnitVector * VelocityAbs;
    }
}