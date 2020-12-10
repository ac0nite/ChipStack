using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class GameManager : SingletoneGameObject<GameManager>
{
    [SerializeField] public Transform Base = null;
    [SerializeField] public Block BlockPrefab = null;
    [SerializeField] public Transform Center = null;
    [SerializeField] public ScoreManager ScoreManager = null;
    [SerializeField] public GameController GameController = null;
    [SerializeField] public AudioManager AudioManager = null;
    [SerializeField] public GradientManager1 Gradient = null;
    public List<Remainder> Remainders = new List<Remainder>();
    [SerializeField] public ParticleSystem BackgroundGroundFX = null;
    [SerializeField] public UpdateFogColor FogColor = null;
    [SerializeField] public List<PropertyStage> Property;
}
