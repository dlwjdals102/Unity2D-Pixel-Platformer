using UnityEngine;

public class Enemy_ReaperBattleState : Enemy_BattleState 
{
    private Enemy_Reaper enemyReaper;

    public Enemy_ReaperBattleState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        enemyReaper = enemy as Enemy_Reaper;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemyReaper.maxBattleIdleTime;
    }

    public override void Update()
    {
        stateTimer -= Time.deltaTime;
        UpdateAnimationParameters();

        if (stateTimer < 0)
            stateMachine.ChangeState(enemyReaper.reaperTeleportState);

        if (enemy.PlayerDetected())
            UpdateBattleTimer();

        if (WithinAttackRange() && enemy.PlayerDetected())
            stateMachine.ChangeState(enemyReaper.reaperAttackState);
        else
        {
            float xVelocity = enemy.groundDetected ? enemy.battleMoveSpeed : 0.0001f;
            enemy.SetVelocity(xVelocity * DirectionToPlayer(), rb.linearVelocity.y);
        }
    }
}
