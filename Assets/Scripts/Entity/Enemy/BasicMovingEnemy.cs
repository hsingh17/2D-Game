using System;

public enum BasicMovingEnemyState
{
    MOVE_LEFT,
    MOVE_RIGHT,
};

public class BasicMovingEnemy : Enemy<BasicMovingEnemyState>
{
    protected override StateManager<BasicMovingEnemyState> StateManager => new();

    protected override void DoAction()
    {
        throw new NotImplementedException();
    }

    protected override void UpdateState()
    {
        throw new NotImplementedException();
    }
}
