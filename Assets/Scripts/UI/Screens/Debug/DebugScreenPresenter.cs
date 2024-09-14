using System.Linq;
using Blocks;
using Core.UI.MVP;
using Gameplay;
using Intersections;
using Settings;
using UnityEngine;

namespace UI.Screens.Debug
{
    public class DebugScreenPresenter : ScreenPresenterBase<DebugScreen>
    {
        private readonly IBlockFacade _blockFacade;
        private InitialDropDebugAnimation initialAnimation;
        private DropAnimationDebug dropAnimation;
        private readonly Movement.Settings _gameplaySettings;

        public DebugScreenPresenter(DebugScreen view, IBlockFacade blockFacade) : base(view)
        {
            _blockFacade = blockFacade;
            _gameplaySettings = GameplaySettings.Instance.MovementSettings;
            
            _view.OnInitialButtonPressedEvent += InitialAnimation;
            _view.OnDownButtonPressedEvent += DropAnimation;
            _view.OnClearButtonPressedEvent += Clear;
        }

        protected override void Dispose()
        {
            _view.OnInitialButtonPressedEvent -= InitialAnimation;
            _view.OnDownButtonPressedEvent -= DropAnimation;
            _view.OnClearButtonPressedEvent -= Clear;
        }
        

        private void InitialAnimation()
        {
            Clear();
            
            var block = _blockFacade.BlockSpawn();
            initialAnimation ??= new InitialDropDebugAnimation();
            initialAnimation.SetComponents(block);
            initialAnimation.SetParams(Vector3.up * _blockFacade.BaseHeight);
            initialAnimation.Play(() => UnityEngine.Debug.Log("Initial animation done"));
        }
        
        private void DropAnimation()
        {
            dropAnimation ??= new DropAnimationDebug();
            Block block;
            if (_blockFacade.GetLastSpawned(1).Count == 0)
            {
                block = _blockFacade.BlockSpawn();
                block.Position = Vector3.up * _blockFacade.BaseHeight;
            }

            InitializeNextBlock();
            if (_blockFacade.IntersectionResolver.HasIntersect)
            {
                block = _blockFacade.LastBlockSpawned;
                dropAnimation.SetComponents(_blockFacade.GetLastSpawned(3).ToArray());
                dropAnimation.SetParams(_blockFacade.LastBlockSpawned.Position, DownPosition(_blockFacade.IntersectionResolver.Offset), _blockFacade.IntersectionResolver.Stretching);
                dropAnimation.Play(() =>
                {
                    var general = _blockFacade.IntersectionResolver.GeneralRect.ToIntersection(_blockFacade.LastBlockSpawned);
                    var intersections = _blockFacade.IntersectionResolver.RemaindersRect.ToIntersection(_blockFacade.LastBlockSpawned);
                    var stretching = _blockFacade.IntersectionResolver.Stretching;
                    _blockFacade.RemainderSpawn().Initialise(_blockFacade.LastBlockSpawned.View.PivotComponent, intersections, stretching).Enable();
                    _blockFacade.LastBlockSpawned.ChangeTransform(general);
                });

                Vector3 DownPosition(Vector2 offset)
                {
                    return new Vector3(
                        block.Position.x + offset.x, 
                        _blockFacade.BaseHeight, 
                        block.Position.z + offset.y);
                }
            }
            
            void InitializeNextBlock()
            {
                var preview = _blockFacade.LastBlockSpawned;
                block = _blockFacade.BlockSpawn();
                block.Size = preview.Size;
                var position = preview.Position;
                
                position.x += _view.DebugPosition.X;
                position.y += preview.Size.y + _gameplaySettings.Center.ShiftFromUp;
                position.z += _view.DebugPosition.Z;
                
                block.Position = position;
                
                UnityEngine.Debug.Log($"LAST: {block.View.transform.name} {block.View.transform.position} {block.Position}", block.View.transform);
            }
        }
        
        private void Clear()
        {
            _blockFacade.Clear();
        }
    }
}