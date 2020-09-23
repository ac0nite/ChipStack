using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStartScreen : UIBasePanel
{
    [SerializeField] private Text _best = null;
    [SerializeField] private Text _score = null;
    [SerializeField] private Button _soundButton = null;

    void Start()
    {
        _best.text = PlayerPrefs.GetInt("BestScore", 0).ToString();
        _score.text = GameManager.Instance.ScoreManager.Score.ToString();
    }

    void OnEnable()
    {
        _best.text = PlayerPrefs.GetInt("BestScore", 0).ToString();
        _score.text = GameManager.Instance.ScoreManager.Score.ToString();
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
             GameManager.Instance.AudioManager.StopMusicBackground();
            //GameManager.Instance.AudioManager.NoVolumeSound();
        }
        else
        {
            btnText.text = "Sound";
            GameManager.Instance.AudioManager.StartMusicBackground();
            //GameManager.Instance.AudioManager.YesVolumeSound();
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
