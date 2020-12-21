using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayPanel : UIBasePanel
{
    [SerializeField] private Text _scoreText = null;
    //[SerializeField] private Text _scoreTotalText = null;
    [SerializeField] private ChangeScoreTxtUI _changeScore = null;

    //void Awake()
    //{
    //    //_scoreTotalText.text = PlayerPrefs.GetInt("BestScore", 0).ToString();
    //    //_scoreTotalText.text = "Total: " + GameManager.Instance.ScoreManager.Total.ToString();
    //    _scoreText.text = GameManager.Instance.ScoreManager.Score.ToString();
    //}

    // Update is called once per frame
    void Update()
    {
        _scoreText.text = GameManager.Instance.ScoreManager.Score.ToString();
        _changeScore.ChangeParam(GameManager.Instance.ScoreManager.Total);
        Debug.Log($"{GameManager.Instance.ScoreManager.Total}");
        //_scoreTotalText.text = "Total: " + GameManager.Instance.ScoreManager.Total.ToString();
        //_scoreTotalText.text = PlayerPrefs.GetInt("BestScore", 0).ToString();
    }

    public void TapToResetButton()
    {
        Debug.Log("Reset()");
        GameManager.Instance.GameController.Reset();
    }
}
