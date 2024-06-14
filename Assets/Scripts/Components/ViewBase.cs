using Remainders;
using UnityEngine;

namespace Components
{
    public class ViewBase : MonoBehaviour
    {
        public CustomComponent Component { get; private set; }
        protected virtual void Awake()
        {
            Component = new CustomComponent(transform);
        }
    }
}