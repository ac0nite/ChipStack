using Core.FSM.Base;
using Settings;

namespace FSM.States
{
    public class GameplayBaseState : BaseState
    {
        protected readonly GameplayContext _context;
        protected readonly GameplaySettings _settings;

        protected GameplayBaseState(GameplayContext context)
        {
            _context = context;
            _settings = GameplaySettings.Instance;
        }
    }   
}