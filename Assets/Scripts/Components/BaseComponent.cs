﻿using Remainders;
using UnityEngine;

namespace Components
{
    public interface IComponent
    {
        public Vector3 Position { get; set; }
        public Vector3 Size { get; set; }
        public void ChangePivot(PivotTransform.PivotWidth pivotWidth, PivotTransform.PivotHeight pivotHeight);
    }
    public class BaseComponent : MonoBehaviour, IComponent
    {
        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        public Vector3 Size
        {
            get => transform.localScale;
            set => transform.localScale = value;
        }

        public void ChangePivot(PivotTransform.PivotWidth pivotWidth, PivotTransform.PivotHeight pivotHeight)
        {
            throw new System.NotImplementedException();
        }
    }   
}