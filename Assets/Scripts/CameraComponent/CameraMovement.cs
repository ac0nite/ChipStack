using System;
using DG.Tweening;
using UnityEngine;
using Random = System.Random;

namespace CameraComponent
{
    public class CameraMovement
    {
        private readonly Transform _transform;
        private readonly Camera _camera;
        private readonly Vector3 _clockwiseRotation = 360 * Vector3.up;
        private readonly Vector3 _counterClockwiseRotation = -360 * Vector3.up;
        private readonly Random _random;
        private readonly Settings _settings;
        private int _direction;

        public CameraMovement(Transform cameraComponent, Settings settings)
        {
            _transform = cameraComponent;
            _settings = settings;
            _camera = _transform.GetComponentInChildren<Camera>();
            _random = new System.Random();
        }

        public void MoveRandomRotateForAxisY()
        {
            RotateTween.Play();
        }
        
        public void MoveForAxisY(Vector3 position)
        {
            MoveTween(position).Play();
        }

        public void ResetView(Action callback = null)
        {
            _transform.DOKill();
            ResetSequence.OnComplete(() => callback?.Invoke()).Play();
        }

        private Tweener MoveTween(Vector3 position) => _transform
            .DOLocalMoveY(_transform.position.y + position.y, _settings.MoveDuration)
            .SetEase(_settings.MoveEase);
        private Tweener RotateTween => _transform
            .DORotate(RandomWiseRotation, _settings.RotateDuration, RotateMode.FastBeyond360)
            .SetEase(_settings.RotateEase)
            .SetLoops(-1, LoopType.Incremental);
        private Sequence ResetSequence => DOTween.Sequence()
            .Append(_transform
                .DORotate(Vector3.zero, _settings.ResetDuration * 0.8f)
                .SetLoops(1, LoopType.Incremental))
            .Join(_transform.DOLocalMoveY(0, _settings.ResetDuration))
            .Append(_transform
                .DORotate(Vector3.zero, _settings.ResetDuration * 0.2f))
            .SetEase(_settings.ResetEase);

        private Vector3 RandomWiseRotation => _random.Next(0, 2) == 0
            ? _clockwiseRotation
            : _counterClockwiseRotation;

        [Serializable]
        public struct Settings
        {
            public float RotateDuration;
            public Ease RotateEase;
            public float MoveDuration;
            public Ease MoveEase;
            public float ResetDuration;
            public Ease ResetEase;
        }
    }
}