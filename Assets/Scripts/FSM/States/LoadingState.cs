using UI;
using UI.Screens.Loading;

namespace FSM.States
{
    public class LoadingState : GameplayBaseState
    {
        public LoadingState(GameplayContext context) : base(context)
        {
        }

        public override void Enter()
        {
            ScreenManager.Show<LoadingScreen>();
            _context.StatesMachineModel.ChangeState<ServiceState>();
        }
    }
}