using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ScoreManager : MonoBehaviour
{
    private int _score = 0;
    private int _best = 0;
    private int _total = 0;
    private int _stage = 1;
    [SerializeField] public int MaxStages = 0;
    [SerializeField] public int LimitBlocksInRound = 4;

    void Start()
    {
        _best = PlayerPrefs.GetInt("BestScore", 0);
        if (MaxStages > GameManager.Instance.Property.Count)
            MaxStages = GameManager.Instance.Property.Count;
    }
    
    public int Stage
    {
        get { return _stage; }
        set
        {
            _stage = (value % (MaxStages + 1)) == 0 ? 1 : (value);
        }
    }

    public int Score
    {
        get { return _score; }
    }

    public int Total
    {
        get { return _total; }
        set { _total = value; }
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

    public void ModifyScore(int points, float area)
    {
        ModifyScore(points);
        
        //TODO как то изменить
        _total += Mathf.RoundToInt(area * _stage * 100f);
    }

    private void OnChangeScoreRecord()
    {
        _best = Mathf.Clamp(_total, _best, Int32.MaxValue);
        PlayerPrefs.SetInt("BestScore", _best);
        PlayerPrefs.Save();
    }
}
