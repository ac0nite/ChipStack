using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Pool
{
    public interface IPresenter<TComponent> where TComponent : Component
    {
        TComponent View { get; }

        void SendToPool(Poolable<TComponent> pool);
    }
    public abstract class SpawnerBase<TComponent, TPresenter> : Poolable<TComponent>
        where TPresenter : IPresenter<TComponent>
        where TComponent : Component
    {
        private List<TPresenter> _spawned;
        private readonly Func<TComponent, TPresenter> _factory;

        protected SpawnerBase(TComponent prefab, int capacity, Func<TComponent, TPresenter> factory) : base(prefab, capacity)
        {
            _spawned = new List<TPresenter>(capacity);
            _factory = factory;
        }
    
        public IReadOnlyList<TPresenter> Spawned => _spawned;

        public TPresenter Spawn()
        {
            var presenter = _factory(Get());
            _spawned.Add(presenter);
            return Initialise(presenter);
        }

        public void Despawn(TPresenter presenter)
        {
            InternalClear(presenter);
            _spawned.Remove(presenter);
        }

        private void InternalClear(TPresenter presenter)
        {
            Clear(presenter).SendToPool(this);
        }
    
        public void DespawnAll()
        {
            foreach (var presenter in _spawned)
                InternalClear(presenter);
            _spawned.Clear();
        }

        protected abstract TPresenter Initialise(TPresenter presenter);
        protected abstract TPresenter Clear(TPresenter presenter);
    }   
}