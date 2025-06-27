using Unity.VisualScripting;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    private float maxTimeIdling;
    private float minTimeIdling;
    private float timeToIdle;
    private float counter = 0;

    public EnemyIdleState(EnemyAI enemyAI, EnemyStateMachine enemyStateMachine, EnemyAnimationController enemyAnimationController) : base(enemyAI, enemyStateMachine, enemyAnimationController)
    {
        maxTimeIdling = enemyAI.MaxTimeIdling;
        minTimeIdling = enemyAI.MinTimeIdling;
    }

    public override void EnterState()
    {
        enemyAnimationController.PlayIdleAnim();
        enemyAI.MoveEnemy(new Vector2(0, 0), 0);
        timeToIdle = Random.Range(minTimeIdling, maxTimeIdling);
        counter = 0;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void Update()
    {
        if (enemyAI.IsChasing)
        {
            enemyAI.StateMachine.ChangeState(enemyAI.ChaseState);
            return;
        }

        if(enemyAI.CurrentHealth <= 0)
        {
            enemyAI.StateMachine.ChangeState(enemyAI.DeathState);
            return;
        }

        counter += Time.deltaTime;
        if(counter >= timeToIdle)
        {
            enemyAI.StateMachine.ChangeState(enemyAI.PatrolState);
            return;
        }
        enemyAI.MoveEnemy(new Vector2(0, 0), 0);
    }
}
