using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int _score = 0;
    private int _best = 0;

    public int Score
    {
        get { return _score; }
    }

    void Start()
    {
        _best = PlayerPrefs.GetInt("BestScore", 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ModifyScore(int points)
    {
        if (points < 0)
            return;

        if (points == 0)
        {
            OnChangeScoreRecord();
            _score = 0;
        }

        _score = (int) Mathf.Clamp(_score + points, 0f, float.MaxValue);
    }

    private void OnChangeScoreRecord()
    {
        _best = Mathf.Clamp(_score, _best, Int32.MaxValue);
        PlayerPrefs.SetInt("BestScore", _best);
        PlayerPrefs.Save();
    }
}
