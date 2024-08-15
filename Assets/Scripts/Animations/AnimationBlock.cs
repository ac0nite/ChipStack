using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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
        protected const int LayerIndex = 0;
        protected readonly Animator _animator;

        public AnimationBase(Animator animator)
        {
            _animator = animator;
        }
        
        public virtual void Play(int id, Settings settings, Action callback = null)
        {
            _animator.DelayAsync(settings.Delay).ContinueWith(animator =>
            {
                animator.SetTrigger(id);
                animator.SetFloat(AnimationsConstants.Speed, settings.Speed);
                _ = animator.OnCompletedAsync(callback);
            });
        }

        #region Settings

        [Serializable]
        public class Settings
        {
            public float Delay = 0;
            public float Speed = 1;
        }

        #endregion
    }
    
    public static class AnimationsConstants
    {
        public static readonly int Idle = Animator.StringToHash(nameof(Idle));
        public static readonly int FlyLanding = Animator.StringToHash(nameof(FlyLanding));
        public static readonly int Landing = Animator.StringToHash(nameof(Landing));
        public static readonly int Speed = Animator.StringToHash(nameof(Speed));
        
        public static readonly IEnumerable<string> DropDowns = new[]
        {
            nameof(FlyLanding), 
            nameof(Landing)
        };
    }
    
    // [Serializable]
    // public class KeyCodeGameObjectListDictionary : UnitySerializedDictionary<DropDowns, AnimationBase.Settings> { }
    
    public static class AnimatorExtension
    {
        public static async UniTask<Animator> DelayAsync(this Animator animator, float delay)
        {
            await UniTask.WaitForSeconds(delay);
            return animator;
        }
        
        public static async UniTask OnCompletedAsync(this Animator animator, Action callback, int layerIndex = 0)
        {
            var length = await animator.ClipLengthAsync(layerIndex);
            await UniTask.WaitForSeconds(length);
            callback?.Invoke();
        }
        
        public static async UniTask<float> ClipLengthAsync(this Animator animator, int layerIndex = 0)
        {
            await UniTask.Yield();
            return animator.GetCurrentAnimatorStateInfo(layerIndex).length;
        }
    }
    
    public abstract class UnitySerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField, HideInInspector]
        private List<TKey> keyData = new List<TKey>();
	
        [SerializeField, HideInInspector]
        private List<TValue> valueData = new List<TValue>();

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            this.Clear();
            for (int i = 0; i < this.keyData.Count && i < this.valueData.Count; i++)
            {
                this[this.keyData[i]] = this.valueData[i];
            }
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            this.keyData.Clear();
            this.valueData.Clear();

            foreach (var item in this)
            {
                this.keyData.Add(item.Key);
                this.valueData.Add(item.Value);
            }
        }
    }
}