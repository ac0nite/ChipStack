using System;
using Animations;
using Blocks;
using DG.Tweening;
using UnityEngine;

namespace Gameplay
{
    public class PreparingGameplay
    {
        private readonly IBlockFacade _blockFacade;
        private Block _block;
        // private Sequence _sequence;
        private TweenAnimation _animation;
        private readonly TweenAnimationSettings _settings;

        public PreparingGameplay(TweenAnimationSettings settings, IBlockFacade facade)
        {
            _settings = settings;
            _blockFacade = facade;
        }

        public void Run(Action callback)
        {
            _block = _blockFacade.BlockSpawn();
            
            _animation ??= TweenAnimation.CreateInitialDrop(_settings.InitialDrop);
            
            _animation.SetComponent(_block);
            _animation.Move.AnimComponent.UpdateParams(_settings.InitialDrop.BeginPosition, Vector3.up * _blockFacade.BaseHeight);
            _animation.Size.AnimComponent.UpdateScaledParams(_settings.InitialDrop.ScaleSize);
            _animation.Play(() =>
            {
                _block.View.Component.EnablePhysics();
                callback?.Invoke();
            });
            // _sequence ??= CreateSequence();
            // _sequence.OnComplete(() =>
            // {
            //     _block.View.Component.EnablePhysics();
            //     callback?.Invoke();
            // }).Restart();
        }

        // private void PlayAnimation(Action callback)
        // {
        //     _animation ??= TweenAnimation.CreateInitialDrop(_settings.InitialDrop);
        // }
        // private Sequence CreateSequence()
        // {
        //     return DOTween
        //         .Sequence()
        //         .Append(DOVirtual.Vector3(_settings.StartPosition, Vector3.up * _blockFacade.BaseHeight, _settings.MoveDuration, (p) => _block.Position = p).SetEase(_settings.MoveEase))
        //         .Append(DOVirtual.Vector3(_block.Size, _block.Size * _settings.ElasticScale, _settings.ElasticDuration, (s) => _block.Size = new Vector3(s.x, (s.x > _block.Size.x)? _block.Size.y - (s.x - _block.Size.x) : _block.Size.y + (_block.Size.x - s.x), s.z)).SetEase(_settings.ElasticEase).SetLoops(2, LoopType.Yoyo))
        //         .SetAutoKill(false)
        //         .SetRecyclable(true);
        // }
        
        [Serializable]
        public struct Settings
        {
            [Header("MOVE")]
            public Vector3 StartPosition;
            public float MoveDuration;
            public Ease MoveEase;
            
            [Header("ELASTIC")]
            public float ElasticScale;
            public float ElasticDuration;
            public Ease ElasticEase;
        }
    }
}