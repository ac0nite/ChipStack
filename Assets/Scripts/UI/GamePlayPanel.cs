using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayPanel : UIBasePanel
{
    [SerializeField] private Text _scoreText = null;
    [SerializeField] private Text _scoreTotalText = null;

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
        _scoreTotalText.text = "Total: " + GameManager.Instance.ScoreManager.Total.ToString();
        //_scoreTotalText.text = PlayerPrefs.GetInt("BestScore", 0).ToString();
    }
}
