using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private Block _block = null;
    private BaseMovement _baseMovement = null;
    public static Action<Vector3> EventBlock;

    private void Awake()
    {
        _baseMovement = GameManager.Instance.Base.GetComponent<BaseMovement>();
    }
    void Start()
    {
        //SpawnBlock(GameManager.Instance.BlockPrefab.Collision.transform);
    }

    public void Go()
    {
        SpawnBlock(GameManager.Instance.BlockPrefab.Collision.transform);
        GameManager.Instance.AudioManager.StartMusicBackgroundPlaying();
    }

    private void SpawnBlock(Transform _transform)
    {
        _block = Instantiate(GameManager.Instance.BlockPrefab);
       // _block.transform.SetParent(GameManager.Instance.Base.transform);

        // block.transform.position = _transform.position;
        _block.Collision.transform.localScale = _transform.localScale;

        _block.Movement.Init(_transform);


        _block.Collision.EventNextBlock += OnNextBlock;
        _block.Movement.EventExit += OnExitRound;

        EventBlock?.Invoke(_transform.position);
    }

    private void OnNextBlock(BlockCollision block)
    {
        //Vector3.down * (_nexttransform.localScale.y)
        //Debug.Log($"spawn new  scale:{block.transform.localScale} position: {block.transform.position}");

        GameManager.Instance.ScoreManager.ModifyScore(+1);

        _baseMovement.Target =
            (GameManager.Instance.Base.transform.position + Vector3.down * block.transform.localScale.y);
        //GameManager.Instance.Base.transform.position += Vector3.down * block.transform.localScale.y;
        _block.Collision.EventNextBlock -= OnNextBlock;
        _block.Movement.EventExit -= OnExitRound;
        SpawnBlock(block.transform);
    }

    private void OnExitRound()
    {
        Debug.Log($"OnExitRound");
        //GameManager.Instance.ScoreManager.ModifyScore(0);
        _block.Collision.EventNextBlock -= OnNextBlock;
        _block.Movement.EventExit -= OnExitRound;
        var blocks = GameManager.Instance.Base.GetComponentsInChildren<Block>().ToList();
        foreach (var block in blocks)
        {
            Destroy(block.gameObject);
        }
        Destroy(_block.gameObject);
        //SpawnBlock(GameManager.Instance.BlockPrefab.Collision.transform);

        foreach (var remainder in GameManager.Instance.Remainders)
        {
            Destroy(remainder.gameObject);
        }
        GameManager.Instance.Remainders.Clear();
        _baseMovement.Target = Vector3.zero;
        
        GameManager.Instance.Base.GetComponentInChildren<BlockCollisionBase>().Collision = BlockCollisionBase.TypeCollision.Second;

        GameManager.Instance.Gradient.GenerateGradient();
        GameManager.Instance.Base.GetComponentInChildren<BlockColor>().NextColor();
        GameManager.Instance.Base.GetComponentInChildren<BackGroundPlaneColor>().NextColor();

        UIManager.Instance.ShowPanel(UITypePanel.StartScreen);

        GameManager.Instance.AudioManager.StartMusicBackground();
    }
}
