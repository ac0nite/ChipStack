using System;
using System.Collections.Generic;
using Components;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;

namespace Animations
{
    public class TweenAnimation
    {
        private Sequence sequence;
        public AnimationParam MoveParam { get; set; } = new(component => component.Position);
        public AnimationParam SizeParam { get; set; } = new(component => component.Size);

        public static TweenAnimation CreateMoveDownAnimation(Settings settings)
        {
            var tween = new TweenAnimation();
            tween.AppendSequence(tween.CreateMoveTween(settings));
            return tween;
        }

        public static TweenAnimation CreateMoveRemainderAnimation(Settings move, Settings elastic)
        {
            var tween = new TweenAnimation();
            tween.AppendSequence(tween.CreateMoveTween(move).OnComplete(tween.MoveParam.NextParam));
            tween.JoinSequence(tween.CreateSizeElasticTween(elastic));
            tween.AppendSequence(tween.CreateMoveTween(move));
            return tween;
        }

        public void Play(Action callback)
        {
            sequence
                ?.OnComplete(() => callback?.Invoke())
                .Restart(false);
        }

        private Tweener CreateMoveTween(Settings settings) => DOVirtual
            .Float(0f, 1f, settings.Duration, TweenMoveUpdate)
            .SetEase(settings.Ease);
        
        private Tweener CreateSizeElasticTween(Settings settings) => DOVirtual
            .Float(0f, 1f, settings.Duration, TweenSizeElasticUpdate)
            .SetEase(settings.Ease)
            .SetLoops(2, LoopType.Yoyo);

        private void TweenMoveUpdate(float value)
        {
            MoveParam.Component.Position = Vector3.Lerp(MoveParam.From, MoveParam.To, value);
        }
        
        private void TweenSizeElasticUpdate(float value)
        {
            SizeParam.Component.Size = Vector3.Lerp(SizeParam.From, SizeParam.To, value);
        }

        private void UpdateTarget()
        {
            MoveParam.NextParam();
        }

        private void AppendSequence(params Tween[] tweens)
        {
            sequence ??= DOTween.Sequence();
            
            foreach (var tween in tweens)
                sequence.Append(tween);
            
            sequence
                .SetAutoKill(false)
                .SetRecyclable(true)
                .Pause();
        }
        
        private void JoinSequence(params Tween[] tweens)
        {
            sequence ??= DOTween.Sequence();
            
            foreach (var tween in tweens)
                sequence.Join(tween);
            
            sequence
                .SetAutoKill(false)
                .SetRecyclable(true)
                .Pause();
        }
        

        public class AnimationParam
        {
            private int index;
            private List<Vector3> available = new(3);

            public delegate Vector3 GetValueDelegate(IComponent component);

            private GetValueDelegate GetValue;
            public AnimationParam(GetValueDelegate getValue)
            {
                GetValue = getValue;
            }
            public void UpdateParams(IComponent component, params Vector3[] data)
            {
                Component = component;
//                Debug.Log(Component.Position);
                
                available.Clear();
                available.AddRange(data);
                
                index = 0;
                From = GetValue(Component);
                To = available[index];
            }

            public void NextParam()
            {
                index++;
                From = GetValue(Component);
                To = available[index];
                Debug.Log($"Next param: {From} => {To}");
            }
            public IComponent Component { get; private set; }
            public Vector3 From { get; private set; }
            public Vector3 To { get; private set; }
        }
        
        [Serializable]
        public struct Settings
        {
            public float Duration;
            public Ease Ease;
            public float Delay;
        }
    }
}