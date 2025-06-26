using UnityEngine;

public class EnemyAttackState : EnemyState
{
    private int damage;
    private float attackCooldown;
    private float counter = 0f;

    public EnemyAttackState(EnemyAI enemyAI, EnemyStateMachine enemyStateMachine) : base(enemyAI, enemyStateMachine)
    {
        damage = enemyAI.AttackDamage;
        attackCooldown = enemyAI.AttackCooldown;
    }

    public override void EnterState()
    {
        enemyAI.PlayAttackAnim();
        counter = 0f;
        enemyAI.MoveEnemy(new Vector2(0, 0), 0);
        Debug.Log("ATTACKK DA PLAYER");
    }

    public override void ExitState()
    {
    }

    public override void Update()
    {
        enemyAI.MoveEnemy(new Vector2(0, 0), 0);

        if (enemyAI.IsChasing && !enemyAI.IsAttacking)
        {
            enemyAI.StateMachine.ChangeState(enemyAI.ChaseState);
            return;
        }

        counter += Time.deltaTime;

        if (counter >= attackCooldown)
        {
            enemyAI.PlayAttackAnim();
            Debug.Log("ATTACKK DA PLAYER");
            counter = 0f;
        }
    }
}
