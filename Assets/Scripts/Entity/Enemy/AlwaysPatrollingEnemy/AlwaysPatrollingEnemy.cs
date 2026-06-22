using System;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public enum AlwaysPatrollingEnemyState
{
    MoveLeft = -1,
    Default = 0,
    MoveRight = 1,
};

public class AlwaysPatrollingEnemy : Enemy<AlwaysPatrollingEnemyState>
{
    private readonly StateManager<AlwaysPatrollingEnemyState> stateManager = new();
    protected override StateManager<AlwaysPatrollingEnemyState> StateManager => stateManager;

    protected override void DoAction()
    {
        Move();
        UpdatePatrolPoint();
    }

    protected override void UpdateState()
    {
        if (patrolPoints.Length == 0)
        {
            return;
        }

        CircleCollider2D currentPatrolPoint = patrolPoints[currentPatrolPointIdx];
        Vector2 patrolPointPos = currentPatrolPoint.transform.position;
        Vector2 curPosition = rb.position;
        float dirX = patrolPointPos.x - curPosition.x;

        stateManager.CurrentState =
            dirX < 0 ? AlwaysPatrollingEnemyState.MoveLeft : AlwaysPatrollingEnemyState.MoveRight;
    }

    protected void Move()
    {
        float ySpeed = IsGrounded() ? GetSnapToGroundDistance() : GetGravityMovement();
        Debug.Log(IsGrounded());
        Vector2 change = new(
            scriptableObject.speed * Time.fixedDeltaTime * (int)StateManager.CurrentState,
            ySpeed
        );
        Vector2 nextPosition = rb.position + change;
        rb.MovePosition(nextPosition);
    }

    protected void UpdatePatrolPoint()
    {
        CircleCollider2D currentPatrolPoint = patrolPoints[currentPatrolPointIdx];
        float dist = Vector2.Distance(currentPatrolPoint.transform.position, rb.position);
        if (dist <= currentPatrolPoint.radius)
        {
            currentPatrolPointIdx = (currentPatrolPointIdx + 1) % patrolPoints.Length;
        }
    }
}
