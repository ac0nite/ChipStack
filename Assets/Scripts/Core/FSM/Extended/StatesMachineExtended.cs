using System;
using Core.FSM.Base;

namespace Core.FSM.Extended
{
    public class StatesMachineExtended : StatesMachine
    {
        protected new IStateExtended _currentState;
        
        public void RegisterTransition<TOwner>(params ITransition[] transition)
        {
            GetExtendedState(typeof(TOwner)).AddTransitions(transition);
        }

        public void RegisterTransition<TOwner, TTarget>()
        {
            var ownerState = GetExtendedState(typeof(TOwner));
            ownerState.AddTransitions(new DefaultTransition()
            {
                OwnerState = ownerState,
                TargetStateType = typeof(TTarget)
            });
        }
		
        private IStateExtended GetExtendedState(Type type)
        {
            if (_states.TryGetValue(type, out var state))
                return state as IStateExtended;
            else
                throw new Exception($"State with type {type} not found");
        }

        public override void Update(float deltaTime)
        {
            var transitions = _currentState.Transitions;
            for (var i = 0; i < transitions.Count; i++)
            {
                if (transitions[i].CanTransition && transitions[i].TargetStateType != _currentState.GetType())
                {
                    NextState(transitions[i].TargetStateType);
                    break;
                }
            }

            base.Update(deltaTime);
        }
    }
}