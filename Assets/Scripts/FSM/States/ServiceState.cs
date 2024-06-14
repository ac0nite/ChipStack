using MEC;
using UI;
using UI.Screens.Loading;

namespace FSM.States
{
    public class ServiceState : GameplayBaseState
    {
        public ServiceState(GameplayContext context) : base(context)
        {
        }
        public override void Enter()
        {
            //TODO: add service
            Timing.CallDelayed(0.1f, () =>
            {
                ScreenManager.Hide<LoadingScreen>();
                _context.StatesMachineModel.ChangeState<PreGameplayState>();
            });
        }
    }   
}