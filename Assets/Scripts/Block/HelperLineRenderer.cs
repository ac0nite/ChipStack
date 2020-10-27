using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperLineRenderer : MonoBehaviour
{
    private Vector3[] _points;
    [SerializeField] private List<LineRenderer> _lines;
    private RaycastHit _hitInfo;
    private Vector3 _endPoint;

    void Start()
    {
        //_point = new Vector3(transform.position.x - transform.localScale.x / 2f, transform.position.y - transform.localScale.y / 2f, transform.position.z - transform.localScale.z / 2f);
        // _lines = new LineRenderer[3];
        _points = new Vector3[4];
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

        //_point = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        for (int i=0; i < _points.Length; i++)
        {
            _endPoint = new Vector3(_points[i].x, _points[i].y - 1.3f, _points[i].z);
            if (Physics.Linecast(_points[i], _endPoint, out _hitInfo))
            {
                _lines[i].enabled = true;
               // Debug.Log($"_hitInfo.distance: {_hitInfo.distance}");
                _lines[i].SetPosition(0, _points[i]);
                _lines[i].SetPosition(1, _endPoint);
            }
            else
            {
                _lines[i].enabled = false;
            }
        }

        //_lines[0].SetPosition(0, _points[0]);
        //_lines[0].SetPosition(1, new Vector3(_points[0].x, _points[0].y - 3f, _points[0].z));

        //_lines[1].SetPosition(0, _points[1]);
        //_lines[1].SetPosition(1, new Vector3(_points[1].x, _points[1].y - 3f, _points[1].z));

        //_lines[2].SetPosition(0, _points[2]);
        //_lines[2].SetPosition(1, new Vector3(_points[2].x, _points[2].y - 3f, _points[2].z));

        //Debug.DrawLine(_point, new Vector3(_point.x, _point.y - 3f, _point.z), Color.red, 0.5f);
    }
}
