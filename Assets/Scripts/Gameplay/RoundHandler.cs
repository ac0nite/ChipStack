using System;
using CameraComponent;
using FSM;
using FSM.States;
using UnityEngine;

namespace Gameplay
{
    public class RoundHandler : RoundGameplayHandlerBase
    {
        private readonly IScoreModelSetter _scoreModel;
        private readonly StatesMachineModel _stateMachineModel;
        private readonly CurrencyManager _currencyManager;
        private readonly ICameraMover _cameraMover;
        private readonly Settings _settings;

        public RoundHandler(
            Settings settings,
            IRoundGameplayEvents gameplayEvents, 
            CurrencyManager currencyManager,
            StatesMachineModel statesMachineModel,
            ICameraMover cameraMover) : base(gameplayEvents)
        {
            _settings = settings;
            _currencyManager = currencyManager;
            _scoreModel = currencyManager.ScoreModelSetter;
            _stateMachineModel = statesMachineModel;
            _cameraMover = cameraMover;
        }

        protected override void BeginRoundHandler()
        {
            Debug.Log($"Begin round");
            
            _scoreModel.ScoreRound.Value = 0;
            _scoreModel.LevelRound.Value = 0;
        }

        protected override void DownBlockHandler()
        {
            
        }

        protected override void NextBlockHandler()
        {
            
        }

        protected override void HasIntersectionHandler(int area)
        {
            _scoreModel.LevelRound.Value++;
            _scoreModel.ScoreRound.Value += _currencyManager.CalculateScoreRound(area);
            MovingCameraUp();
        }

        protected override void ReplayRoundHandler(int stackNumber)
        {
            _scoreModel.LevelRound.Value -= stackNumber;
        }

        protected override void CompletedRoundHandler()
        {
            _stateMachineModel.ChangeState<ResultState>();
        }
        
        private void MovingCameraUp()
        {
            if(_scoreModel.LevelRound % _settings.MultiplicityBlocksToUpCamera == 0) 
                _cameraMover.Move(_settings.CameraUpPosition);
        }
        
        [Serializable]
        public struct Settings
        {
            [Range(1,5)] public int MultiplicityBlocksToUpCamera;
            public Vector3 CameraUpPosition;
        }
    }
}