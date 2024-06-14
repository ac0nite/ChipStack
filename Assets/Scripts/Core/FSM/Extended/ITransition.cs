using System;

namespace Core.FSM.Extended
{
    public interface ITransition
    {
        bool CanTransition { get; }
        Type TargetStateType { get; }
    }
}