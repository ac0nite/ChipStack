using System;
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
        BlocksIntersection Intersection { get; }
        float BaseHeight { get; }
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

            Intersection = new BlocksIntersection(settings.IntersectionSettings);
            
            _stepHeight = settings.BlockPrefab.transform.lossyScale.y;
            BaseHeight = -_stepHeight * 0.5f;
            _defaultPosition = Vector3.up * BaseHeight;
        }
    
        public Block BlockSpawn()
        {
            LastBlockSpawned = _blockSpawner.Spawn();
            LastBlockSpawned.Size = _defaultBlockSize;
            Intersection.Add(LastBlockSpawned);
            BaseHeight += _stepHeight;
            return LastBlockSpawned;
        }

        public Block LastBlockSpawned { get; private set; }

        public Remainder RemainderSpawn() => _remainderSpawner.Spawn();
        public BlocksIntersection Intersection { get; private set; }

        public void Clear()
        {
            Intersection.Clear();
            _blockSpawner.DespawnAll();
            _remainderSpawner.DespawnAll();
            BaseHeight = _defaultPosition.y;
        }

        public float BaseHeight { get; private set; }

        [Serializable]
        public struct Settings
        {
            public BlockView BlockPrefab;
            public int BlockPoolCapacity;
            public Vector3 DefaultBlockSize;
            public RemainderView RemainderPrefab;
            public int RemainderPoolCapacity;
            public BlocksIntersection.Settings IntersectionSettings;
        }
    }
}