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
        Vector3 CenterPosition { get; }
        void Clear();
    }

    public class BlockFacade : IBlockFacade
    {
        private readonly BlockSpawner _blockSpawner;
        private readonly RemainderSpawner _remainderSpawner;
        private readonly Vector3 _stepPosition;
        private readonly Vector3 _defaultPosition;
        private readonly Vector3 _defaultBlockSize;

        public BlockFacade(Settings settings)
        {
            _defaultBlockSize = settings.DefaultBlockSize;
            _blockSpawner = new BlockSpawner(settings.BlockPrefab, settings.BlockPoolCapacity, view => new Block(view));
            _remainderSpawner = new RemainderSpawner(settings.RemainderPrefab, settings.RemainderPoolCapacity, view => new Remainder(view));
            _stepPosition = Vector3.up * settings.BlockPrefab.transform.lossyScale.y;
            
            Intersection = new BlocksIntersection(settings.IntersectionSettings);
            CenterPosition = _defaultPosition = -_stepPosition * 0.5f;
        }
    
        public Block BlockSpawn()
        {
            LastBlockSpawned = _blockSpawner.Spawn();
            LastBlockSpawned.Size = _defaultBlockSize;
            Intersection.Add(LastBlockSpawned);
            CenterPosition += _stepPosition;
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
            CenterPosition = _defaultPosition;
        }

        public Vector3 CenterPosition { get; private set; }

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