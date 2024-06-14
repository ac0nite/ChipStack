using System;

namespace Gameplay
{
    public interface IRoundGameplayEvents
    {
        event Action OnEnterRoundEvent;
        event Action OnNextBlockEvent;
        event Action OnDownBlockEvent;
        event Action<int> OnIntersectionAreaEvent;
        event Action<int> OnReplayEvent;
        event Action OnExitRoundEvent;
    }
}