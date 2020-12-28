using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStartScreen : UIBasePanel
{
    [SerializeField] private Text _best = null;
    //[SerializeField] private Text _score = null;
    [SerializeField] private Text _total = null;
    [SerializeField] private Button _soundButton = null;
    [SerializeField] private Text _message = null;

    void Start()
    {
        // OnEnable();
    }

    void OnEnable()
    {
        if (GameManager.Instance.ScoreManager.Total > PlayerPrefs.GetInt("BestScore", 0))
            _best.text = "Best: " + GameManager.Instance.ScoreManager.Total.ToString();
        else
            _best.text = "Best: " + PlayerPrefs.GetInt("BestScore", 0).ToString();

        //_score.text = GameManager.Instance.ScoreManager.Score.ToString();

        if (GameManager.Instance.AudioManager.IsSound)
            _soundButton.GetComponentInChildren<Text>().text = "Sound";
        else
            _soundButton.GetComponentInChildren<Text>().text = "No Sound";

        if(GameManager.Instance.GameController.State == StateGame.WIN_STATE)
            _message.text = "- next Stage " + GameManager.Instance.ScoreManager.Stage.ToString() + " -\nYou win! Try again...";
        else
            _message.text = "- next Stage " + GameManager.Instance.ScoreManager.Stage.ToString() + " -"; ;

        _total.text = "Total: " + GameManager.Instance.ScoreManager.Total.ToString();

        InputManager.Instance.EventTap += OnTap;
    }

    void OnDisable()
    {
        //Debug.Log($"OnDisable()");
        InputManager.Instance.EventTap -= OnTap;
    }

    public void OnTap()
    {
        //Debug.Log($"OnTap()");
        TapToStartButton();
        //InputManager.Instance.EventTap -= OnTap;
    }

    public void TapToStartButton()
    {
        GameManager.Instance.ScoreManager.ModifyScore(0);
        UIManager.Instance.ShowPanel(UITypePanel.GamePlay);
        GameManager.Instance.GameController.Go();
    }

    public void SoundButton()
    {
        var btnText = _soundButton.GetComponentInChildren<Text>();
        if (btnText.text == "Sound")
        {
            btnText.text = "No Sound";
             //GameManager.Instance.AudioManager.StopMusicBackground();
            GameManager.Instance.AudioManager.NoVolumeSound();
        }
        else
        {
            btnText.text = "Sound";
            //GameManager.Instance.AudioManager.StartMusicBackground();
            GameManager.Instance.AudioManager.YesVolumeSound();
        }
    }

    public void QuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
