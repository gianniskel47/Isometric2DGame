using Unity.VisualScripting;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    private float maxTimeIdling;
    private float minTimeIdling;
    private float timeToIdle;
    private float counter = 0;

    //passing the values we need through the constructor from the player
    // I could also set them directly in here but  I could not modify them since
    // this is not monobehavior so the values were going to be hard coded
    public EnemyIdleState(EnemyAI enemyAI, EnemyStateMachine enemyStateMachine, EnemyAnimationController enemyAnimationController) : base(enemyAI, enemyStateMachine, enemyAnimationController)
    {
        maxTimeIdling = enemyAI.MaxTimeIdling;
        minTimeIdling = enemyAI.MinTimeIdling;
    }

    public override void EnterState()
    {
        //imobilizing enemy and playing animation
        // randomize the time to sit idle
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
        // if the time to sit idle is reached change to patrol state and so move to the next point
        if(counter >= timeToIdle)
        {
            enemyAI.StateMachine.ChangeState(enemyAI.PatrolState);
            return;
        }
        enemyAI.MoveEnemy(new Vector2(0, 0), 0);
    }
}
