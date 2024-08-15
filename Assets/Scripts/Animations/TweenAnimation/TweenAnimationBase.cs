using System;
using Components;
using DG.Tweening;
using UnityEngine;

namespace Animations
{
    public class TweenAnimationBase
    {
        protected Sequence sequence;
        public TweenComponent Move { get; protected set; } = null;
        public TweenComponent Size { get; protected set; } = null;
            

        protected void AppendSequence(params Tween[] tweens)
        {
            sequence ??= DOTween.Sequence();
            
            foreach (var tween in tweens)
                sequence.Append(tween);
            
            sequence
                .SetAutoKill(false)
                .SetRecyclable(true)
                .Pause();
        }
        
        protected void JoinSequence(params Tween[] tweens)
        {
            sequence ??= DOTween.Sequence();
            
            foreach (var tween in tweens)
                sequence.Join(tween);
            
            sequence
                .SetAutoKill(false)
                .SetRecyclable(true)
                .Pause();
        }
        
        public void Play(Action callback)
        {
            sequence
                ?.OnComplete(() =>
                {
                    Debug.Log($"Tween completed!");
                    callback?.Invoke();
                })
                .Restart(false);
        }

        public void SetComponent(IComponent component)
        {
            Move?.AnimComponent.SetComponent(component);
            Size?.AnimComponent.SetComponent(component);
        }
    }
}