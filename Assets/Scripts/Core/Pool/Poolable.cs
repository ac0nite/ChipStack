using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Pool
{
    public class Poolable<TComponent> where TComponent : Component
    {
        private readonly Stack<TComponent> _pool;
        private readonly Transform _parent;
        private readonly TComponent _prefab;

        protected Poolable(TComponent prefab, int capacity)
        {
            _prefab = prefab;
            _parent = new GameObject($"{typeof(TComponent)}Pool").transform;
            _pool = new Stack<TComponent>(Enumerable.Range(0, capacity).Select(_ => Create()).Take(capacity));
        }

        private TComponent Create() => InternalInstantiate(Object.Instantiate(_prefab, _parent));

        protected virtual TComponent InternalInstantiate(TComponent view)
        {
            return view;
        }

        protected TComponent Get()
        {
            if(_pool.Count == 0)
                _pool.Push(Create());

            return _pool.Pop();
        }
    
        public void Release(TComponent component)
        {
            component.transform.SetParent(_parent);
            _pool.Push(component);
        }
    }
}