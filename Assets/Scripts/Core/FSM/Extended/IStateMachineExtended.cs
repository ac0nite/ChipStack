using Core.FSM.Base;

namespace Core.FSM.Extended
{
    public interface IStateMachineExtended : IStateMachine
    {
        void RegisterTransition<TOwner>(params ITransition[] targetTransition);
        void RegisterTransition<TOwner, TTarget>();
    }
}