using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remainder : MonoBehaviour
{
    [SerializeField] public Transform Remainder_1 = null;
    [SerializeField] public Transform Remainder_2 = null;
    [SerializeField] private Rigidbody _rigidbody = null;
    [SerializeField] private float _power = 10f;
    [SerializeField] public RemainderColor RemainderColor = null;
    [SerializeField] private List<string> _listAudio = new List<string>();
    [SerializeField] private AudioSource _audio = null;

    public void Force(Vector3 direction)
    {
        _rigidbody.useGravity = true;
        _rigidbody.AddForce(direction * _power, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"OnTriggerEnter: {other.name}", transform.gameObject);
        _audio.PlayOneShot(Resources.Load<AudioClip>(("Music/" + _listAudio[Random.Range(0, _listAudio.Count)])));
        var colliders = GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.isTrigger = false;
        }
    }

}
