using System;
using Core.UI.MVP;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens.PreGameplay
{
    public class PreGameplayScreen : ViewBase
    {
        [SerializeField] private Button _startButton;

        public event Action StartButtonPressedEvent;
        
        protected override void Subscribe()
        {
            _startButton.onClick.AddListener(StartButtonPressedHandler);
        }

        protected override void UnSubscribe()
        {
            _startButton.onClick.RemoveAllListeners();
        }

        private void StartButtonPressedHandler() => StartButtonPressedEvent?.Invoke();
    }
}