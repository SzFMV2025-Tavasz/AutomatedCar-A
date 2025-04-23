using AutomatedCar.Helpers;
using AutomatedCar.Models;
using ReactiveUI;
using System;

namespace AutomatedCar.SystemComponents
{
    public class Transmission : ReactiveObject
    {
        //Datas:
        private string gear = "P";
        private string dstate = "-";
        private string state = "Stay";
        public static bool ShiftGearUp { get; set; } = false;
        public static bool ShiftGearDown { get; set; } = false;
        //Notifications:
        public string Gear
        {
            get => gear;
            set => this.RaiseAndSetIfChanged(ref this.gear, value);
        }
        public string DState
        {
            get => dstate;
            set => this.RaiseAndSetIfChanged(ref this.dstate, value);
        }

        public string State
        {
            get => state;
            set => this.RaiseAndSetIfChanged(ref this.state, value);
        }
        
        //Functions:
        public void UpdateStateAndGear()
        {
            const double epsilon = 0.0001;
            switch (this.State)
            {
                case "Stay":
                    //ha frissítést kérnek a gear-re:
                    if (ShiftGearUp && !ShiftGearDown)
                    {
                        if(this.Gear != "D")
                        {
                            this.GearUp();
                        }
                        ShiftGearUp = false;
                    }
                    else if (ShiftGearDown && !ShiftGearUp)
                    {
                        if(this.Gear != "P")
                        {
                            this.GearDown();
                        }
                        ShiftGearDown = false;
                    }
                    //ha nem kérnek frissítést a gear-re:
                    else
                    {
                        //ha van gyorsulás, és D-ben vagy R-ben vagyunk:--> akkor el tudunk indulni.
                        if (this.Gear == "D" && World.Instance.ControlledCar.Acceleration > 0 + epsilon)
                        {
                            this.State = "Move_Forward";
                        }
                        else if(this.Gear=="R" && World.Instance.ControlledCar.Acceleration > 0 + epsilon)
                        {
                            this.State = "Move_Backward";
                        }
                    }
                    break;
                case "Move_Forward":
                    this.UpdateDState();
                    //ha a sebesség kb 0, térjünk vissza a "Stay" állapotba:
                    if (Math.Abs(World.Instance.ControlledCar.Velocity - 0) < epsilon && World.Instance.ControlledCar.Acceleration <= 0)
                    {
                        this.State = "Stay";
                        this.DState = "-";
                    }
                    //Egyébként figyeljük, hogy van e váltás: mivel D az utolsó betű a P,R,N,D-ben, csak lefele válthatunk. és ezt engedélyezzük is:
                    else if (ShiftGearDown && !ShiftGearUp)
                    {
                        this.Gear = "N";
                        this.State = "Neutral_forward";
                        this.DState = "-";
                        ShiftGearDown = false;
                    }
                    break;
                case "Neutral_forward":
                    //ha a sebesség kb 0, térjünk vissza a "Stay" állapotba:
                    if (Math.Abs(World.Instance.ControlledCar.Velocity - 0) < epsilon)
                    {
                        this.State = "Stay";
                        this.DState = "-";
                    }
                    //Egyébként figyeljük, hogy van e váltás: mivel N köztes betű a P,R,N,D-ben, elvileg válthatunk lefele és felfele is. 
                    // Viszont R-be váltani ebben az állapotban az autó kiszámíthatatlan viselkedéséhez vezetne ezért ezt nem engedélyezzük. 
                    // Ellenben a D-be váltást engedélyezzük.
                    else if (!ShiftGearDown && ShiftGearUp)
                    {
                        this.Gear = "D";
                        this.State = "Move_forward";
                        ShiftGearUp = false;
                    }
                    break;
                    //------------------------------------------------------------------------------------------------------
                case "Move_Backward":
                    //ha a sebesség kb 0, térjünk vissza a "Stay" állapotba:
                    if (Math.Abs(World.Instance.ControlledCar.Velocity - 0) < epsilon && World.Instance.ControlledCar.Acceleration <= 0)
                    {
                        this.State = "Stay";
                    }
                    //Egyébként figyeljük, hogy van e váltás: mivel R köztes betű a P,R,N,D-ben, elvileg válthatunk lefele és felfele is. 
                    // Viszont P-be váltani ebben az állapotban az autó kiszámíthatatlan viselkedéséhez vezetne ezért ezt nem engedélyezzük. 
                    // Ellenben a N-be váltást engedélyezzük.
                    else if (!ShiftGearDown && ShiftGearUp)
                    {
                        this.Gear = "N";
                        this.State = "Neutral_backward";
                        ShiftGearUp = false;
                    }
                    break;
                case "Neutral_backward":
                    //ha a sebesség kb 0, térjünk vissza a "Stay" állapotba:
                    if (Math.Abs(World.Instance.ControlledCar.Velocity - 0) < epsilon)
                    {
                        this.State = "Stay";
                    }
                    //Egyébként figyeljük, hogy van e váltás: mivel N köztes betű a P,R,N,D-ben, elvileg válthatunk lefele és felfele is. 
                    // Viszont D-be váltani ebben az állapotban az autó kiszámíthatatlan viselkedéséhez vezetne ezért ezt nem engedélyezzük. 
                    // Ellenben az R-be váltást engedélyezzük.
                    else if (ShiftGearDown && !ShiftGearUp)
                    {
                        this.Gear = "R";
                        this.State = "Move_backward";
                        ShiftGearDown = false;
                    }
                    break;
                default:
                    break;
            }
        }

        // Felfelé váltás a fokozatok között (P -> R -> N -> D)
        public void GearUp()
        {
            switch (this.Gear)
            {
                case "P":
                    this.Gear = "R";
                    break;
                case "R":
                    this.Gear = "N";
                    break;
                case "N":
                    this.Gear = "D";
                    break;
                default:
                    // D állapotban már nem lehet feljebb váltani
                    break;
            }
        }

        // Lefelé váltás a fokozatok között (D -> N -> R -> P)
        public void GearDown()
        {
            switch (this.Gear)
            {
                case "D":
                    this.Gear = "N";
                    break;
                case "N":
                    this.Gear = "R";
                    break;
                case "R":
                    this.Gear = "P";
                    break;
                default:
                    // P állapotban már nem lehet lejjebb váltani
                    break;
            }
        }

        public void UpdateDState()
        {
            var v = World.Instance.ControlledCar.Velocity;
            switch(v)
            {
                case < 25:
                    this.DState = "D1";
                    break;
                case < 50:
                    this.DState = "D2";
                    break;
                case < 75:
                    this.DState = "D3";
                    break;
                case < 100:
                    this.DState = "D4";
                    break;
                case >= 100:
                    this.DState = "D5";
                    break;
            }
        }
    }
}