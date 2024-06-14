using System;
using Core.UI.MVP;
using DG.Tweening;
using Settings;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens.Loading
{
    public class LoadingScreen : ViewBase
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Text[] _names;
        private Settings _settings;

        protected override void Subscribe()
        {
            _settings = GameplaySettings.Instance.ScreensSettings.LoadingScreen;
        }

        protected override void UnSubscribe()
        {
            
        }

        protected override void PlayShowAnimation(Action callback)
        {
            _canvasGroup.alpha = 1;
            foreach (var symbol in _names)
            {
                symbol.transform
                    .DOShakePosition(_settings.ShowFadeDuration, Vector3.up * 8, 3)
                    .SetLoops(-1, LoopType.Yoyo);
            }
            
            callback?.Invoke();
        }

        protected override void PlayHideAnimation(Action callback)
        {
            foreach (var symbol in _names)
                symbol.transform.DOKill();

            _canvasGroup
                .DOFade(0, _settings.HideFadeDuration)
                .SetEase(_settings.HideEaseType)
                .OnComplete(() => callback?.Invoke());
        }
        
        [Serializable]
        public class Settings
        {
            public float ShowFadeDuration;
            public float HideFadeDuration;
            public Ease HideEaseType;
        }
    }
}

