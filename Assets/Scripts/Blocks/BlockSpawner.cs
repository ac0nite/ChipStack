using System;
using System.Collections.Generic;
using Core.Pool;

namespace Blocks
{
    public class BlockSpawner : SpawnerBase<BlockView, Block>
    {
        public BlockSpawner(BlockView prefab, int capacity, Func<BlockView, Block> factory) : base(prefab, capacity, factory)
        {
        }

        protected override BlockView InternalInstantiate(BlockView view)
        {
            view.Reset();
            return base.InternalInstantiate(view);
        }

        protected override Block Initialise(Block presenter)
        {
            presenter.Enable();
            return presenter;
        }

        protected override Block Clear(Block presenter)
        {
            presenter.ClearAndDisable();
            return presenter;
        }

        public List<Block> Spawned => _spawned;
    }
}