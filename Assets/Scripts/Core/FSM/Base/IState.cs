namespace Core.FSM.Base
{
    public interface IState
    {
        void Enter();
        void Update(float deltaTime);
        void Exit();
    }
}