using CameraComponent;
using Gameplay;
using UI;
using UI.Popups;
using UI.Screens.Gameplay;

namespace FSM.States
{
    public class GameplayState : GameplayBaseState
    {
        private readonly RoundGameplay _roundGameplay;
        private readonly RoundHandler _roundHandler;
        private readonly ICameraMover _cameraMover;

        public GameplayState(GameplayContext context) : base(context)
        {
            _cameraMover = _context.CameraMover;
            _roundGameplay = new RoundGameplay(_context.BlockFacade, _settings.MovementSettings);
            _roundHandler = new RoundHandler(_settings.RoundSettings, _roundGameplay, _context.Currency, _context.StatesMachineModel, _context.CameraMover);
        }

        public override void Enter()
        {
            ScreenManager.Show<GameplayScreen>();
            ScreenManager.Show<ModifyScreenPopup>();
            
            _cameraMover.Rotate();
            _roundHandler.Subscribe();
            _roundGameplay.Run();
        }
        
        public override void Exit()
        {
            ScreenManager.Hide<GameplayScreen>();
            
            _roundGameplay.Stop();
            _roundHandler.UnSubscribe();
        }
    }
}