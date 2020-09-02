using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TreeEditor;
using UnityEngine;
using UnityEngine.UI;

public class BlockCollision : BlockCollisionBase
{
    private ContactPoint[] _contactPoints = new ContactPoint[4];
    private List<Vector3> side = new List<Vector3>();
    [SerializeField] public Transform Block = null;
    [SerializeField] private Remainder _remainderPrefab = null;
    public Action<BlockCollision> EventNextBlock;

    private void OnCollisionEnter(Collision other)
    {
        var blockCollision = other.transform.GetComponent<BlockCollisionBase>();
        if (blockCollision == null)
            return;

        if(!blockCollision.IsCollision)
            return;
        
        if(!IsCollision)
            return;
        
        blockCollision.IsCollision = false;
        //IsCollision = false;
        
        var contacts = other.GetContacts(_contactPoints);
//        Debug.Log(contacts);
        foreach (ContactPoint contact in _contactPoints)
        {
            Debug.DrawRay(contact.point, contact.normal * 2, Color.red, 5);
        }
        //Debug.DrawLine(_contactPoints[0].point, _contactPoints[1].point, Color.green, 5f);
        //Debug.DrawLine(_contactPoints[0].point, _contactPoints[2].point, Color.green, 5f);
        //Debug.DrawLine(_contactPoints[0].point, _contactPoints[3].point, Color.green, 5f);
        
        side.Add(_contactPoints[0].point - _contactPoints[1].point);
        side.Add(_contactPoints[0].point - _contactPoints[2].point);
        side.Add(_contactPoints[0].point - _contactPoints[3].point);
        
        side.Sort((a, b) => a.magnitude.CompareTo(b.magnitude));
        
        Debug.DrawLine(_contactPoints[0].point, side[0], Color.green, 5f);
        Debug.DrawLine(_contactPoints[0].point, side[1], Color.green, 5f);

        var center = Vector3.Lerp(_contactPoints[0].point - side[0], _contactPoints[0].point - side[1], 0.5f);
        Debug.DrawLine(_contactPoints[0].point, center, Color.blue, 5f);

        var a1 = Vector3.Project(center - _contactPoints[0].point, side[0]).magnitude;
        var a2 = Vector3.Project(center - _contactPoints[0].point, side[1]).magnitude;
        //Debug.Log($"{a1} - {a2}");
        Debug.Log($"dot_sise_0: {Vector3.Dot(side[0].normalized, transform.forward)}");
        Debug.Log($"dot_side_1: {Vector3.Dot(side[1].normalized, transform.forward)}");

        Vector3 scale = Vector3.zero;
        Vector3 scale_remainder = Vector3.zero;
        Vector3 position_remainder = Vector3.zero;
        
        Vector3 scale_remainder_2 = Vector3.zero;
        Vector3 position_remainder_2 = Vector3.zero;

        var dot = Vector3.Dot(side[0].normalized, transform.forward);
        
        //Debug.Log($"dot_l: {dot_l}");
        if (dot == 0f)
        {
            Debug.Log($"-1-");
            scale = new Vector3(side[0].magnitude, Block.localScale.y, side[1].magnitude);
        
            var direction_litle = Vector3.Dot(side[1].normalized, Vector3.forward);
            var direction_large = Vector3.Dot(side[0].normalized, Vector3.left);
            
            Debug.Log($"direction_litle: {direction_litle}");
            Debug.Log($"direction_large: {direction_large}");
            
            scale_remainder = new Vector3(side[0].magnitude, Block.localScale.y, Block.localScale.z - side[1].magnitude);
            position_remainder = new Vector3(center.x, transform.position.y, _contactPoints[0].point.z + direction_litle * (Block.localScale.z - side[1].magnitude)/2f);
            
            scale_remainder_2 = new Vector3(Block.localScale.x - side[0].magnitude, Block.localScale.y, Block.localScale.z);
            position_remainder_2 = new Vector3(_contactPoints[0].point.x - direction_large * (Block.localScale.x - side[0].magnitude)/2f, transform.position.y, Block.position.z); 
        }
        else
        {
            Debug.Log($"-2-");
            scale = new Vector3(side[1].magnitude, Block.localScale.y, side[0].magnitude);

           // direction = dot == 1 ? -1 : 1;
            var direction_litle = Vector3.Dot(side[1].normalized, Vector3.left);
            var direction_large = dot;
            
            Debug.Log($"direction_litle: {direction_litle}");
            Debug.Log($"direction_large: {direction_large}");
            
            scale_remainder = new Vector3(Block.localScale.x - side[1].magnitude, Block.localScale.y, side[0].magnitude);
            position_remainder = new Vector3(_contactPoints[0].point.x - direction_litle * (Block.localScale.x - side[1].magnitude)/2f, transform.position.y, center.z);
            
            scale_remainder_2 = new Vector3(Block.localScale.x, Block.localScale.y, Block.localScale.z - side[0].magnitude);
            position_remainder_2 = new Vector3(Block.position.x, transform.position.y, _contactPoints[0].point.z + direction_large * (Block.localScale.z - side[0].magnitude)/2f); 
        }

        Block.position = center;
        Block.localScale = scale;
        Debug.Log($"scale: {scale}");
        
        var remainder = Instantiate(_remainderPrefab);
        remainder.Remainder_1.localScale = scale_remainder;
        remainder.Remainder_1.position = position_remainder;
        remainder.Remainder_2.localScale = scale_remainder_2;
        remainder.Remainder_2.position = position_remainder_2;

        remainder.Force((remainder.Remainder_2.position - Block.position).normalized);

        side.Clear();

        EventNextBlock?.Invoke(this);
        StartCoroutine(DestroyRemainder(remainder));
    }

    IEnumerator DestroyRemainder(Remainder _remainder)
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(_remainder.gameObject);
    }
}
