using System;
using Core.UI.MVP;
using SavingData;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = System.Numerics.Vector3;

namespace UI.Screens.Debug
{
    public class DebugScreen : ViewBase
    {
        [SerializeField] private Button _initialButton;
        [SerializeField] private Button _downButton;
        [SerializeField] private Button _clearButton;
        [SerializeField] private Slider _axisXSlider;
        [SerializeField] private Slider _axisZSlider;
        
        public event Action OnInitialButtonPressedEvent;
        public event Action OnDownButtonPressedEvent;
        public event Action OnClearButtonPressedEvent;
        
        public Vector3 DebugPosition => new Vector3(_axisXSlider.value, 0, _axisZSlider.value);
        protected override void Subscribe()
        {
            _axisXSlider.maxValue = 2.5f;
            _axisXSlider.minValue = -2.5f;
            _axisZSlider.maxValue = 2.5f;
            _axisZSlider.minValue = -2.5f;
            
            _axisXSlider.value = Saving.GetFloatValue("AxisXSliderValue");
            _axisZSlider.value = Saving.GetFloatValue("AxisZSliderValue");
            
            _axisXSlider.onValueChanged.AddListener((value) => Saving.SetFloatValue("AxisXSliderValue", value));
            _axisZSlider.onValueChanged.AddListener((value) => Saving.SetFloatValue("AxisZSliderValue", value));
            
            _initialButton.onClick.AddListener(() => OnInitialButtonPressedEvent?.Invoke());
            _downButton.onClick.AddListener(() => OnDownButtonPressedEvent?.Invoke());
            _clearButton.onClick.AddListener(() => OnClearButtonPressedEvent?.Invoke());
        }

        protected override void UnSubscribe()
        {
            _axisXSlider.onValueChanged.RemoveAllListeners();
            _axisZSlider.onValueChanged.RemoveAllListeners();

            _initialButton.onClick.RemoveAllListeners();
            _downButton.onClick.RemoveAllListeners();
            _clearButton.onClick.RemoveAllListeners();
        }
    }
}