using UnityEngine;

namespace Remainders
{
    public class CustomPhysics
    {
        private readonly Rigidbody _rigidbody;

        public CustomPhysics(Rigidbody rigidbody)
        {
            _rigidbody = rigidbody;
        }

        public void Enable()
        {
            if(_rigidbody) _rigidbody.isKinematic = false;
        }
        
        public void Disable()
        {
            if(_rigidbody) _rigidbody.isKinematic = true;
        }
    }
}