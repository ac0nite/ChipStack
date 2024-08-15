using System;
using System.Numerics;
using Animations;
using Blocks;
using Components;
using DG.Tweening;
using Intersections;
using MEC;
using TMPro;
using UnityEngine;
using RectTransform = Intersections.RectTransform;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Gameplay
{
    public class RoundGameplay : IRoundGameplayEvents
    {
        private readonly IBlockFacade _blockFacade;
        private readonly Movement _movement;
        private readonly InputManager _input;
        private readonly BlocksIntersection _intersection;
        private readonly Movement.Settings _defaultSettings;
        private readonly TweenComponent _blockComponent;
        private readonly TweenComponent _remainderComponent;
        private Block _movableBlock;
        private Remainder _remainder;

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
            
            // _blockComponent = TweenComponent.CreateMoveDownAnimation(defaultSettings.DownBlockMoveAnimation);
            // _remainderComponent = TweenComponent.CreateMoveRemainderAnimation(defaultSettings.RemainderMoveAnimation, defaultSettings.RemainderElasticAnimation);
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
            _movableBlock = _blockFacade.BlockSpawn();
            _movableBlock.Size = previewBlock.Size;

            _movement.Play(_movableBlock, previewBlock, _defaultSettings);
            Timing.CallDelayed(1f, _input.UnLocked);
            OnNextBlockEvent?.Invoke();
        }
        
        private void CheckIntersection()
        {
            OnDownBlockEvent?.Invoke();
            _movement.Stop();
            _input.Locked();

            if (_intersection.HasIntersect)
            {
                UpdateDownAnimationParam(_intersection.Offset);

                // _blockComponent.Play(() =>
                // {
                //     var v1 = _movableBlock.Position;
                //     
                //     var generalRectTransform = _intersection.GeneralRect.ToRectTransform(_movableBlock.Position.y, _movableBlock.Size.y);
                //     var remaindersRectTransform = _intersection.RemaindersRect.ToRectTransform(_movableBlock.Position.y, _movableBlock.Size.y);
                //     
                //     _movableBlock.ChangeTransform(generalRectTransform);
                //
                //     var v0 = _movableBlock.Position;
                //     _remainder = _blockFacade.RemainderSpawn().Initialise(remaindersRectTransform);
                //     _remainder.Enable();
                //     UpdateRemainderAnimationParam(remaindersRectTransform, v1 - v0);
                //     _remainderComponent.Play(null);
                //     OnIntersectionAreaEvent?.Invoke(generalRectTransform.Area);
                //     MoveNextBlock();
                // });
            }
            else
            {
                Debug.LogWarning($"Block has no intersection", _movableBlock.View);
                //_movableBlock.View.Component.EnablePhysics();
                OnExitRoundEvent?.Invoke();
            }
        }

        private void UpdateDownAnimationParam(Vector2 offset)
        {
            // if (offset != Vector2.zero)
            // {
            //     Debug.Log($"offset: {offset}");
            //     UnityEditor.EditorApplication.isPaused = true;
            // }
            
            // _blockComponent.MoveComponent.UpdateParams(
            //     _movableBlock,
            //     new Vector3(_movableBlock.Position.x + offset.x, _blockFacade.BaseHeight, _movableBlock.Position.z + offset.y));
        }

        private void UpdateRemainderAnimationParam((RectTransform one, RectTransform two) remainders, Vector3 direction)
        {
            var remOneIsValid = remainders.one.IsValid;
            var remTwoIsValid = remainders.two.IsValid;

            if (remOneIsValid && !remTwoIsValid)
                direction.z = 0;
            else if(!remOneIsValid && remTwoIsValid)
                direction.x = 0;

            var move1 = _remainder.Position + direction.normalized * 2f;
            var move2 = new Vector3(move1.x, -10, move1.z);
            var elastic = new Vector3(_remainder.Size.x * 1.1f, _remainder.Size.y, _remainder.Size.z * 1.1f);
            // _remainderComponent.MoveComponent.UpdateParams(_remainder, move1, move2);
            // _remainderComponent.SizeParam.UpdateParams(_remainder, elastic);
        }

        private IComponent CreateCenterComponent() => new GameObject("Center").AddComponent<BaseComponent>();
    }
}