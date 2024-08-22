using System;
using System.Collections.Generic;
using Animations;
using Blocks;
using Blocks.Movements;
using Components;
using MEC;
using UnityEngine;

namespace Gameplay
{
    public class Movement
    {
        private readonly CenterMovement _centerMoving;
        private readonly LineMovement _lineMoving;
        private readonly CircleMovement _circleMoving;
        private CoroutineHandle _coroutineHandler;
        private readonly float _wait;

        public Movement(IComponent centerPoint)
        {
            _centerMoving = new CenterMovement(centerPoint);
            _lineMoving = new LineMovement();
            _circleMoving = new CircleMovement(centerPoint);
            _wait = Timing.WaitForOneFrame;
        }

        public void Play(Block movable, Block preview, Settings settings)
        {
            _centerMoving.UpdateSettings(settings.Center, preview);

            _circleMoving.UpdateSettings(settings.Circle);
            _circleMoving.AssignMoving(movable);
            
            _lineMoving.UpdateSettings(settings.Line);
            _lineMoving.AssignMoving(movable, _circleMoving);

            if (Timing.IsAliveAndPaused(_coroutineHandler))
                Timing.ResumeCoroutines(_coroutineHandler);
            else
                _coroutineHandler = Timing.RunCoroutine(Update());
        }

        public void Stop()
        {
            if (Timing.IsRunning(_coroutineHandler))
                Timing.PauseCoroutines(_coroutineHandler);
        }

        private IEnumerator<float> Update()
        {
            while (true)
            {
                _centerMoving.UpdatePosition(Time.deltaTime);
                _circleMoving.UpdatePosition(Time.deltaTime);
                _lineMoving.UpdatePosition(Time.deltaTime);

                _centerMoving.ApplyPosition();

                if (!_lineMoving.IsCompleted)
                {
                    // _lineMoving.UpdatePosition(Time.deltaTime);
                    _lineMoving.ApplyPosition();
                }
                else
                {
                    // _circleMoving.UpdatePosition(Time.deltaTime);
                    _circleMoving.ApplyPosition();
                }

                yield return _wait;
            }
        }

        [Serializable]
        public struct Settings
        {
            public CenterMovement.Settings Center;
            public LineMovement.Settings Line;
            public CircleMovement.Settings Circle;
        }
    }
}