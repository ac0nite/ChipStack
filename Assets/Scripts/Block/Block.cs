using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] public BlockCollision Collision = null;
    [SerializeField] public BlockMovement Movement = null;

    [SerializeField] public GameObject _fxForDestroy = null;

    private void OnDestroy()
    {
        Instantiate(_fxForDestroy, transform.position, Quaternion.identity, GameManager.Instance.transform);
    }
}
