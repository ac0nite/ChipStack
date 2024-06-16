using System;
using Core.UI.MVP;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Popups
{
    public class ModifyScreenPopup : ViewBase
    {
        [SerializeField] private Button _closeButton;
        public event Action OnCloseButtonPressedEvent;
        protected override void Subscribe()
        {
            _closeButton.onClick.AddListener(CloseButtonPressedHandler);
        }

        protected override void UnSubscribe()
        {
            _closeButton.onClick.RemoveAllListeners();
        }
        
        private void CloseButtonPressedHandler() => OnCloseButtonPressedEvent?.Invoke();
    }
}