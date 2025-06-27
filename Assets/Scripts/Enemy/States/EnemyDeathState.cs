using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class EnemyDeathState : EnemyState
{
    public EnemyDeathState(EnemyAI enemyAI, EnemyStateMachine enemyStateMachine, EnemyAnimationController enemyAnimationController) : base(enemyAI, enemyStateMachine, enemyAnimationController)
    {

    }

    public override void EnterState()
    {
        enemyAI.MoveEnemy(new Vector2(0, 0), 0);
        enemyAnimationController.PlayDeathAnim();
    }

    public override void Update()
    {
        enemyAI.MoveEnemy(new Vector2(0, 0), 0);
    }
}
