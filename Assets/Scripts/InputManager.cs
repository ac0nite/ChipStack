using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class InputManager : SingletoneGameObject<InputManager>
{
    public Action EventTap;
    private string _tap;
    private Touch _touch;
    private void Update()
    {
#if UNITY_EDITOR
        if(Input.GetMouseButtonDown(0))
            EventTap?.Invoke();
#elif UNITY_STANDALONE_WIN
        if(Input.GetMouseButtonDown(0))
            EventTap?.Invoke();
#else
      if (Input.touchCount > 0)
        {
            _touch = Input.GetTouch(0);
            if (_touch.phase == TouchPhase.Began)
            {
                EventTap?.Invoke();
            }
        }  
#endif
    }
}
