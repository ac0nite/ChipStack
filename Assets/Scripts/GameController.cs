using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Imphenzia;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private Block _block = null;
    private BaseMovement _baseMovement = null;
    public static Action<Vector3> EventBlock;
    private Gradient _backgroundGradientColor = null;

    [SerializeField] private GradientSkyCamera _gradientSkyCamera;

    private void Awake()
    {
        _baseMovement = GameManager.Instance.Base.GetComponent<BaseMovement>();
    }
    void Start()
    {
        _backgroundGradientColor = new Gradient();
        GameManager.Instance.Gradient.GenerateGradient();
        _backgroundGradientColor.SetKeys(GameManager.Instance.Gradient.getGradient().colorKeys, GameManager.Instance.Gradient.getGradient().alphaKeys);

        GameManager.Instance.Gradient.GenerateGradient();

        GameManager.Instance.FogColor.FogColor = GameManager.Instance.Gradient.getGradient().colorKeys[1].color;

        //для скриншота на иконку
        //GameManager.Instance.BackgroundGroundFX.Play();
        //GameManager.Instance.FogColor.FogColor = new Color();
    }

    public void Go()
    {
        SpawnBlock(GameManager.Instance.BlockPrefab.Collision.transform);
        GameManager.Instance.AudioManager.StartMusicBackgroundPlaying();

        GameManager.Instance.Gradient.GenerateGradient();
       
        _backgroundGradientColor.SetKeys(GameManager.Instance.Gradient.getGradient().colorKeys, GameManager.Instance.Gradient.getGradient().alphaKeys);
        GameManager.Instance.FogColor.FogColor = GameManager.Instance.Gradient.getGradient().colorKeys[1].color;

        GameManager.Instance.BackgroundGroundFX.Play();

        GameManager.Instance.Gradient.GenerateGradient();
        GameManager.Instance.Base.GetComponentInChildren<BlockColor>().NextColor();
    }

    private void Update()
    {
        _gradientSkyCamera.gradient = GameManager.Instance.Gradient.Lerp(_gradientSkyCamera.gradient,_backgroundGradientColor, Time.deltaTime * 0.5f);
    }

    private void SpawnBlock(Transform _transform)
    {
        _block = Instantiate(GameManager.Instance.BlockPrefab);

        _block.Collision.transform.localScale = _transform.localScale;

        _block.Movement.Init(_transform);

        _block.Collision.EventNextBlock += OnNextBlock;
        _block.Movement.EventExit += OnExitRound;

        EventBlock?.Invoke(_transform.position);
    }

    private void OnNextBlock(BlockCollision block)
    {
        GameManager.Instance.ScoreManager.ModifyScore(+1, block.transform.localScale.x * block.transform.localScale.z);

        if (GameManager.Instance.ScoreManager.Score == GameManager.Instance.ScoreManager.LimitBlocksInRound)
        {
            OnExitRound();
            GameManager.Instance.ScoreManager.Stage += 1;
            return;
        }

        //if (GameManager.Instance.ScoreManager.Score != 0 && GameManager.Instance.ScoreManager.Score % 2 == 0)
        //    GameManager.Instance.FogColor.FogColor = GameManager.Instance.Gradient.RandomColor();

        _baseMovement.Target =
            (GameManager.Instance.Base.transform.position + Vector3.down * block.transform.localScale.y);

        _block.Collision.EventNextBlock -= OnNextBlock;
        _block.Movement.EventExit -= OnExitRound;

        SpawnBlock(block.transform);
    }

    private void OnExitRound()
    {
        Debug.Log("OnExitRound");

        _block.Collision.EventNextBlock -= OnNextBlock;
        _block.Movement.EventExit -= OnExitRound;

        foreach (var remainder in GameManager.Instance.Remainders)
        {
            Destroy(remainder.gameObject);
        }

        GameManager.Instance.Remainders.Clear();

        GameManager.Instance.Base.GetComponentInChildren<BlockCollisionBase>().Collision = BlockCollisionBase.TypeCollision.Second;
        
        GameManager.Instance.BackgroundGroundFX.Stop();

        StartCoroutine(WaitDel());
    }

    private IEnumerator WaitDel()
    {
        var blocks = GameManager.Instance.Base.GetComponentsInChildren<Block>().ToList();
        for (int i = blocks.Count-1; i >= 0; i--)
        {
            Destroy(blocks[i].gameObject);
            yield return new WaitForSeconds(1f);
        }
        
        _baseMovement.Target = Vector3.zero;

        yield return new WaitForSeconds(0.5f);

        UIManager.Instance.ShowPanel(UITypePanel.StartScreen);
        GameManager.Instance.AudioManager.StartMusicBackground();
    }
}
