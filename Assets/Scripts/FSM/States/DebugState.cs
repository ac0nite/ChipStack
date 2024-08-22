using UI;
using UI.Screens.Debug;

namespace FSM.States
{
    public class DebugState : GameplayBaseState
    {
        public DebugState(GameplayContext context) : base(context)
        {
        }
        
        public override void Enter()
        {
            ScreenManager.Show<DebugScreen>();
        }
    }
}