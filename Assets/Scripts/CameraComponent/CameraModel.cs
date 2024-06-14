using System;
using UnityEngine;

namespace CameraComponent
{
    public interface ICameraMover
    {
        void Rotate();
        void Move(Vector3 position);
        void Reset(Action callback = null);
    }
    public class CameraMover : CameraMovement, ICameraMover
    {
        public CameraMover(Transform cameraComponent, Settings settings) : base(cameraComponent, settings)
        { }

        public void Rotate() => MoveRandomRotateForAxisY();

        public void Move(Vector3 position) => MoveForAxisY(position);

        public void Reset(Action callback = null) => ResetView(callback);
    }
}