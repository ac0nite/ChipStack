using Remainders;
using UnityEngine;

namespace Components
{
    public class ViewBase : MonoBehaviour, IViewComponent
    {
        public Transform Root => transform;
        public Animator Animator { get; private set; }
        
        private CustomPhysics _physics;
        
        private void OnValidate() => Awake();
        
        protected virtual void Awake()
        {
            Animator = GetComponentInChildren<Animator>();
            _physics = new CustomPhysics(GetComponentInChildren<Rigidbody>());
        }

        public virtual void Enable() => gameObject.SetActive(true);
        public virtual void Disable() => gameObject.SetActive(false);
        public void Reset()
        {
            Disable();
            _physics.Disable();
            
            // TODO RESET VIEW
        }
    }
}