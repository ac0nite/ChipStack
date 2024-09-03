using System;
using Animations;
using Blocks;
using Components;
using UnityEngine;

public class InitialAnimation : GameplayAnimationBase
{
    private readonly InitialDropAnimation _settings;
    private readonly TweenAnimation _moving;
    private AnimationBlock _animation;
    public InitialAnimation()
    {
        _settings = _animationSettings.InitialDrop;
        _moving = TweenAnimation.CreateSimpleMove(_settings.Move);
    }
    
    public override GameplayAnimationBase SetComponents(params IAnimationComponent[] components)
    {
        _moving.Move.AnimComponent.SetComponent(components[0]);
        _animation = components[0].Animation;
        return this;
    }

    public override GameplayAnimationBase SetParams(params Vector3[] targets)
    {
        _moving.Move.AnimComponent.UpdateParams(_settings.BeginPosition, targets[0]);
        return this;
    }

    public override void Play(Action callback = null)
    {
        _animation.Play(_settings.DownAnimation);
        _moving.Play(() => _animation.Play(_settings.HitAnimation, callback));
    }
}