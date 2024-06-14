namespace Gameplay
{
    public abstract class RoundGameplayHandlerBase
    {
        private readonly IRoundGameplayEvents _gameplayEvents;

        protected RoundGameplayHandlerBase(IRoundGameplayEvents gameplayEvents)
        {
            _gameplayEvents = gameplayEvents;
        }

        public void Subscribe()
        {
            _gameplayEvents.OnEnterRoundEvent += BeginRoundHandler;
            _gameplayEvents.OnNextBlockEvent += NextBlockHandler;
            _gameplayEvents.OnDownBlockEvent += DownBlockHandler;
            _gameplayEvents.OnIntersectionAreaEvent += HasIntersectionHandler;
            _gameplayEvents.OnReplayEvent += ReplayRoundHandler;
            _gameplayEvents.OnExitRoundEvent += CompletedRoundHandler;
        }
        
        public void UnSubscribe()
        {
            _gameplayEvents.OnEnterRoundEvent -= BeginRoundHandler;
            _gameplayEvents.OnNextBlockEvent -= NextBlockHandler;
            _gameplayEvents.OnDownBlockEvent -= DownBlockHandler;
            _gameplayEvents.OnIntersectionAreaEvent -= HasIntersectionHandler;
            _gameplayEvents.OnReplayEvent -= ReplayRoundHandler;
            _gameplayEvents.OnExitRoundEvent -= CompletedRoundHandler;
        }
        protected abstract void BeginRoundHandler();
        protected abstract void DownBlockHandler();
        protected abstract void NextBlockHandler();
        protected abstract void HasIntersectionHandler(int area);
        protected abstract void ReplayRoundHandler(int stackNumber);
        protected abstract void CompletedRoundHandler();
    }
}