using UnityEngine;

public class EnemyChaseState : EnemyState
{
    private Transform playerTransform;
    private float chaseSpeed;

    public EnemyChaseState(EnemyAI enemyAI, EnemyStateMachine enemyStateMachine) : base(enemyAI, enemyStateMachine)
    {
        chaseSpeed = enemyAI.ChaseSpeed;
    }

    public override void EnterState()
    {
        enemyAI.PlayPatrolAnim();
        playerTransform = enemyAI.DetectedPlayer;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void Update()
    {
        Debug.Log("IS CHASING");
        if (enemyAI.IsAttacking)
        {
            enemyAI.StateMachine.ChangeState(enemyAI.AttackState);
            return;
        }

        if (!enemyAI.IsChasing)
        {
            enemyAI.StateMachine.ChangeState(enemyAI.IdleState);
            return;
        }
        Vector2 dir = (playerTransform.position - enemyAI.transform.position).normalized;
        enemyAI.MoveEnemy(dir, chaseSpeed);
    }
}
