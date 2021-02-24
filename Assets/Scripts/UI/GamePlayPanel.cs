using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayPanel : UIBasePanel
{
    [SerializeField] private Text _scoreText = null;
    [SerializeField] private ChangeScoreTxtUI _changeScore = null;
    void Update()
    {
        _scoreText.text = GameManager.Instance.ScoreManager.Score.ToString();
        _changeScore.ChangeParam(GameManager.Instance.ScoreManager.Total);
    }

    public void TapToResetButton()
    {
        Debug.Log("Reset()");
        GameManager.Instance.GameController.Reset();
    }
}
