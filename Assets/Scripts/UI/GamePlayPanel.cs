using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayPanel : UIBasePanel
{
    [SerializeField] private Text _scoreText = null;
    [SerializeField] private Text _bestText = null;

    void Awake()
    {
        _bestText.text = PlayerPrefs.GetInt("BestScore", 0).ToString();
        _scoreText.text = GameManager.Instance.ScoreManager.Score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        _scoreText.text = GameManager.Instance.ScoreManager.Score.ToString();
        _bestText.text = PlayerPrefs.GetInt("BestScore", 0).ToString();
    }
}
