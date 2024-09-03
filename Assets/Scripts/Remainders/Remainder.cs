using System;
using Animations;
using Components;
using Core.Pool;
using Intersections;
using MEC;
using Pivots;
using Remainders;
using UnityEngine;

public class Remainder : IPresenter<RemainderView>, IAnimationComponent
{
    public Remainder(RemainderView view)
    {
        View = view;
        Animation = new AnimationBlock(view.Animator);
    }
    
    public RemainderView View { get; }
    public void SendToPool(Poolable<RemainderView> pool)
    {
        pool.Release(View);
    }

    public Remainder Initialise((Intersection one, Intersection two) intersection, Vector3 stretching)
    {
        intersection.one.ApplyTo(View.One);
        intersection.two.ApplyTo(View.Two);
        
        View.SetActive(intersection.one.IsValid, intersection.two.IsValid);
        
        var pivot = stretching.ToPivotTransform();
        ChangePivot(pivot.width, pivot.height);

        return this;
    }

    public void Enable()
    {
        View.Enable();
    }

    public void AddForce(Vector3 direction)
    {
        throw new System.NotImplementedException();
    }
    public void ClearAndDisable()
    {
        View.Reset();
    }

    public void CompletionOnFall(float delayTime, Action<Remainder> action)
    {
        Timing.CallDelayed(delayTime, () => action(this));
    }

    public Vector3 Position
    {
        get => View.Root.Position;
        set => View.Root.Position = value;
    }

    public Vector3 Size
    {
        get => View.Root.Size;
        set => View.Root.Size = value;
    }

    public AnimationBlock Animation { get; }
    public void ChangePivot(PivotTransform.PivotWidth pivotWidth, PivotTransform.PivotHeight pivotHeight)
    {
        View.Root.SetPivot(pivotWidth, pivotHeight);
        View.One.SetPivot(pivotWidth, pivotHeight);
        View.Two.SetPivot(pivotWidth, pivotHeight);
    }
}