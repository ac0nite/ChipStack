namespace Core.FSM.Base
{
    public interface IStateMachine
    {
        void NextState<T>() where T : IState;
    }
}