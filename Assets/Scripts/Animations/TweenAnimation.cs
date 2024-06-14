using System;
using Components;
using DG.Tweening;
using UnityEngine;

namespace Animations
{
    public class TweenAnimation
    {
        private Sequence _sequence;
        private MoveObject _moveObject = null;

        public static TweenAnimation CreateMoveAnimation(Settings settings)
        {
            var tween = new TweenAnimation();
            tween.CreateSequence(tween.CreateMoveTween(settings));
            return tween;
        }

        public void Play(MoveObject moveObject, Action callback)
        {
            _moveObject = moveObject;
            _sequence?.OnComplete(() => callback?.Invoke()).Restart(false);
        }

        private Tweener CreateMoveTween(Settings settings) => DOVirtual.Float(0f, 1f, settings.Duration, TweenMoveUpdate).SetEase(settings.Ease);

        private void TweenMoveUpdate(float value)
        {
            _moveObject.Component.Position = Vector3.Lerp(_moveObject.Component.Position, _moveObject.EndPosition, value);
        }

        private void CreateSequence(params Tween[] tweens)
        {
            _sequence = DOTween.Sequence();
            foreach (var tween in tweens)
                _sequence.Append(tween);
            _sequence.SetAutoKill(false).SetRecyclable(true).Pause();
        }
        
        public class MoveObject
        {
            public MoveObject(IComponent component, Vector3 endPosition)
            {
                Component = component;
                EndPosition = endPosition;
            }
            public IComponent Component { get; }
            public Vector3 EndPosition { get; }
        }
        
        [Serializable]
        public struct Settings
        {
            public float Duration;
            public Ease Ease;
        }
    }
}