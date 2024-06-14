using System;
using System.Collections.Generic;

namespace Core.FSM.Base
{
    public class StatesMachine : IStateMachine
    {
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        protected Dictionary<Type, IState> _states = new();
        protected IState _currentState;

        public virtual StatesMachine Register<T>(IState state) where T : IState
        {
            _states.Add(typeof(T), state);
            return this;
        }
        
        public virtual StatesMachine Register<T>() where T : IState
        {
            _states.Add(typeof(T), (T)Activator.CreateInstance(typeof(T)));
            return this;
        }

        public void NextState<T>() where T : IState
        {
            NextState(typeof(T));
        }
        
        public void NextState(Type type)
        {
            _currentState?.Exit();
            if(_states.TryGetValue(type, out _currentState))
                _currentState.Enter();
        }

        public virtual void Update(float deltaTime)
        {
            _currentState?.Update(deltaTime);
        }
    }
}