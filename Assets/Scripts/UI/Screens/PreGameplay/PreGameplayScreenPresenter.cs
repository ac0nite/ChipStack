using Core.UI.MVP;
using FSM;
using FSM.States;

namespace UI.Screens.PreGameplay
{
    public class PreGameplayScreenPresenter : ScreenPresenterBase<PreGameplayScreen>
    {
        private readonly StatesMachineModel _stateMachine;

        public PreGameplayScreenPresenter(PreGameplayScreen view, StatesMachineModel stateMachine) : base(view)
        {
            _stateMachine = stateMachine;
            _view.StartButtonPressedEvent += StartGameplayHandler;
        }

        private void StartGameplayHandler()
        {
            _stateMachine.ChangeState<GameplayState>();
        }

        protected override void Dispose()
        {
            _view.StartButtonPressedEvent -= StartGameplayHandler;
        }
    }
}