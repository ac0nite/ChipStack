﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Imphenzia;
using UnityEngine;

public enum StateGame
{
    NEXT_STAGE = 0,
    RESET_GAME,
    LOSE_ROUND,
    WIN_STATE
}

public class GameController : MonoBehaviour
{
    private Block _block = null;
    private BaseMovement _baseMovement = null;
    public static Action<Vector3> EventBlock;
    private Gradient _backgroundGradientColor = null;

    [SerializeField] private GradientSkyCamera _gradientSkyCamera;
    [SerializeField] private ParticleSystem _fxWin = null;
    [SerializeField] private AudioSource _audioWin = null;
    [HideInInspector] public bool IsWin = false;
    [SerializeField] public StateGame State = StateGame.NEXT_STAGE; 

    private void Awake()
    {
        _baseMovement = GameManager.Instance.Base.GetComponent<BaseMovement>();
    }
    void Start()
    {
        _backgroundGradientColor = new Gradient();
        //GameManager.Instance.Gradient.GenerateGradient();
        _backgroundGradientColor.SetKeys(GameManager.Instance.Gradient.getGradient().colorKeys, GameManager.Instance.Gradient.getGradient().alphaKeys);

        //GameManager.Instance.Gradient.GenerateGradient();

        GameManager.Instance.FogColor.FogColor = GameManager.Instance.Gradient.getGradient().colorKeys[1].color;

        //для скриншота на иконку
        //GameManager.Instance.BackgroundGroundFX.Play();
        //GameManager.Instance.FogColor.FogColor = new Color(111f/255f, 111f / 255f, 119f / 255f);
        //GameManager.Instance.Base.GetComponentInChildren<BlockColor>().SetColor(new Color(192f / 255f, 183f / 255f, 160f / 255f));
    }

    public void Go()
    {
        if (State == StateGame.LOSE_ROUND || State == StateGame.RESET_GAME)
            GameManager.Instance.ScoreManager.Total = 0;
            
        SpawnBlock(GameManager.Instance.BlockPrefab.Collision.transform);
        GameManager.Instance.AudioManager.StartMusicBackgroundPlaying();

        //GameManager.Instance.Gradient.GenerateGradient();
        _backgroundGradientColor.SetKeys(GameManager.Instance.Gradient.getGradient().colorKeys, GameManager.Instance.Gradient.getGradient().alphaKeys);
        //GameManager.Instance.FogColor.FogColor = GameManager.Instance.Gradient.getGradient().colorKeys[1].color;
        GameManager.Instance.FogColor.FogColor = GameManager.Instance.Gradient.RandomColor();
        //Debug.Log($"Fog Color: {GameManager.Instance.FogColor.FogColor}");

        GameManager.Instance.BackgroundGroundFX.Play();

        //GameManager.Instance.Gradient.GenerateGradient();
        GameManager.Instance.Base.GetComponentInChildren<BlockColor>().NextColor();

        State = StateGame.NEXT_STAGE;
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
            _fxWin.Play();
            _audioWin.Play();

            if (GameManager.Instance.ScoreManager.Stage == GameManager.Instance.ScoreManager.MaxStages)
            {
                State = StateGame.WIN_STATE;
                GameManager.Instance.ScoreManager.LimitBlocksInRound++;
            }

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

    public void OnExitRound()
    {
        if (GameManager.Instance.ScoreManager.Score != GameManager.Instance.ScoreManager.LimitBlocksInRound && State != StateGame.WIN_STATE)
        {
            GameManager.Instance.ScoreManager.Stage = 1;
            State = StateGame.LOSE_ROUND;
        }



        _block.Collision.EventNextBlock -= OnNextBlock;
        _block.Movement.EventExit -= OnExitRound;

        foreach (var remainder in GameManager.Instance.Remainders)
        {
            Destroy(remainder.gameObject);
        }

        GameManager.Instance.Remainders.Clear();

        GameManager.Instance.Base.GetComponentInChildren<BlockCollisionBase>().Collision = BlockCollisionBase.TypeCollision.Second;
        
        GameManager.Instance.BackgroundGroundFX.Stop();
        
        if(GameManager.Instance.ScoreManager.Score != GameManager.Instance.ScoreManager.LimitBlocksInRound)
            Destroy(_block.gameObject);

        StartCoroutine(WaitDel());
    }

    public void Reset()
    {
        State = StateGame.RESET_GAME;
        _block.Movement.Stop();
        GameManager.Instance.ScoreManager.Stage = 1;
        OnExitRound();
    }

    private IEnumerator WaitDel()
    {
        yield return new WaitForSeconds(0.7f);
        
        var movBase = GameManager.Instance.Base.GetComponent<BaseMovement>();
        var caсheSpeedLerp = movBase.SpeedLerp;
        movBase.SpeedLerp = 0.8f;
        _baseMovement.Target = Vector3.zero;
        
        var blocks = GameManager.Instance.Base.GetComponentsInChildren<Block>().ToList();
        for (int i = blocks.Count-1; i >= 0; i--)
        {
            Destroy(blocks[i].gameObject);
            yield return new WaitForSeconds(0.25f);
            if (State == StateGame.WIN_STATE)
                _fxWin.Play();
        }
        
        yield return new WaitForSeconds(0.5f);

        movBase.SpeedLerp = caсheSpeedLerp;
        
        //_baseMovement.Target = Vector3.zero;

        UIManager.Instance.ShowPanel(UITypePanel.StartScreen);
        GameManager.Instance.AudioManager.StartMusicBackground();
    }
}
