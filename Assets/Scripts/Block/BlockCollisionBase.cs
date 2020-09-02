using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCollisionBase : MonoBehaviour
{
    [SerializeField] private bool _isCollision = true;
    [SerializeField] public bool IsCollision
    {
        get { return _isCollision;}
        set { _isCollision = value; }
    }
}
