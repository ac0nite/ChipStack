using Core.UI.MVP;
using FSM;
using FSM.States;

namespace UI.Screens.Result
{
    public class ResultScreenPresenter : ScreenPresenterBase<ResultScreen>
    {
        private readonly IScoreModelGetter _scoreModel;
        private readonly StatesMachineModel _statesMachine;

        public ResultScreenPresenter(ResultScreen view, IScoreModelGetter scoreModel, StatesMachineModel statesMachine) : base(view)
        {
            _scoreModel = scoreModel;
            _statesMachine = statesMachine;
            
            _scoreModel.ScoreRound.OnValueChangedEvent += UpdateScoreRoundHandler;
            _scoreModel.LevelRound.OnValueChangedEvent += UpdateLevelRoundHandler;
            _scoreModel.TotalScoreRound.OnValueChangedEvent += UpdateTotalScoreRoundHandler;
            _scoreModel.TotalScore.OnValueChangedEvent += UpdateTotalScoreHandler;
            
            _view.OnNextButtonPressedEvent += NextStateHandler;
        }

        protected override void Dispose()
        {
            _scoreModel.ScoreRound.OnValueChangedEvent -= UpdateScoreRoundHandler;
            _scoreModel.LevelRound.OnValueChangedEvent -= UpdateLevelRoundHandler;
            _scoreModel.TotalScoreRound.OnValueChangedEvent -= UpdateTotalScoreRoundHandler;
            _scoreModel.TotalScore.OnValueChangedEvent -= UpdateTotalScoreHandler;
            
            _view.OnNextButtonPressedEvent -= NextStateHandler;
        }

        private void NextStateHandler()
        {
            _statesMachine.ChangeState<ResetState>();
        }

        private void UpdateTotalScoreHandler()
        {
            _view.SetTotalScoreValue(_scoreModel.TotalScore);
        }

        private void UpdateTotalScoreRoundHandler()
        {
            _view.SetTotalScoreRoundValue(_scoreModel.TotalScoreRound);
        }

        private void UpdateLevelRoundHandler()
        {
            _view.SetLeveRoundValue(_scoreModel.LevelRound);
        }

        private void UpdateScoreRoundHandler()
        {
            _view.SetScoreRoundValue(_scoreModel.ScoreRound);
        }
    }
}