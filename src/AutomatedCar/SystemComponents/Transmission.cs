using AutomatedCar.Helpers;
using AutomatedCar.Models;
using ReactiveUI;
using System;

namespace AutomatedCar.SystemComponents
{
    public class Transmission : ReactiveObject
    {
        //Datas:-----------------------------------------------------------------
        private GearState gearState = GearState.P;
        private DriveGearState driveGearState = DriveGearState.None;
        private MovingState movingState = MovingState.Stay;
        public static bool ShiftGearUpRequest { get; set; } = false;
        public static bool ShiftGearDownRequest { get; set; } = false;
        private const double epsilon = 0.00001;

        //Notifications:---------------------------------------------------------
        //States:
        public GearState GearState
        {
            get => gearState;
            set => this.RaiseAndSetIfChanged(ref gearState, value);
        }
        public DriveGearState DriveGearState
        {
            get => driveGearState;
            set => this.RaiseAndSetIfChanged(ref driveGearState, value);
        }
        public MovingState MovingState
        {
            get => movingState;
            set => this.RaiseAndSetIfChanged(ref movingState, value);
        }
        //State Strings:
        private string gearStateString = "P";
        private string driveGearStateString = "None";
        private string movingStateString = "Stay";
        public string GearStateString
        {
            get => gearStateString;
            set => this.RaiseAndSetIfChanged(ref gearStateString, value);
        }
        public string DriveGearStateString
        {
            get => driveGearStateString;
            set => this.RaiseAndSetIfChanged(ref driveGearStateString, value);
        }
        public string MovingStateString
        {
            get => movingStateString;
            set => this.RaiseAndSetIfChanged(ref movingStateString, value);
        }

        //Functions:----------------------------------------------------------------------
        public void UpdateStateAndGear()
        {
            this.UpdateDriveGearState();
            switch (this.MovingState)
            {
                case MovingState.Stay:
                    //ha frissítést kérnek a gear-re:
                    if (ShiftGearUpRequest && !ShiftGearDownRequest)
                    {
                        if(this.GearState != GearState.D)
                        {
                            this.GearUp();
                        }
                        ShiftGearUpRequest = false;
                    }
                    else if (ShiftGearDownRequest && !ShiftGearUpRequest)
                    {
                        if(this.GearState != GearState.P)
                        {
                            this.GearDown();
                        }
                        ShiftGearDownRequest = false;
                    }
                    //ha nem kérnek frissítést a gear-re:
                    else
                    {
                        //ha van gyorsulás, és D-ben vagy R-ben vagyunk:--> akkor el tudunk indulni.
                        if (this.GearState == GearState.D && World.Instance.ControlledCar.Acceleration > 0 + epsilon)
                        {
                            this.MovingState = MovingState.MoveForward;
                        }
                        else if (this.GearState == GearState.R && World.Instance.ControlledCar.Acceleration > 0 + epsilon)
                        {
                            this.MovingState = MovingState.MoveBackward;
                        }
                    }
                    this.UpdateStatesStrings();
                    break;
                case MovingState.MoveForward:
                    //ha a sebesség kb 0, térjünk vissza a "Stay" állapotba:
                    if (Math.Abs(World.Instance.ControlledCar.Velocity - 0) < epsilon && World.Instance.ControlledCar.Acceleration <= 0)
                    {
                        this.MovingState = MovingState.Stay;
                    }
                    //Egyébként figyeljük, hogy van e váltás: mivel D az utolsó betű a P,R,N,D-ben, csak lefele válthatunk. és ezt engedélyezzük is:
                    else if (ShiftGearDownRequest && !ShiftGearUpRequest)
                    {
                        this.GearState = GearState.N;
                        this.MovingState = MovingState.NeutralForward;
                        ShiftGearDownRequest = false;
                    }
                    this.UpdateStatesStrings();
                    break;
                case MovingState.NeutralForward:
                    //ha a sebesség kb 0, térjünk vissza a "Stay" állapotba:
                    if (Math.Abs(World.Instance.ControlledCar.Velocity - 0) < epsilon)
                    {
                        this.MovingState = MovingState.Stay;
                    }
                    //Egyébként figyeljük, hogy van e váltás: mivel N köztes betű a P,R,N,D-ben, elvileg válthatunk lefele és felfele is. 
                    // Viszont R-be váltani ebben az állapotban az autó kiszámíthatatlan viselkedéséhez vezetne ezért ezt nem engedélyezzük. 
                    // Ellenben a D-be váltást engedélyezzük.
                    else if (!ShiftGearDownRequest && ShiftGearUpRequest)
                    {
                        this.GearState = GearState.D;
                        this.MovingState = MovingState.MoveForward;
                        ShiftGearUpRequest = false;
                    }
                    this.UpdateStatesStrings();
                    break;
                    //------------------------------------------------------------------------------------------------------
                case MovingState.MoveBackward:
                    //ha a sebesség kb 0, térjünk vissza a "Stay" állapotba:
                    if (Math.Abs(World.Instance.ControlledCar.Velocity - 0) < epsilon && World.Instance.ControlledCar.Acceleration <= 0)
                    {
                        this.MovingState = MovingState.Stay;
                    }
                    //Egyébként figyeljük, hogy van e váltás: mivel R köztes betű a P,R,N,D-ben, elvileg válthatunk lefele és felfele is. 
                    // Viszont P-be váltani ebben az állapotban az autó kiszámíthatatlan viselkedéséhez vezetne ezért ezt nem engedélyezzük. 
                    // Ellenben a N-be váltást engedélyezzük.
                    else if (!ShiftGearDownRequest && ShiftGearUpRequest)
                    {
                        this.GearState = GearState.N;
                        this.MovingState = MovingState.NeutralBackward;
                        ShiftGearUpRequest = false;
                    }
                    this.UpdateStatesStrings();
                    break;
                case MovingState.NeutralBackward:
                    //ha a sebesség kb 0, térjünk vissza a "Stay" állapotba:
                    if (Math.Abs(World.Instance.ControlledCar.Velocity - 0) < epsilon)
                    {
                        this.MovingState = MovingState.Stay;
                    }
                    //Egyébként figyeljük, hogy van e váltás: mivel N köztes betű a P,R,N,D-ben, elvileg válthatunk lefele és felfele is. 
                    // Viszont D-be váltani ebben az állapotban az autó kiszámíthatatlan viselkedéséhez vezetne ezért ezt nem engedélyezzük. 
                    // Ellenben az R-be váltást engedélyezzük.
                    else if (ShiftGearDownRequest && !ShiftGearUpRequest)
                    {
                        this.GearState = GearState.R;
                        this.MovingState = MovingState.MoveBackward;
                        ShiftGearDownRequest = false;
                    }
                    this.UpdateStatesStrings();
                    break;
                default:
                    break;
            }
        }

        // Felfelé váltás a fokozatok között (P -> R -> N -> D)
        public void GearUp()
        {
            switch (this.GearState)
            {
                case GearState.P:
                    this.GearState = GearState.R;
                    break;
                case GearState.R:
                    this.GearState = GearState.N;
                    break;
                case GearState.N:
                    this.GearState = GearState.D;
                    this.DriveGearState = DriveGearState.D1;
                    break;
                default:
                    break;
            }
        }

        // Lefelé váltás a fokozatok között (D -> N -> R -> P)
        public void GearDown()
        {
            switch (this.GearState)
            {
                case GearState.D:
                    this.GearState = GearState.N;
                    this.DriveGearState = DriveGearState.None;
                    break;
                case GearState.N:
                    this.GearState = GearState.R;
                    break;
                case GearState.R:
                    this.GearState = GearState.P;
                    break;
                default:
                    break;
            }
        }

        public void UpdateDriveGearState()
        {
            if(this.GearState != GearState.D)
            {
                this.DriveGearState = DriveGearState.None;
                return;
            }
            var velocity = World.Instance.ControlledCar.Velocity;
            switch (velocity)
            {
                case < 25:
                    this.DriveGearState = DriveGearState.D1;
                    break;
                case < 50:
                    this.DriveGearState = DriveGearState.D2;
                    break;
                case < 75:
                    this.DriveGearState = DriveGearState.D3;
                    break;
                case < 100:
                    this.DriveGearState = DriveGearState.D4;
                    break;
                case >= 100:
                    this.DriveGearState = DriveGearState.D5;
                    break;
            }
        }
        public void UpdateStatesStrings()
        {
            this.GearStateString = this.GearState.ToString();
            this.DriveGearStateString = this.DriveGearState.ToString();
            this.MovingStateString = this.MovingState.ToString();
        }
    }
}