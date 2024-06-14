using System;
using UnityEngine;

namespace Core.UI.MVP
{
    public abstract class ViewBase : MonoBehaviour, IView
    {
        protected Canvas _canvas;
        public virtual bool IsVisible => _canvas.enabled;
        public event Action OnDisposeEvent;

        protected virtual void Awake()
        {
            _canvas = GetComponent<Canvas>();
            _canvas.enabled = false;
            Subscribe();
        }
        protected abstract void Subscribe();
        protected abstract void UnSubscribe();
        public virtual void Show(bool withAnimation = true)
        {
            _canvas.enabled = true;
            if (withAnimation)
                PlayShowAnimation(null);
        }
        public virtual void Hide(bool withAnimation = true)
        {
            if(withAnimation) 
                PlayHideAnimation(() => _canvas.enabled = false);
            else
                _canvas.enabled = false;
        }

        public void ChangeOrder(int order)
        {
            _canvas.sortingOrder = order;
        }
        protected virtual void PlayShowAnimation(Action callback)
        {
            callback?.Invoke();
        }
        protected virtual void PlayHideAnimation(Action callback)
        {
            callback?.Invoke();
        }

        protected virtual void OnDestroy()
        {
            UnSubscribe();
            OnDisposeEvent?.Invoke();
        }
    }
}