using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Animations
{
    public class AnimationBlock : AnimationBase
    {
        public AnimationBlock(Animator animator) : base(animator)
        {
        }
    }

    public class AnimationBase
    {
        private const int LayerIndex = 0;
        protected readonly Animator _animator;

        public AnimationBase(Animator animator)
        {
            _animator = animator;
        }
        
        public void Play(int id, Settings settings, Action callback = null)
        {
            _animator.DelayAsync(settings.Delay).ContinueWith(animator =>
            {
                animator.SetTrigger(id);
                //animator.SetFloat(AnimationsConstants.Speed, settings.Speed);
                _ = animator.SetFloatWithCurve(AnimationsConstants.Speed, settings.Speed, settings.Curve, callback);
                //_ = animator.OnCompletedAsync(callback);
                //animator.ClipLength().ContinueWith(l => Debug.Log($"Length: {l}"));
            });
        }

        #region Settings

        [Serializable]
        public class Settings
        {
            public float Delay = 0;
            public float Speed = 1;
            public AnimationCurve Curve = AnimationCurve.Linear(0, 0, 1, 1);
            public TweenParams Params = TweenParams.Params;
        }

        #endregion
    }
    
    public static class AnimationsConstants
    {
        public static readonly int Idle = Animator.StringToHash("Idle");
        public static readonly int FlyLanding = Animator.StringToHash("FlyLanding");
        public static readonly int Landing = Animator.StringToHash("Landing");
        public static readonly int Speed = Animator.StringToHash(nameof(Speed));
    }
    
    public static class AnimatorExtension
    {
        public static async UniTask<Animator> DelayAsync(this Animator animator, float delay)
        {
            await UniTask.WaitForSeconds(delay);
            return animator;
        }
        
        public static async UniTask OnCompletedAsync(this Animator animator, Action callback, int layerIndex = 0)
        {
            var length = await animator.ClipLength();
            DOVirtual.DelayedCall(length, () => callback?.Invoke());
        }
        
        private static async UniTask<float> ClipLength(this Animator animator, int layerIndex = 0)
        {
            await UniTask.Yield();
            return animator.GetCurrentAnimatorStateInfo(layerIndex).length;
        }
        
        public static async UniTask SetFloatWithCurve(this Animator animator, int id, float speed, AnimationCurve curve, Action callback)
        {
            animator.SetFloat(id, speed);
            var length = await animator.ClipLength(); 
            DOVirtual.Float(0, 1, length, value =>
            {
                animator.SetFloat(id, value * speed);
            })
            .SetEase(curve)
            .OnComplete(Completed);

            void Completed()
            {
                animator.SetFloat(id, 1f * speed);
                callback?.Invoke();
            }
        }
    }
}