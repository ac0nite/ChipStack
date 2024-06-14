using System.Collections.Generic;
using Core.FSM.Base;

namespace Core.FSM.Extended
{
    public interface IStateExtended : IState
    {
        void AddTransitions(params ITransition[] transitions);
        IReadOnlyList<ITransition> Transitions { get; }
        bool IsCompleted { get; }
    }
}