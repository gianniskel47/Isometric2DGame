using UnityEngine;

public class EnemyAttackState : EnemyState
{
    private float attackCooldown;
    private float normalAttackCounter = 0f;
    private float currentAbilityCooldown;
    private float abilityCooldownMin;
    private float abilityCooldownMax;
    private float abilityCounter = 0f;
    private bool isNormalAttacking = false;
    private bool isPerformingAbility = false;
    private bool isFirstTimeAttacking = true; // we need this to check if its the 1st time attacking
                                              // so the 1st time it goes on EnterState the enemy should 
                                              //attack immediately. then the counter handles it

    public EnemyAttackState(EnemyAI enemyAI, EnemyStateMachine enemyStateMachine, EnemyAnimationController enemyAnimationController) : base(enemyAI, enemyStateMachine, enemyAnimationController)
    {
        attackCooldown = enemyAI.AttackCooldown;
        abilityCooldownMax = enemyAI.AbilityCooldownMax;
        abilityCooldownMin = enemyAI.AbilityCooldownMin;
    }

    public override void EnterState()
    {
        currentAbilityCooldown = Random.Range(abilityCooldownMin, abilityCooldownMax);
        enemyAI.MoveEnemy(new Vector2(0, 0), 0);
        enemyAnimationController.PlayIdleAnim();

        if (isFirstTimeAttacking)
        {
            Debug.Log("AAAA");
            enemyAnimationController.PlayAttackAnim(() => NormalAttackFinished());
            isFirstTimeAttacking = false;
        }
    }

    public override void ExitState()
    {
    }

    public override void Update()
    {
        enemyAI.MoveEnemy(new Vector2(0, 0), 0);

        if (enemyAI.IsChasing && !enemyAI.IsAttacking && !isPerformingAbility)
        {
            enemyAI.StateMachine.ChangeState(enemyAI.ChaseState);
            return;
        }

        // in case the player is dead
        if (!enemyAI.DetectedPlayer.CompareTag("Player"))
        {
            enemyAI.IsAttacking = false;
            enemyAI.IsChasing = false;
            enemyAI.StateMachine.ChangeState(enemyAI.IdleState);
            return;
        }

        // even though the ability anim is playing we can transition instantly on death without checking when its finished
        if(enemyAI.CurrentHealth <= 0)
        {
            enemyAI.StateMachine.ChangeState(enemyAI.DeathState);
            return;
        }

        normalAttackCounter += Time.deltaTime;
        abilityCounter += Time.deltaTime;

        // prioritize ability
        if (abilityCounter >= currentAbilityCooldown && !isNormalAttacking)
        {
            isPerformingAbility = true;
            // making a callback so the bool isPerformingAbility changes pixel perfect on time
            enemyAnimationController.PlayAbilityAnim(() => AbilityFinished());
            currentAbilityCooldown = Random.Range(abilityCooldownMin, abilityCooldownMax);
            abilityCounter = 0;

            // reset normal attack cooldown to prevent double attack
            normalAttackCounter = 0f;
            return;
        }

        if (normalAttackCounter >= attackCooldown && !isPerformingAbility)
        {
            isNormalAttacking = true;
            enemyAnimationController.PlayAttackAnim(() => NormalAttackFinished());
            normalAttackCounter = 0f;
        }
    }

    // we need to keep track of the ability animation
    // so it doesnt stop when the player gets out of range
    // (changing state to chase)
    private void AbilityFinished()
    {
        isPerformingAbility = false;
    }

    private void NormalAttackFinished()
    {
        isNormalAttacking = false;
    }
}
