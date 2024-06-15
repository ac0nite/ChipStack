using System;
using Core.Pool;
using Intersections;
using MEC;
using Remainders;
using UnityEngine;

public class Remainder : IPresenter<RemainderView>
{
    public Remainder(RemainderView view)
    {
        View = view;
    }
    
    public RemainderView View { get; }
    public void SendToPool(Poolable<RemainderView> pool)
    {
        pool.Release(View);
    }

    public Remainder Initialise((Intersection one, Intersection two) intersection)
    {
        intersection.one.ApplyTo(View.OneRemainder);
        intersection.two.ApplyTo(View.TwoRemainder);
        
        View.OneRemainder.gameObject.SetActive(intersection.one.IsValid);
        View.TwoRemainder.gameObject.SetActive(intersection.two.IsValid);

        View.Component.EnableRenderers();
        View.Component.EnablePhysics();
        
        return this;
    }

    public void Enable()
    {
        View.Component.EnableActive();
    }
    public void ClearAndDisable()
    {
        View.Component.DisablePhysics();
        View.Component.SetTransformDefault();
        View.Component.DisableActive();
    }

    public void CompletionOnFall(float delayTime, Action<Remainder> action)
    {
        Timing.CallDelayed(delayTime, () => action(this));
    }
}