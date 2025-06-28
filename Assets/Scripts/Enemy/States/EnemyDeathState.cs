using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class EnemyDeathState : EnemyState
{
    public EnemyDeathState(EnemyAI enemyAI, EnemyStateMachine enemyStateMachine, EnemyAnimationController enemyAnimationController) : base(enemyAI, enemyStateMachine, enemyAnimationController)
    {

    }

    public override void EnterState()
    {
        //imobilize enemy, play animation
        enemyAI.MoveEnemy(new Vector2(0, 0), 0);
        enemyAnimationController.PlayDeathAnim();
    }

    public override void Update()
    {
        // cannot transition from this state to any other state.
        enemyAI.MoveEnemy(new Vector2(0, 0), 0);
    }
}
