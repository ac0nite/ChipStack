using Blocks;
using Cysharp.Threading.Tasks;
using UI;

namespace FSM.States
{
    public class ResetState : GameplayBaseState
    {
        private readonly IBlockFacade _blockFacade;
        private readonly StatesMachineModel _stateMachine;

        public ResetState(GameplayContext context) : base(context)
        {
            _blockFacade = context.BlockFacade;
            _stateMachine = context.StatesMachineModel;
        }

        public override void Enter()
        {
            ScreenManager
                .Loading
                .Execute(Clear)
                .OnCompleted(() => _stateMachine.ChangeState<PreGameplayState>())
                .Show();
        }

        private async UniTask Clear()
        {
            _blockFacade.Clear();
            await UniTask.Delay(1000);
        }
    }   
}