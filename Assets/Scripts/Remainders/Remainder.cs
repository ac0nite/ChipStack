using System;
using Components;
using Core.Pool;
using Intersections;
using MEC;
using Remainders;
using UnityEngine;
using RectTransform = Intersections.RectTransform;

public class Remainder : IPresenter<RemainderView>, IComponent
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

    public Remainder Initialise((RectTransform one, RectTransform two) intersection)
    {
        intersection.one.ApplyTo(View.OneRemainder);
        intersection.two.ApplyTo(View.TwoRemainder);
        
        View.OneRemainder.gameObject.SetActive(intersection.one.IsValid);
        View.TwoRemainder.gameObject.SetActive(intersection.two.IsValid);

        return this;
    }

    public void Enable()
    {
        View.Component.EnableActive();
    }

    public void AddForce(Vector3 direction)
    {
        View.Component.AddForce(direction);
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

    public Vector3 Position
    {
        get => View.transform.position;
        set => View.transform.position = value;
    }

    public Vector3 Size
    {
        get => View.transform.localScale;
        set => View.transform.localScale = value;
    }
}