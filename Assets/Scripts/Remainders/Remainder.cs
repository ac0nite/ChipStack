using Animations;
using Components;
using Core.Pool;
using Cysharp.Threading.Tasks;
using Intersections;
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

    public Remainder Initialise(IPivotExtended root, (Intersection one, Intersection two) intersection, Vector3 stretching)
    {
        View.Root.Position = root.Position;
        View.Root.Size = root.Size;
        
        intersection.one.ApplyTo(View.One, View.Root.Size);
        intersection.two.ApplyTo(View.Two, View.Root.Size);
        
        View.SetActive(intersection.one.IsValid, intersection.two.IsValid);
        
        var pivot = stretching.ToPivotAlignment();
        ChangePivot(pivot.width, pivot.height);

        _ = ChangePivotScaleAsync(root.Pivot.localScale);
        
        return this;
    }

    private async UniTask ChangePivotScaleAsync(Vector3 scale)
    {
        View.PivotsAdjuster.Enable();
        await UniTask.DelayFrame(1);
        View.Root.Pivot.localScale = scale;
        View.PivotsAdjuster.Disable();
        await UniTask.DelayFrame(1);
        View.Root.FineTunePivotAlignment();
        View.One.FineTunePivotAlignment();
        View.Two.FineTunePivotAlignment();
    }

    public void Enable()
    {
        View.Enable();
    }
    
    public IPivotsAdjuster PivotsAdjuster => View.PivotsAdjuster;

    public void AddForce(Vector3 direction)
    {
        throw new System.NotImplementedException();
    }
    public void ClearAndDisable()
    {
        View.Reset();
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

    public bool IsActive => View.Root.IsActive;

    public AnimationBlock Animation { get; }
    public void ChangePivot(PivotComponent.WidthAlignment widthAlignment, PivotComponent.HeightAlignment heightAlignment)
    {
        View.Root.SetPivotAlignment(widthAlignment, heightAlignment);
        View.One.SetPivotAlignment(widthAlignment, heightAlignment);
        View.Two.SetPivotAlignment(widthAlignment, heightAlignment);
    }
}