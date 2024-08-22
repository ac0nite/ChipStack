using Blocks;
using Core.UI.MVP;
using Gameplay;
using UnityEngine;

namespace UI.Screens.Debug
{
    public class DebugScreenPresenter : ScreenPresenterBase<DebugScreen>
    {
        private readonly IBlockFacade _blockFacade;
        private InitialDropDebugAnimation animation;

        public DebugScreenPresenter(DebugScreen view, IBlockFacade blockFacade) : base(view)
        {
            _blockFacade = blockFacade;
            
            _view.OnInitialButtonPressedEvent += InitialAnimation;
            _view.OnDownButtonPressedEvent += DownAnimation;
            _view.OnClearButtonPressedEvent += Clear;
        }

        protected override void Dispose()
        {
            _view.OnInitialButtonPressedEvent -= InitialAnimation;
            _view.OnDownButtonPressedEvent -= DownAnimation;
            _view.OnClearButtonPressedEvent -= Clear;
        }
        

        private void InitialAnimation()
        {
            Clear();
            
            var block = _blockFacade.BlockSpawn();
            animation ??= new InitialDropDebugAnimation();
            animation.SetBlocks(block);
            animation.SetParams(Vector3.up * _blockFacade.BaseHeight);
            animation.Play(() => UnityEngine.Debug.Log("Initial animation done"));
        }
        
        private void DownAnimation()
        {
            Clear();
        }
        
        private void Clear()
        {
            _blockFacade.Clear();
        }
    }
}