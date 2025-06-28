using UnityEngine;

public class EnemyPatrolState : EnemyState
{
    private Transform[] patrolPoints;
    private float patrolSpeed;
    private Transform currentTarget;

    //passing the values we need through the constructor from the player
    // I could also set them directly in here but  I could not modify them since
    // this is not monobehavior so the values were going to be hard coded
    public EnemyPatrolState(EnemyAI enemyAI, EnemyStateMachine enemyStateMachine, EnemyAnimationController enemyAnimationController) : base(enemyAI, enemyStateMachine, enemyAnimationController)
    {
        patrolPoints = enemyAI.PatrolPoints;
        patrolSpeed = enemyAI.PatrolSpeed;
    }

    public override void EnterState()
    {
        // the first time its going to enter find a random point
        if (currentTarget != null) return;

        currentTarget = patrolPoints[Random.Range(0, patrolPoints.Length)];
    }

    public override void ExitState()
    {
        
    }

    public override void Update()
    {
        if (enemyAI.IsChasing)
        {
            enemyAI.StateMachine.ChangeState(enemyAI.ChaseState);
            return;
        }

        if (enemyAI.CurrentHealth <= 0)
        {
            enemyAI.StateMachine.ChangeState(enemyAI.DeathState);
            return;
        }

        Vector2 dir = (currentTarget.transform.position - enemyAI.transform.position).normalized;
        enemyAI.MoveEnemy(dir, patrolSpeed);
        enemyAnimationController.PlayPatrolAnim();

        if (Vector2.Distance(enemyAI.transform.position, currentTarget.position) < 0.2f)
        {
            enemyAI.StateMachine.ChangeState(enemyAI.IdleState);
            Transform nextPoint;
            do
            {
                nextPoint = patrolPoints[Random.Range(0, patrolPoints.Length)];
            } while (nextPoint == currentTarget && patrolPoints.Length > 1);
            currentTarget = nextPoint;
        }
    }
}
