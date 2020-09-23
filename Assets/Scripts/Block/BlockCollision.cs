using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BlockCollision : BlockCollisionBase
{
    private ContactPoint[] _contactPoints = new ContactPoint[4];
    private List<Vector3> side = new List<Vector3>();
   // [SerializeField] public Transform Block = null;
    [SerializeField] private Remainder _remainderPrefab = null;
    public Action<BlockCollision> EventNextBlock;
    private int count = 0;
    [SerializeField] private AudioSource _cutAudio = null;
    [SerializeField] private Light _lighting = null;
    private bool _isLighting = false;
    [SerializeField] private float _stepIntensityLight = 5f;

    void Update()
    {
        //if (_isLighting)
        //{
        //    _lighting.intensity += _stepIntensityLight;
        //}
    }

    public void RunLighting()
    {
        //_isLighting = true;
        _lighting.intensity = 5f;
    }

    public void StopLighting()
    {
        //_isLighting = false;
        _lighting.intensity = 0f;
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log($"---OnCollisionEnter---");
        StopLighting();
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

        //StopLighting();
        _cutAudio.Play();

            //
            //        if(!blockCollision.IsCollision)
            //            return;

        //if (!IsCollision)
        //    return; 

        Debug.Log($"----------------- OnCollisionEnter -----------------", transform.gameObject);

        //blockCollision.IsCollision = false;

        //IsCollision = false;

        var contacts = other.GetContacts(_contactPoints);

        //transform.GetComponent<Rigidbody>().useGravity = false;
        //        Debug.Log(contacts);


        List<Vector2> angles = new List<Vector2>();
        var c = other.gameObject.transform;
        angles.Add(new Vector2(c.position.x + c.localScale.x / 2, c.position.z + c.localScale.z / 2));
        angles.Add(new Vector2(c.position.x + c.localScale.x / 2, c.position.z - c.localScale.z / 2));
        angles.Add(new Vector2(c.position.x - c.localScale.x / 2, c.position.z - c.localScale.z / 2));
        angles.Add(new Vector2(c.position.x - c.localScale.x / 2, c.position.z + c.localScale.z / 2));

        //foreach (var a in angles)
        //{
        //    Debug.Log($"angl: {a}");
        //}

        Vector3 point0 = Vector3.zero;

        foreach (ContactPoint contact in _contactPoints)
        {
            //Debug.DrawRay(contact.point, contact.normal * 5, Color.red, 5);

            //RaycastHit h;
            //if (Physics.Raycast(contact.point, contact.normal, out h, Mathf.Infinity))
            //{
            //    Debug.Log($"hit!!!");
            //}
            //var hits = Physics.RaycastAll(new Ray(contact.point, contact.normal), 5f);
            //Debug.Log($"hits length: {hits.Length}");
            
            //Debug.Log($"count: {angles.Find(a => (a.x == contact.point.x && a.y == contact.point.z))}  {contact.point}");
            if (angles.Exists(a => (a.x == contact.point.x && a.y == contact.point.z)))
            {
               // Debug.Log($"YES!!!");
                point0 = contact.point;

                Debug.DrawRay(contact.point, contact.normal * 5, Color.red, 5);

                break;
            }
        }

        if (point0 == Vector3.zero)
            point0 = _contactPoints[0].point;

        //Debug.DrawLine(_contactPoints[0].point, _contactPoints[1].point, Color.green, 5f);
        //Debug.DrawLine(_contactPoints[0].point, _contactPoints[2].point, Color.green, 5f);
        //Debug.DrawLine(_contactPoints[0].point, _contactPoints[3].point, Color.green, 5f);

            //search first angle 
        foreach (ContactPoint contact in _contactPoints)
        {
            if(contact.point != point0)
                side.Add(point0 - contact.point);
        }

       // Debug.Log($"side count: {side.Count}");
        
        //side.Add(_contactPoints[0].point - _contactPoints[1].point);
        //side.Add(_contactPoints[0].point - _contactPoints[2].point);
        //side.Add(_contactPoints[0].point - _contactPoints[3].point);
        
        side.Sort((a, b) => a.magnitude.CompareTo(b.magnitude));
        
        Debug.DrawLine(point0, side[0], Color.green, 5f);
        Debug.DrawLine(point0, side[1], Color.green, 5f);

        var center = Vector3.Lerp(point0 - side[0], point0 - side[1], 0.5f);
        Debug.DrawLine(point0, center, Color.blue, 5f);

        var a1 = Vector3.Project(center - point0, side[0]).magnitude;
        var a2 = Vector3.Project(center - point0, side[1]).magnitude;
        //Debug.Log($"{a1} - {a2}");
        //Debug.Log($"dot_sise_0: {Vector3.Dot(side[0].normalized, transform.forward)}");
        //Debug.Log($"dot_side_1: {Vector3.Dot(side[1].normalized, transform.forward)}");

        Vector3 scale = Vector3.zero;
        Vector3 scale_remainder = Vector3.zero;
        Vector3 position_remainder = Vector3.zero;
        
        Vector3 scale_remainder_2 = Vector3.zero;
        Vector3 position_remainder_2 = Vector3.zero;

        var dot = Vector3.Dot(side[0].normalized, transform.forward);

        Debug.Log($"side[0]: {side[0].magnitude} side[1]: {side[1].magnitude}");
        //Debug.Log($"dot: {dot} {Math.Round(dot)}");
        if (Math.Round(dot) == 0f)
        {
            Debug.Log($"-1-");
            scale = new Vector3(side[0].magnitude, transform.localScale.y, side[1].magnitude);
        
            var direction_litle = Vector3.Dot(side[1].normalized, Vector3.forward);
            var direction_large = Vector3.Dot(side[0].normalized, Vector3.left);
            
           // Debug.Log($"direction_little: {direction_litle}");
           // Debug.Log($"direction_large: {direction_large}");
            
            scale_remainder = new Vector3(side[0].magnitude, transform.localScale.y, transform.localScale.z - side[1].magnitude);
            position_remainder = new Vector3(center.x, transform.position.y, point0.z + direction_litle * (transform.localScale.z - side[1].magnitude)/2f);
            
            scale_remainder_2 = new Vector3(transform.localScale.x - side[0].magnitude, transform.localScale.y, transform.localScale.z);
            position_remainder_2 = new Vector3(point0.x - direction_large * (transform.localScale.x - side[0].magnitude)/2f, transform.position.y, transform.position.z); 
        }
        else
        {
            Debug.Log($"-2-");
            scale = new Vector3(side[1].magnitude, transform.localScale.y, side[0].magnitude);

           // direction = dot == 1 ? -1 : 1;
            var direction_litle = Vector3.Dot(side[1].normalized, Vector3.left);
            var direction_large = Mathf.Round(dot);

           // Debug.Log($"direction_litle: {direction_litle}");
           // Debug.Log($"direction_large: {direction_large}");

            scale_remainder = new Vector3(transform.localScale.x - side[1].magnitude, transform.localScale.y, side[0].magnitude);
            position_remainder = new Vector3(point0.x - direction_litle * (transform.localScale.x - side[1].magnitude)/2f, transform.position.y, center.z);
            
            scale_remainder_2 = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z - side[0].magnitude);
            position_remainder_2 = new Vector3(transform.position.x, transform.position.y, point0.z + direction_large * (transform.localScale.z - side[0].magnitude)/2f); 
        }

        

        Vector3 position = new Vector3(center.x, transform.position.y, center.z);

        Debug.Log($"new scale: {scale} position: {position}");

        transform.position = position;
        transform.localScale = scale;

        //Destroy(transform.gameObject);
        //GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //go.transform.position = position;
        //go.transform.localScale = scale;
        //go.transform.SetParent(GameManager.Instance.Base);

        //transform.position = scale_remainder;
        //transform.localScale = position_remainder;

        //Debug.Log($"scale:{scale}  center:{center}");

        var color = GetComponent<BlockColor>().Color;

        var remainder = Instantiate(_remainderPrefab);
        remainder.Remainder_1.localScale = scale_remainder;
        remainder.Remainder_1.position = position_remainder;
        remainder.Remainder_2.localScale = scale_remainder_2;
        remainder.Remainder_2.position = position_remainder_2;
        remainder.RemainderColor.Color = color;

        var dir = (remainder.Remainder_2.position - transform.position).normalized / 2f;
        //var dir = Vector3.down;
        dir.y = -0.1f;
        //remainder.Force((remainder.Remainder_2.position - transform.position).normalized);
        remainder.Force(dir);

        EventNextBlock?.Invoke(this);
      
       // StartCoroutine(DestroyRemainder(remainder));
       GameManager.Instance.Remainders.Add(remainder);

        side.Clear();

        
    }

    IEnumerator test(Transform t)
    {
        yield return new WaitForSeconds(0.5f);
        t.GetComponent<Rigidbody>().isKinematic = true;
    }

    IEnumerator DestroyRemainder(Remainder _remainder)
    {
        yield return new WaitForSeconds(1.5f);
       // Destroy(_remainder.gameObject);
    }
}
