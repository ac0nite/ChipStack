using UnityEngine;

namespace Components
{
    public interface IComponent
    {
        public Vector3 Position { get; set; }
        public Vector3 Size { get; set; }
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
    }   
}