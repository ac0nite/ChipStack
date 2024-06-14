using CameraComponent;
using UI;
using UI.Screens.Result;

namespace FSM.States
{
    public class ResultState : GameplayBaseState
    {
        private readonly ICameraMover _cameraMover;
        private readonly CurrencyManager _currency;

        public ResultState(GameplayContext context) : base(context)
        {
            _cameraMover = _context.CameraMover;
            _currency = _context.Currency;
        }

        public override void Enter()
        {
            _cameraMover.Reset(ShowRoundResult);
        }
        
        public override void Exit()
        {
            ScreenManager.Hide<ResultScreen>();
        }

        private void ShowRoundResult()
        {
            _currency.CalculateAndApplyResultScore();
            ScreenManager.Show<ResultScreen>();   
        }
    }   
}