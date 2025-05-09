namespace AutomatedCar.SystemComponents
{
    using AutomatedCar.Helpers;
    using AutomatedCar.SystemComponents.Packets;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class InputHandler : SystemComponent
    {
        public InputHandlerPacket InputHandlerPacket { get; }

        private readonly Models.AutomatedCar car;

        public InputHandler(VirtualFunctionBus virtualFunctionBus, Models.AutomatedCar car) : base(virtualFunctionBus)
        {
            this.InputHandlerPacket = new InputHandlerPacket();
            this.virtualFunctionBus.InputHandlerPacket = this.InputHandlerPacket;
            this.car = car;
        }

        public override void Process()
        {
            bool isThrottling = false;
            bool isBraking = false;
            bool isSteeringLeft = false;
            bool isSteeringRight = false;

            isThrottling = this.CheckInputPrioritiesWithCounterInput(InputRequestType.Throttle, InputRequestType.Brake);
            if (!isThrottling)
            {
                isBraking = this.CheckInputPrioritiesWithCounterInput(InputRequestType.Brake, InputRequestType.Throttle);
            }

            isSteeringLeft = this.CheckInputPrioritiesWithCounterInput(InputRequestType.SteerLeft, InputRequestType.SteerRight);
            if (!isSteeringLeft)
            {
                isSteeringRight = this.CheckInputPrioritiesWithCounterInput(InputRequestType.SteerRight, InputRequestType.SteerLeft);
            }

            this.InputHandlerPacket.Throttling = isThrottling;
            this.InputHandlerPacket.Braking = isBraking;
            this.InputHandlerPacket.SteeringLeft = isSteeringLeft;
            this.InputHandlerPacket.SteeringRight = isSteeringRight;
        }

        private bool CheckInputPrioritiesWithCounterInput(InputRequestType input, InputRequestType counterInput)
        {
            var highestPriorityInput = this.GetHighestPriorityComponentForInputRequestType(input);
            var highestPriorityCounter = this.GetHighestPriorityComponentForInputRequestType(counterInput);

            if (highestPriorityInput == null) return false;
            if (highestPriorityCounter == null) return true;
            return highestPriorityInput.Value < highestPriorityCounter.Value;
        }

        private InputRequesterComponent? GetHighestPriorityComponentForInputRequestType(InputRequestType type)
        {
            var matching = this.car.InputRequests
                .Where(kvp => kvp.Value.TryGetValue(type, out var isOn) && isOn)
                .Select(kvp => kvp.Key)
                .OrderBy(k => (int)k);

            return matching.Any() ? matching.First() : (InputRequesterComponent?)null;
        }
    }
}
