using System;

public class StateManager<T>
    where T : Enum
{
    public delegate void OnStateUpdate(T newState);
    public static event OnStateUpdate onStateUpdate;

    private T currentState;

    public T CurrentState
    {
        get { return currentState; }
        set
        {
            currentState = value;
            onStateUpdate?.Invoke(currentState);
        }
    }
}
