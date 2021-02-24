using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BlockCollision : BlockCollisionBase
{
    public Action<BlockCollision> EventNextBlock;

    [SerializeField] private Remainder _remainderPrefab = null;
    [SerializeField] private AudioSource _cutAudio = null;
    [SerializeField] private Light _lighting = null;

    private ContactPoint[] _contactPoints = new ContactPoint[4];
    private List<Vector3> side = new List<Vector3>();

    public void RunLighting()
    {
        _lighting.intensity = 5f;
    }

    public void StopLighting()
    {
        _lighting.intensity = 0f;
    }

    private void OnCollisionEnter(Collision other)
    {
        StopLighting();
        Vibration.Vibrate(40);
    }

    private void OnCollisionStay(Collision other)
    {
        var blockCollision = other.transform.GetComponent<BlockCollisionBase>();
        if (blockCollision == null) 
            return;


        if (Collision == TypeCollision.First && blockCollision.Collision == TypeCollision.Second)
        {
            Collision = TypeCollision.Second;
            blockCollision.Collision = TypeCollision.Undefibed;
            transform.SetParent(GameManager.Instance.Base);
        }
        else
        {
            return;
        }

        _cutAudio.Play();

        var contacts = other.GetContacts(_contactPoints);

        List<Vector2> angles = new List<Vector2>();
        var c = other.gameObject.transform;
        angles.Add(new Vector2(c.position.x + c.localScale.x / 2, c.position.z + c.localScale.z / 2));
        angles.Add(new Vector2(c.position.x + c.localScale.x / 2, c.position.z - c.localScale.z / 2));
        angles.Add(new Vector2(c.position.x - c.localScale.x / 2, c.position.z - c.localScale.z / 2));
        angles.Add(new Vector2(c.position.x - c.localScale.x / 2, c.position.z + c.localScale.z / 2));

        Vector3 point0 = Vector3.zero;

        foreach (ContactPoint contact in _contactPoints)
        {
            if (angles.Exists(a => (a.x == contact.point.x && a.y == contact.point.z)))
            {
                point0 = contact.point;

                Debug.DrawRay(contact.point, contact.normal * 5, Color.red, 5);

                break;
            }
        }

        if (point0 == Vector3.zero)
            point0 = _contactPoints[0].point;

        //search first angle 
        foreach (ContactPoint contact in _contactPoints)
        {
            if(contact.point != point0)
                side.Add(point0 - contact.point);
        }
        
        side.Sort((a, b) => a.magnitude.CompareTo(b.magnitude));
        
        //Debug.DrawLine(point0, side[0], Color.green, 5f);
        //Debug.DrawLine(point0, side[1], Color.green, 5f);

        var center = Vector3.Lerp(point0 - side[0], point0 - side[1], 0.5f);
        //Debug.DrawLine(point0, center, Color.blue, 5f);

        var a1 = Vector3.Project(center - point0, side[0]).magnitude;
        var a2 = Vector3.Project(center - point0, side[1]).magnitude;


        Vector3 scale = Vector3.zero;
        Vector3 scale_remainder = Vector3.zero;
        Vector3 position_remainder = Vector3.zero;
        
        Vector3 scale_remainder_2 = Vector3.zero;
        Vector3 position_remainder_2 = Vector3.zero;

        var dot = Vector3.Dot(side[0].normalized, transform.forward);

        if (Math.Round(dot) == 0f)
        {
            scale = new Vector3(side[0].magnitude, transform.localScale.y, side[1].magnitude);
        
            var direction_litle = Vector3.Dot(side[1].normalized, Vector3.forward);
            var direction_large = Vector3.Dot(side[0].normalized, Vector3.left);

            scale_remainder = new Vector3(side[0].magnitude, transform.localScale.y, transform.localScale.z - side[1].magnitude);
            position_remainder = new Vector3(center.x, transform.position.y, point0.z + direction_litle * (transform.localScale.z - side[1].magnitude)/2f);
            
            scale_remainder_2 = new Vector3(transform.localScale.x - side[0].magnitude, transform.localScale.y, transform.localScale.z);
            position_remainder_2 = new Vector3(point0.x - direction_large * (transform.localScale.x - side[0].magnitude)/2f, transform.position.y, transform.position.z); 
        }
        else
        {
            scale = new Vector3(side[1].magnitude, transform.localScale.y, side[0].magnitude);

            var direction_litle = Vector3.Dot(side[1].normalized, Vector3.left);
            var direction_large = Mathf.Round(dot);

            scale_remainder = new Vector3(transform.localScale.x - side[1].magnitude, transform.localScale.y, side[0].magnitude);
            position_remainder = new Vector3(point0.x - direction_litle * (transform.localScale.x - side[1].magnitude)/2f, transform.position.y, center.z);
            
            scale_remainder_2 = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z - side[0].magnitude);
            position_remainder_2 = new Vector3(transform.position.x, transform.position.y, point0.z + direction_large * (transform.localScale.z - side[0].magnitude)/2f); 
        }

        Vector3 position = new Vector3(center.x, transform.position.y, center.z);

        transform.position = position;
        transform.localScale = scale;

        var color = GetComponent<BlockColor>().Color;

        var remainder = Instantiate(_remainderPrefab);
        remainder.Remainder_1.localScale = scale_remainder;
        remainder.Remainder_1.position = position_remainder;
        remainder.Remainder_2.localScale = scale_remainder_2;
        remainder.Remainder_2.position = position_remainder_2;
        remainder.RemainderColor.Color = color;

        var dir = (remainder.Remainder_2.position - transform.position).normalized / 2f;
        dir.y = -0.1f;
        remainder.Force(dir);
        EventNextBlock?.Invoke(this);

        side.Clear();
    }
}
