using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletoneGameObject<GameManager>
{
    [SerializeField] public Transform Base = null;
    [SerializeField] public Block BlockPrefab = null;
}
