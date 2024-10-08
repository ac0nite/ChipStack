﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            EventTap?.Invoke();
        }

#elif UNITY_STANDALONE_WIN
                if(Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
                    EventTap?.Invoke();
#else
              if (Input.touchCount > 0 && !EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
                {
                    _touch = Input.GetTouch(0);
                    if (_touch.phase == TouchPhase.Began)
                    {
                        EventTap?.Invoke();
                        //Vibration.Vibrate();
                        //Vibration.Vibrate(1000);
                    }
                }
#endif
    }
}
