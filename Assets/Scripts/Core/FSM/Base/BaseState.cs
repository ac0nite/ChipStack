namespace Core.FSM.Base
{
    public class BaseState : IState
    {
        public virtual void Enter() {}
        public virtual void Update(float deltaTime) {}
        public virtual void Exit() {}
    }
}