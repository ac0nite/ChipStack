using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCollisionBase : MonoBehaviour
{
    [SerializeField] private bool _isCollision = true;
    [SerializeField] public bool IsCollision
    {
        get { return _isCollision;}
        set
        {
            _isCollision = value;
           // transform.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    public enum TypeCollision
    {
        Undefibed = -1,
        First = 0,
        Second = 2
    }

    [SerializeField] private TypeCollision _collision = TypeCollision.First;

    public TypeCollision Collision
    {
        get { return _collision; }
        set { _collision = value; }
    }
}
