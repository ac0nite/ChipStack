using System;
using Core.Singleton;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : SingletoneGameObject<InputManager>
{
    public Action TapEvent;
    private string _tap;
    private Touch _touch;
    private bool _isLocked;

    private void Update()
    {
        if(_isLocked) return;
        
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            TapEvent?.Invoke();
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

    public void Locked() => _isLocked = true;
    public void UnLocked() => _isLocked = false;
}
