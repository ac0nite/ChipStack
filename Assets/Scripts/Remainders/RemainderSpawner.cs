using System;
using Core.Pool;
using Remainders;

public class RemainderSpawner : SpawnerBase<RemainderView, Remainder>
{
    public RemainderSpawner(RemainderView prefab, int capacity, Func<RemainderView, Remainder> factory) : base(prefab, capacity, factory)
    {
    }

    protected override RemainderView InternalInstantiate(RemainderView view)
    {
        view.Reset();
        return base.InternalInstantiate(view);
    }

    protected override Remainder Initialise(Remainder remainder)
    {
        remainder.Enable();
        return remainder;
    }

    protected override Remainder Clear(Remainder remainder)
    {
        remainder.ClearAndDisable();
        return remainder;
    }
}