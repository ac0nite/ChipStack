using System;
using Core.UI.MVP;
using MEC;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens.Result
{
    public class ResultScreen : ViewBase
    {
        [SerializeField] private Button _nextButton;
        [SerializeField] private Text _scoreRoundValueText;
        [SerializeField] private Text _levelRoundValueText;
        [SerializeField] private Text _totalScoreRoundValueText;
        [SerializeField] private Text _totalScoreValueText;

        public event Action OnNextButtonPressedEvent;

        protected override void Subscribe()
        {
            _nextButton.onClick.AddListener(NextButtonHandler);
        }

        protected override void UnSubscribe()
        {
            _nextButton.onClick.RemoveAllListeners();
        }

        public void SetScoreRoundValue(int value) => _scoreRoundValueText.text = value.ToString();
        public void SetLeveRoundValue(int value) => _levelRoundValueText.text = value.ToString();
        public void SetTotalScoreRoundValue(int value) => _totalScoreRoundValueText.text = value.ToString();
        public void SetTotalScoreValue(int value) => _totalScoreValueText.text = value.ToString();
        
        public void Show()
        {
            base.Show();
        }

        private void NextButtonHandler() => OnNextButtonPressedEvent?.Invoke();
    }
}