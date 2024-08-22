using System;
using Animations;
using Blocks;
using Settings;
using UnityEngine;

public abstract class GameplayAnimationBase
{
    protected readonly AnimationSettings _animationSettings = GameplaySettings.Instance.AnimationSettings;
    protected readonly SequenceWrapper _sequence = new SequenceWrapper();
    public abstract GameplayAnimationBase SetBlocks(params Block[] blocks);
    public abstract GameplayAnimationBase SetParams(params Vector3[] targets);
    public abstract void Play(Action callback = null);
}