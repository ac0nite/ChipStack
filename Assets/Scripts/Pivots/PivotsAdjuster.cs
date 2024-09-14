using System.Collections.Generic;
using MEC;

namespace Pivots
{
    public interface IPivotsAdjuster
    {
        void Enable(MEC.Segment segment = Segment.Update);
        void Disable();
    }
    
    public class PivotsAdjuster : IPivotsAdjuster
    {
        private IPivotExtended[] _pivots;
        private IPivotScaling _onePivotScaling;
        private IPivotScaling _twoPivotScaling;
        private CoroutineHandle? _updateCoroutine = null;
        private IPivotExtended _root;

        public PivotsAdjuster(params IPivotExtended[] pivots)
        {
            _pivots = pivots;
        }
        
        public void Enable(MEC.Segment segment = Segment.Update)
        {
            InitializePivotPositionAndScaling();
            _updateCoroutine ??= Timing.RunCoroutine(UpdatePivotsCoroutine(), segment);
            Timing.ResumeCoroutines(_updateCoroutine.Value);
        }
        
        public void Disable()
        {
            if(_updateCoroutine.HasValue) 
                Timing.PauseCoroutines(_updateCoroutine.Value);
            // foreach (var p in _pivots) 
            //     p.FineTunePivotAlignment();

#if UNITY_EDITOR
            if (_updateCoroutine is { Segment: Segment.EditorUpdate or Segment.Invalid })
                _updateCoroutine = null;
#endif
        }
        
        private IEnumerator<float> UpdatePivotsCoroutine()
        {
            while (true)
            {
                yield return Timing.WaitForOneFrame;
                UpdatePivotPositionAndScale();
            }
        }

        private void InitializePivotPositionAndScaling()
        {
            _root ??= _pivots[0];
            _onePivotScaling = new PivotScaling(_root, _pivots[1]);
            _twoPivotScaling = new PivotCroppedScaling(_root, _pivots[2]);
        }

        private void UpdatePivotPositionAndScale()
        {
            if (_onePivotScaling.IsChanged(_root))
                _onePivotScaling.UpdatePositionAndScale();

            if (_twoPivotScaling.IsChanged(_root))
                _twoPivotScaling.UpdatePositionAndScale();
        }
    }
}