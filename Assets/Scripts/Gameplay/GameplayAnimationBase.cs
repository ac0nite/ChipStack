using System;
using Animations;
using Blocks;
using Settings;
using UnityEngine;

public abstract class GameplayAnimationBase
{
    protected static readonly AnimationSettings _animationSettings = GameplaySettings.Instance.AnimationSettings;

    public abstract GameplayAnimationBase SetBlocks(params Block[] blocks);
    public abstract GameplayAnimationBase SetParams(params Vector3[] targets);
    public abstract void Play(Action callback = null);
}