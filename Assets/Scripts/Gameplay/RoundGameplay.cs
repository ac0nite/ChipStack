using System;
using Animations;
using Blocks;
using Components;
using Intersections;
using MEC;
using UnityEngine;

namespace Gameplay
{
    public class RoundGameplay : IRoundGameplayEvents
    {
        private readonly IBlockFacade _blockFacade;
        private readonly Movement _movement;
        private readonly InputManager _input;
        private readonly BlocksIntersection _intersection;
        private readonly Movement.Settings _defaultSettings;
        private readonly TweenAnimation _animation;
        
        public event Action OnEnterRoundEvent;
        public event Action OnNextBlockEvent;
        public event Action OnDownBlockEvent;
        public event Action<int> OnIntersectionAreaEvent;
        public event Action<int> OnReplayEvent;
        public event Action OnExitRoundEvent;

        public RoundGameplay(IBlockFacade blockFacade, Movement.Settings defaultSettings)
        {
            _blockFacade = blockFacade;
            _defaultSettings = defaultSettings;
            _movement = new Movement(CreateCenterComponent());
            _input = InputManager.Instance;
            _intersection = _blockFacade.Intersection;
            _animation = TweenAnimation.CreateMoveAnimation(defaultSettings.MoveAnimation);
        }

        public void Run()
        {
            OnEnterRoundEvent?.Invoke();
            _input.TapEvent += CheckIntersection;
            MoveNextBlock();
        }

        public void Stop()
        {
            _input.TapEvent -= CheckIntersection;
            _movement.Stop();
        }

        private void MoveNextBlock()
        {
            var previewBlock = _blockFacade.LastBlockSpawned;
            var movableBlock = _blockFacade.BlockSpawn();
            movableBlock.Size = previewBlock.Size;

            _movement.Play(movableBlock, previewBlock, _defaultSettings);
            Timing.CallDelayed(1f, _input.UnLocked);
            OnNextBlockEvent?.Invoke();
        }
        
        private void CheckIntersection()
        {
            OnDownBlockEvent?.Invoke();
            _movement.Stop();
            _input.Locked();
            
            var lastBlock = _blockFacade.LastBlockSpawned;

            if (_intersection.HasIntersect)
            {
                _animation.Play(CreateAnimationParam(), () => 
                {
                    var intersection = _intersection.AreaOfIntersection;
                    var remainderIntersection = _intersection.AreaOfRemaindersIntersection;
                    _blockFacade.LastBlockSpawned.ChangeTransform(intersection);
                    _blockFacade.RemainderSpawn().Initialise(remainderIntersection).Enable();
                    OnIntersectionAreaEvent?.Invoke(intersection.Area);
                    MoveNextBlock();
                });
            }
            else
            {
                Debug.LogWarning($"Block has no intersection", lastBlock.View);
                lastBlock.View.Component.EnablePhysics();
                OnExitRoundEvent?.Invoke();
            }
        }
        private TweenAnimation.MoveObject CreateAnimationParam() =>
            new (_blockFacade.LastBlockSpawned, new Vector3(_blockFacade.LastBlockSpawned.Position.x, _blockFacade.CenterPosition.y, _blockFacade.LastBlockSpawned.Position.z));
        
        private IComponent CreateCenterComponent()
        {
            return new GameObject("Center").AddComponent<BaseComponent>();
        }
    }
}