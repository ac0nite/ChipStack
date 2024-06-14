using System;

namespace Core.FSM.Extended
{
    public class DefaultTransition : ITransition
    {
        public IStateExtended OwnerState { get; internal set; }
        public bool CanTransition => OwnerState.IsCompleted;
        public Type TargetStateType { get; set; }
    }
}