﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Remainders
{
    [Serializable]
    public class CustomComponent
    {
        [SerializeField]
        private readonly Rigidbody _rigidbody;
        [SerializeField]
        private readonly Transform _transform;
        [SerializeField]
        private readonly CustomRenderer[] _renderers;
        [SerializeField]
        private readonly Collider _collider;
        [SerializeField] 
        private readonly Animator _animator;

        public CustomComponent(Transform transform)
        {
            _transform = transform;
            _rigidbody = transform.GetComponentInChildren<Rigidbody>();
            _collider = transform.GetComponentInChildren<Collider>();
        
            var renderers = transform.GetComponentsInChildren<Renderer>();
            _renderers = new CustomRenderer[renderers.Length];
            for (var i = 0; i < renderers.Length; i++)
                _renderers[i] = new CustomRenderer(renderers[i]);
            
            _animator = transform.GetComponentInChildren<Animator>();
        }
    
        public void EnablePhysics()
        {
            _rigidbody.useGravity = true;
            _rigidbody.isKinematic = false;
            _collider.enabled = true;
        }
    
        public void DisablePhysics()
        {
            _rigidbody.useGravity = false;
            _rigidbody.isKinematic = true;
            _collider.enabled = false;
        }
    
        public void EnableRenderers()
        {
            foreach (var renderer in _renderers)
                renderer.Enable();
        }
    
        public void DisableRenderers()
        {
            foreach (var renderer in _renderers)
                renderer.Disable();
        }
        
        public Animator Animator => _animator;
    
        public void Enable() => _transform.gameObject.SetActive(true);
        public void Disable() => _transform.gameObject.SetActive(false);

        public void SetTransformDefault()
        {
            _transform.position = Vector3.zero;
            _transform.rotation = Quaternion.identity;
        }
    
        public IReadOnlyCollection<CustomRenderer> Renderers => _renderers;

        public void EnableActive() => _transform.gameObject.SetActive(true);
        public void DisableActive() => _transform.gameObject.SetActive(false);

        public void AddForce(Vector3 direction)
        {
            _rigidbody.AddForce(direction, ForceMode.Impulse);
        }
    }
}