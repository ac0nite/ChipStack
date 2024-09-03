using System;
using System.Collections.Generic;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions;

namespace Pivots
{
    public interface IPivotsScaling
    {
        void ScalingEnable();
        void ScalingDisable();
    }
    public class PivotsScaling : MonoBehaviour, IPivotsScaling
    {
        private IPivotExtendedTransform[] _pivots;
        private IPivotScaling _onePivotScaling;
        private IPivotScaling _twoPivotScaling;
        private CoroutineHandle _updateCoroutine;

        public void SetParams(params IPivotExtendedTransform[] pivots)
        {
            _pivots = pivots;
        }

        public void ScalingEnable()
        {
            InitializePivotPositionAndScaling();
            
            if (Timing.IsAliveAndPaused(_updateCoroutine))
                Timing.ResumeCoroutines(_updateCoroutine);
            else 
                _updateCoroutine = Timing.RunCoroutine(UpdatePivotsCoroutine());
        }

        public void ScalingDisable()
        {
            if (Timing.IsRunning(_updateCoroutine))
                Timing.PauseCoroutines(_updateCoroutine);

            Array.ForEach(_pivots, p => p.AdjustPivot());
        }
        
        private IEnumerator<float> UpdatePivotsCoroutine()
        {
            yield return Timing.WaitForOneFrame;
            UpdatePivotPositionAndScale();
        }

        private void InitializePivotPositionAndScaling()
        {
            _onePivotScaling = new PivotCroppedScaling(_pivots[0], _pivots[1], PivotCroppedScaling.CroppedAxisXDelegate);
            _twoPivotScaling = new PivotScaling(_pivots[0], _pivots[2]);
        }

        private void UpdatePivotPositionAndScale()
        {
            if (_onePivotScaling.IsChanged(_pivots[0]))
                _onePivotScaling.UpdatePositionAndScale();
            
            if(_twoPivotScaling.IsChanged(_pivots[0]))
                _twoPivotScaling.UpdatePositionAndScale();
        }

#if UNITY_EDITOR
        [Button]
        private void Subscribe()
        {
            _pivots ??= transform.GetComponentsInChildren<IPivotExtendedTransform>();
            
            Assert.IsNotNull(_pivots, "PivotTransforms not found");
            Assert.AreEqual(3, _pivots.Length, "The number of pivots should be 3");
            
            InitializePivotPositionAndScaling();
            UnityEditor.EditorApplication.update += UpdatePivotPositionAndScale;
        }
        
        [Button]
        private void UnSubscribe()
        {
            UnityEditor.EditorApplication.update -= UpdatePivotPositionAndScale;
        }  
#endif
    }
}