using System;
using Blocks;
using UnityEngine;

namespace Gameplay
{
    public class PreparingGameplay
    {
        private readonly IBlockFacade _blockFacade;
        private readonly InitialAnimation _animation;

        public PreparingGameplay(IBlockFacade facade)
        {
            _blockFacade = facade;
            _animation = new InitialAnimation();
        }

        public void Run(Action callback)
        {
            _animation
                .SetBlocks(_blockFacade.BlockSpawn())
                .SetParams(Vector3.up * _blockFacade.BaseHeight)
                .Play(callback);
        }
    }
}