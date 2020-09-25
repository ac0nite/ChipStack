using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoScreenPanel : UIBasePanel
{
    private void LogoScreenEndAnimation()
    {
        Debug.Log("LogoScreenEndAnimation");
        UIManager.Instance.ShowPanel(UITypePanel.StartScreen);
        GameManager.Instance.AudioManager.StartMusicBackground();
    }
}

