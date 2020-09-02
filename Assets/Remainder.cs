using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remainder : MonoBehaviour
{
    [SerializeField] public Transform Remainder_1 = null;
    [SerializeField] public Transform Remainder_2 = null;
    [SerializeField] private Rigidbody _rigidbody = null;
    [SerializeField] private float _power = 5f;

    public void Force(Vector3 direction)
    {
        _rigidbody.useGravity = true;
        _rigidbody.AddForce(direction * _power, ForceMode.Impulse);
    }
}
