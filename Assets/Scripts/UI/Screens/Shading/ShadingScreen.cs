using System;
using Core.UI.MVP;
using DG.Tweening;
using Settings;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens.Shading
{
    public class ShadingScreen : ViewBase
    {
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private Button _shadingButton;
        private Settings _settings;
        private Color _showColor;
        private Color _hideColor;
        
        public event Action OnShadingTapPressedEvent; 
        protected override void Subscribe()
        {
            _settings = GameplaySettings.Instance.ScreensSettings.Shading;
            InitialiseColor();
            _shadingButton.onClick.AddListener(ShadingTapPressedHandler);
        }

        private void InitialiseColor()
        {
            _showColor = _hideColor = _settings.BackgroundColor;
            _hideColor.a = 0;
            _backgroundImage.color = _hideColor;
        }

        protected override void UnSubscribe()
        {
            _shadingButton.onClick.RemoveAllListeners();
        }
        private void ShadingTapPressedHandler() => OnShadingTapPressedEvent?.Invoke();
        protected override void PlayShowAnimation(Action callback)
        {
            PLayAnimation(true).OnComplete(() => callback?.Invoke());
        }
        protected override void PlayHideAnimation(Action callback)
        {
            PLayAnimation(false).OnComplete(() => callback?.Invoke());
        }

        private Tweener PLayAnimation(bool isShow)
        {
            return _backgroundImage
                .DOColor(isShow ? _showColor : _hideColor, _settings.FadeDuration)
                .SetEase(_settings.FadeEase);
        }
        
        [Serializable]
        public struct Settings
        {
            public float FadeDuration;
            public Ease FadeEase;
            public Color BackgroundColor;
        }
    }
}