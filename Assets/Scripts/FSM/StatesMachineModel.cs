using System;
using Core.FSM.Base;
using Core.UI.MVP;
using UnityEngine;

namespace FSM
{
    public class StatesMachineModel : IModel
    {
        public event Action<Type> ChangeStateEvent;
        public void ChangeState<T>() where T : BaseState
        {
            Debug.Log($"ChangeState: {typeof(T)}");
            ChangeStateEvent?.Invoke(typeof(T));
        }
    }
}