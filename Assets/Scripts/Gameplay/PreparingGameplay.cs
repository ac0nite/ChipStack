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
            
            Debug.Log($"Run!");
            _animation
                .SetComponents(_blockFacade.BlockSpawn())
                .SetParams(Vector3.up * _blockFacade.BaseHeight)
                .Play(callback);
        }
    }
}