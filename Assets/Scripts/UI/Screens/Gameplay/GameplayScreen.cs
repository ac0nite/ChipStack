using Core.UI.MVP;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens.Gameplay
{
    public class GameplayScreen : ViewBase
    {
        [SerializeField] private Text _stackLevelText;
        [SerializeField] private Text _scoreText;

        protected override void Subscribe()
        { }

        protected override void UnSubscribe()
        { }
        public void ChangeLevelRound(int value)
        {
            _stackLevelText.text = value.ToString();
        }
        
        public void ChangeScoreRound(int value)
        {
            _scoreText.text = value.ToString();
        }
    }
}