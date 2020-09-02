using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private Block _block = null;
    void Start()
    {
        SpawnBlock(GameManager.Instance.BlockPrefab.Collision.Block.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnBlock(Transform _transform)
    {
        _block = Instantiate(GameManager.Instance.BlockPrefab);
        _block.transform.SetParent(GameManager.Instance.Base.transform);

        // block.transform.position = _transform.position;
        _block.Collision.Block.transform.localScale = _transform.localScale;
        
        _block.Collision.EventNextBlock += OnNextBlock;
        _block.Movement.EventExit += OnExitRound;
    }

    private void OnNextBlock(BlockCollision block)
    {
        //Vector3.down * (_nextBlock.transform.localScale.y)
        Debug.Log($"new scale: {block.Block.transform.localScale}");
        GameManager.Instance.Base.transform.position += Vector3.down * block.Block.localScale.y;
        _block.Collision.EventNextBlock -= OnNextBlock;
        _block.Movement.EventExit -= OnExitRound;
        SpawnBlock(block.Block);
    }

    private void OnExitRound()
    {
        Debug.Log($"OnExitRound");
        _block.Collision.EventNextBlock -= OnNextBlock;
        _block.Movement.EventExit -= OnExitRound;
        var blocks = GameManager.Instance.Base.GetComponentsInChildren<Block>().ToList();
        foreach (var block in blocks)
        {
            Destroy(block.gameObject);
        }
        SpawnBlock(GameManager.Instance.BlockPrefab.Collision.Block.transform);
        GameManager.Instance.Base.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        GameManager.Instance.Base.GetComponentInChildren<BlockCollisionBase>().IsCollision = true;
    }
}
