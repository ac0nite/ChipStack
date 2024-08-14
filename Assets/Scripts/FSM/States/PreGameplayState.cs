using Gameplay;
using UI;
using UI.Screens.PreGameplay;

namespace FSM.States
{
    public class PreGameplayState : GameplayBaseState
    {
        private readonly PreparingGameplay _preparingGameplay;

        public PreGameplayState(GameplayContext context) : base(context)
        {
            _preparingGameplay = new PreparingGameplay(_settings.AnimationSettings, _context.BlockFacade);
        }

        public override void Enter()
        {
            _preparingGameplay.Run(() =>
            {
                ScreenManager.Show<PreGameplayScreen>();
            });
        }

        public override void Exit()
        {
            ScreenManager.Hide<PreGameplayScreen>();
        }
    }
}