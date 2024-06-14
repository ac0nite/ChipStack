using Core.UI.MVP;

namespace UI.Screens.Gameplay
{
    public class GameplayScreenPresenter : ScreenPresenterBase<GameplayScreen>
    {
        private readonly IScoreModelGetter _scoreModel;

        public GameplayScreenPresenter(GameplayScreen view, IScoreModelGetter scoreModel) : base(view)
        {
            _scoreModel = scoreModel;
            _scoreModel.LevelRound.OnValueChangedEvent += LevelRoundChangedHandler;
            _scoreModel.ScoreRound.OnValueChangedEvent += ScoreRoundChangedHandler;
        }
        protected override void Dispose()
        {
            _scoreModel.LevelRound.OnValueChangedEvent -= LevelRoundChangedHandler;
            _scoreModel.ScoreRound.OnValueChangedEvent -= ScoreRoundChangedHandler;
        }

        private void ScoreRoundChangedHandler()
        {
            _view.ChangeScoreRound(_scoreModel.ScoreRound);
        }

        private void LevelRoundChangedHandler()
        {
            _view.ChangeLevelRound(_scoreModel.LevelRound);
        }
    }
}