using Unity.VisualScripting;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    private float maxTimeIdling;
    private float minTimeIdling;
    private float timeToIdle;
    private float counter = 0;

    public EnemyIdleState(EnemyAI enemyAI, EnemyStateMachine enemyStateMachine) : base(enemyAI, enemyStateMachine)
    {
        maxTimeIdling = enemyAI.MaxTimeIdling;
        minTimeIdling = enemyAI.MinTimeIdling;
    }

    public override void EnterState()
    {
        enemyAI.PlayIdleAnim();
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
        }

        counter += Time.deltaTime;
        if(counter >= timeToIdle)
        {
            enemyAI.StateMachine.ChangeState(enemyAI.PatrolState);
        }
        enemyAI.MoveEnemy(new Vector2(0, 0), 0);
    }
}
