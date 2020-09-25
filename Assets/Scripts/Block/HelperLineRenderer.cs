using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperLineRenderer : MonoBehaviour
{
    private Vector3 _point = Vector3.zero;
    [SerializeField] private LineRenderer _line = null;
    void Start()
    {
        // _point = new Vector3(transform.position.x - transform.localScale.x / 2f, transform.position.y - transform.localScale.y / 2f, transform.position.z - transform.localScale.z / 2f);
        _point = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }
    void Update()
    {
        _line.SetPosition(0, _point);
        _line.SetPosition(1, new Vector3(_point.x, _point.y - 3f, _point.z));
//#pragma warning disable 618
//        _line.SetWidth(0.1f, 0.1f);
//#pragma warning restore 618
    }
}
