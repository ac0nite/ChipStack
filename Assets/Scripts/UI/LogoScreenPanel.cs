using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoScreenPanel : UIBasePanel
{
    [SerializeField] private AudioSource _backgrountAudioLogo = null;
    [SerializeField] [Range(0.1f, 1f)] private float _timeDelayPlay = 0.5f;

    private void Awake()
    {
        StartCoroutine(PlayBackGround());

    }
    private void LogoScreenEndAnimation()
    {
        Debug.Log("LogoScreenEndAnimation");
        UIManager.Instance.ShowPanel(UITypePanel.StartScreen);
        GameManager.Instance.AudioManager.StartMusicBackground();
    }

    IEnumerator PlayBackGround()
    {
        yield return new WaitForSeconds(_timeDelayPlay);
        _backgrountAudioLogo.Play();
    }
}

