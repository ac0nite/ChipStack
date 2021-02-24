using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperLineRenderer : MonoBehaviour
{
    private Vector3[] _points;
    [SerializeField] private List<LineRenderer> _lines = new List<LineRenderer>();
    private RaycastHit _hitInfo;
    private Vector3 _endPoint;

    void Start()
    {
        _points = new Vector3[4];


        foreach (var line in _lines)
        {
            line.enabled = false;
        }
    }

    public void DisableHelper()
    {
        enabled = false;
        foreach (var line in _lines)
        {
            line.enabled = false;
        }
    }

    void Update()
    {

        _points[0] = new Vector3(transform.position.x - transform.localScale.x / 2f, transform.position.y - transform.localScale.y / 2f, transform.position.z - transform.localScale.z / 2f);
        _points[1] = new Vector3(transform.position.x + transform.localScale.x / 2f, transform.position.y - transform.localScale.y / 2f, transform.position.z - transform.localScale.z / 2f);
        _points[2] = new Vector3(transform.position.x - transform.localScale.x / 2f, transform.position.y - transform.localScale.y / 2f, transform.position.z + transform.localScale.z / 2f);
        _points[3] = new Vector3(transform.position.x + transform.localScale.x / 2f, transform.position.y - transform.localScale.y / 2f, transform.position.z + transform.localScale.z / 2f);

        for (int i=0; i < _points.Length; i++)
        {
            _endPoint = new Vector3(_points[i].x, _points[i].y - 1.0f, _points[i].z);
            if (Physics.Linecast(_points[i], _endPoint, out _hitInfo))
            {
                _lines[i].enabled = true;
                _lines[i].SetPosition(0, _points[i]);
                _lines[i].SetPosition(1, _endPoint);
            }
            else
            {
                _lines[i].enabled = false;
            }
        }
    }
}
