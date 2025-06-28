using UnityEngine;

public class EnemyChaseState : EnemyState
{
    private Transform playerTransform;
    private float chaseSpeed;

    //passing the values we need through the constructor from the player
    // I could also set them directly in here but  I could not modify them since
    // this is not monobehavior so the values were going to be hard coded
    public EnemyChaseState(EnemyAI enemyAI, EnemyStateMachine enemyStateMachine, EnemyAnimationController enemyAnimationController) : base(enemyAI, enemyStateMachine, enemyAnimationController)
    {
        chaseSpeed = enemyAI.ChaseSpeed;
    }

    public override void EnterState()
    {
        //start the animation
        enemyAnimationController.PlayPatrolAnim();
        //hold the player to chase him
        playerTransform = enemyAI.DetectedPlayer;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void Update()
    {
        if (enemyAI.IsAttacking)
        {
            enemyAI.StateMachine.ChangeState(enemyAI.AttackState);
            return;
        }

        if(enemyAI.CurrentHealth <= 0)
        {
            enemyAI.StateMachine.ChangeState(enemyAI.DeathState);
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
