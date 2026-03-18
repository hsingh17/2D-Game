using System;
using UnityEngine;

public enum AlwaysPatrollingEnemyState
{
    MOVE_LEFT,
    MOVE_RIGHT,
};

public class AlwaysPatrollingEnemy : Enemy<AlwaysPatrollingEnemyState>
{
    protected override StateManager<AlwaysPatrollingEnemyState> StateManager => new();

    protected override void DoAction()
    {
        Move();
    }

    protected override void UpdateState()
    {
        Vector2 a = (Vector2)patrolPointA.transform.position;
        Vector2 b = (Vector2)patrolPointA.transform.position;
        Vector2 left = a.x < b.x ? a : b;
        Vector2 right = a.x > b.x ? a : b;

        // FIXME: the patrol points are moving with the enemy

        if (rb.position == left)
        {
            StateManager.CurrentState = AlwaysPatrollingEnemyState.MOVE_RIGHT;
        }
        else if (rb.position == right)
        {
            StateManager.CurrentState = AlwaysPatrollingEnemyState.MOVE_LEFT;
        }
    }

    protected void Move()
    {
        Vector2 xDir =
            StateManager.CurrentState == AlwaysPatrollingEnemyState.MOVE_LEFT
                ? Vector2.left
                : Vector2.right;
        Vector2 change = new(
            scriptableObject.speed * Time.fixedDeltaTime,
            GetSnapToGroundDistance()
        );
        Vector2 nextPosition = rb.position + (change * xDir);
        rb.MovePosition(nextPosition);
    }
}
