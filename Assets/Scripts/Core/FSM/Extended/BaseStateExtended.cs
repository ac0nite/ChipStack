using System.Collections.Generic;

namespace Core.FSM.Extended
{
    public class BaseStateExtended : IStateExtended
    {
        private readonly List<ITransition> _transitions = new();
        public virtual void Enter() {}

        public virtual void Update(float deltaTime) {}

        public virtual void Exit() {}
        public void AddTransitions(params ITransition[] transitions)
        {
            _transitions.AddRange(transitions);
        }

        public IReadOnlyList<ITransition> Transitions => _transitions;
        public virtual bool IsCompleted { get; } = false;
    }
}