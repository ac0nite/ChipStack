using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class InputManager : SingletoneGameObject<InputManager>
{
    public Action EventTap;
    private void Update()
    {
        if(Input.GetMouseButtonUp(0))
            EventTap?.Invoke();
    }
}
