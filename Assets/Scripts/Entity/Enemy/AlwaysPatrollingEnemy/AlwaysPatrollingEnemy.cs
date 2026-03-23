using System;
using UnityEngine;

public enum AlwaysPatrollingEnemyState
{
    MOVE_LEFT = -1,
    INITIAL_STATE = 0,
    MOVE_RIGHT = 1,
};

public class AlwaysPatrollingEnemy : Enemy<AlwaysPatrollingEnemyState>
{
    private readonly StateManager<AlwaysPatrollingEnemyState> stateManager = new();
    protected override StateManager<AlwaysPatrollingEnemyState> StateManager => stateManager;

    protected override void DoAction()
    {
        Move();
    }

    protected override void UpdateState()
    {
        Vector2 left = (Vector2)leftPatrolPoint.transform.position;
        Vector2 right = (Vector2)rightPatrolPoint.transform.position;
        float distLeft = Vector2.Distance(left, rb.position);
        float distRight = Vector2.Distance(right, rb.position);

        if (StateManager.CurrentState == AlwaysPatrollingEnemyState.INITIAL_STATE)
        {
            if (distLeft < distRight)
            {
                StateManager.CurrentState = AlwaysPatrollingEnemyState.MOVE_LEFT;
            }
            else
            {
                StateManager.CurrentState = AlwaysPatrollingEnemyState.MOVE_RIGHT;
            }
        }
        else if (
            StateManager.CurrentState == AlwaysPatrollingEnemyState.MOVE_LEFT
            && distLeft < leftPatrolPoint.radius
        )
        {
            StateManager.CurrentState = AlwaysPatrollingEnemyState.MOVE_RIGHT;
        }
        else if (
            StateManager.CurrentState == AlwaysPatrollingEnemyState.MOVE_RIGHT
            && distRight < rightPatrolPoint.radius
        )
        {
            StateManager.CurrentState = AlwaysPatrollingEnemyState.MOVE_LEFT;
        }
    }

    protected void Move()
    {
        Vector2 change = new(
            scriptableObject.speed * Time.fixedDeltaTime * (int)StateManager.CurrentState,
            GetSnapToGroundDistance()
        );
        // TODO: Think of a way for all entities to be subjected to gravity or snap to ground

        Vector2 nextPosition = rb.position + change;
        rb.MovePosition(nextPosition);
    }
}
