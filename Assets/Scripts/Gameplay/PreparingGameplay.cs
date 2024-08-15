using System;
using Animations;
using Blocks;
using Components;
using DG.Tweening;
using UnityEngine;

namespace Gameplay
{
    public class PreparingGameplay
    {
        private readonly IBlockFacade _blockFacade;
        private readonly AnimationSettings _settings;
        private Block _block;
        private Sequence _sequence;

        public PreparingGameplay(AnimationSettings settings, IBlockFacade facade)
        {
            _settings = settings;
            _blockFacade = facade;
        }

        public void Run(Action callback)
        {
            _block = _blockFacade.BlockSpawn();
            
            _sequence ??= CreateSequence();
            _block.View.Animation.Play(_settings.InitialDrop.DownAnimation);
            _sequence.OnComplete(() =>
            {
                //_block.View.Component.EnablePhysics();
                _block.View.Animation.Play(_settings.InitialDrop.HitAnimation, () => 
                {
                    Debug.Log($"END!");
                    callback?.Invoke();
                });
                // callback?.Invoke();
            }).Restart();
        }
        
        private Sequence CreateSequence()
        {
            return DOTween
                .Sequence()
                .Append(DOVirtual.Vector3(_settings.InitialDrop.BeginPosition, Vector3.up * _blockFacade.BaseHeight, _settings.InitialDrop.Move.Duration, (p) => _block.Position = p).SetEase(_settings.InitialDrop.Move.Ease))
                //.Append(DOVirtual.Vector3(_block.Size, _block.Size * _settings.ElasticScale, _settings.ElasticDuration, (s) => _block.Size = new Vector3(s.x, (s.x > _block.Size.x)? _block.Size.y - (s.x - _block.Size.x) : _block.Size.y + (_block.Size.x - s.x), s.z)).SetEase(_settings.ElasticEase).SetLoops(2, LoopType.Yoyo))
                .SetAutoKill(false)
                .SetRecyclable(true);
        }
        
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
            public AnimationBase.Settings AnimationFlySettings;
            public AnimationBase.Settings AnimationSettings;
        }
    }
}