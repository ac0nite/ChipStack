using System;
using Core.FSM.Base;
using FSM.States;

namespace FSM
{
    public class GameplayStateMachine : StatesMachine
    {
        private readonly GameplayContext _context;

        private GameplayStateMachine(GameplayContext context)
        {
            _context = context;
        }

        public override StatesMachine Register<T>()
        {
            Register<T>((T)Activator.CreateInstance(typeof(T), _context));
            return this;
        }
        
        public static StatesMachine Create(GameplayContext context)
        {
            return new GameplayStateMachine(context)
                .Register<LoadingState>()
                .Register<ServiceState>()
                .Register<PreGameplayState>()
                .Register<GameplayState>()
                .Register<ResultState>()
                .Register<ResetState>()
                .Register<DebugState>();
        }
    }   
}