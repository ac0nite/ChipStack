using System;
using System.Collections.Generic;
using System.Linq;
using Intersections;
using Remainders;
using UnityEngine;

namespace Blocks
{
    public interface IBlockFacade
    {
        Block BlockSpawn();
        Block LastBlockSpawned { get; }
        Remainder RemainderSpawn();
        IntersectionResolver IntersectionResolver { get; }
        float BaseHeight { get; }
        IReadOnlyList<Block> GetLastSpawned(int count);
        void Clear();
    }

    public class BlockFacade : IBlockFacade
    {
        private readonly BlockSpawner _blockSpawner;
        private readonly RemainderSpawner _remainderSpawner;
        private readonly float _stepHeight;
        private readonly Vector3 _defaultPosition;
        private readonly Vector3 _defaultBlockSize;

        public BlockFacade(Settings settings)
        {
            _defaultBlockSize = settings.DefaultBlockSize;
            _blockSpawner = new BlockSpawner(settings.BlockPrefab, settings.BlockPoolCapacity, view => new Block(view));
            _remainderSpawner = new RemainderSpawner(settings.RemainderPrefab, settings.RemainderPoolCapacity, view => new Remainder(view));

            IntersectionResolver = new IntersectionResolver(settings.IntersectionSettings);
            
            _stepHeight = settings.BlockPrefab.transform.lossyScale.y;
            BaseHeight = -_stepHeight * 0.5f;
            _defaultPosition = Vector3.up * BaseHeight;
        }
    
        public Block BlockSpawn()
        {
            LastBlockSpawned = _blockSpawner.Spawn();
            LastBlockSpawned.Size = _defaultBlockSize;
            IntersectionResolver.Add(LastBlockSpawned);
            BaseHeight += _stepHeight;
            return LastBlockSpawned;
        }

        public Block LastBlockSpawned { get; private set; }

        public Remainder RemainderSpawn() => _remainderSpawner.Spawn();
        public IntersectionResolver IntersectionResolver { get; private set; }

        public IReadOnlyList<Block> GetLastSpawned(int count)
        {
            var all = _blockSpawner.Spawned.Count;
            return _blockSpawner.Spawned
                .Skip(Math.Max(all - count, 0))
                .Take(Math.Min(count, all))
                .Reverse()
                .ToArray();
        }

        public void Clear()
        {
            IntersectionResolver.Clear();
            _blockSpawner.DespawnAll();
            _remainderSpawner.DespawnAll();
            BaseHeight = _defaultPosition.y;
        }

        public float BaseHeight { get; private set; }
        
        public List<Block> GetSpawned(int count) => _blockSpawner.Spawned.GetRange(0, count);

        [Serializable]
        public struct Settings
        {
            public BlockView BlockPrefab;
            public int BlockPoolCapacity;
            public Vector3 DefaultBlockSize;
            public RemainderView RemainderPrefab;
            public int RemainderPoolCapacity;
            public IntersectionResolver.Settings IntersectionSettings;
        }
    }
}