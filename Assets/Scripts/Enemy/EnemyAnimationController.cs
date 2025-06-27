using System;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private const string IDLE = "Idle_";
    private const string WALK = "Walk_";
    private const string ATTACK = "Attack_";
    private const string TAKE_DAMAGE = "TakeDamageEnemy";
    private const string ABILITY = "Ability_";
    private const string DEATH = "Death_";

    Animator animator;

    private EnemyAI enemyAI;
    private Action callback;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyAI = GetComponentInParent<EnemyAI>();
    }

    #region ANIMATIONS
    public void PlayIdleAnim()
    {
        animator.Play(IDLE + DirToString(enemyAI.LastMoveDirection));
    }

    public void PlayPatrolAnim()
    {
        animator.Play(WALK + DirToString(enemyAI.LastMoveDirection));
    }

    public void PlayAttackAnim(Action abilityEndedCallback)
    {
        callback = abilityEndedCallback;
        // normalized time var because if this the active animation already
        // then if I call it again it wont play from the start by default.
        animator.Play(ATTACK + DirToString(enemyAI.LastMoveDirection), 0, 0f);
    }

    public void PlayDeathAnim()
    {
        animator.Play(DEATH + DirToString(enemyAI.LastMoveDirection));
    }

    public void PlayAbilityAnim(Action abilityEndedCallback)
    {
        callback = abilityEndedCallback;
        animator.Play(ABILITY + DirToString(enemyAI.LastMoveDirection));
    }

    // anim event when enemy using the ability
    public void DoAoeDamageOnAbilityAttack()
    {
        enemyAI.DoAoeDamage();
    }

    // anim event when the enemy finished the ability
    public void OnAbilityEnded()
    {
        callback?.Invoke();
        callback = null;
    }

    // anim event when the enemy finished normal attack
    public void OnNormalAttackEnded()
    {
        callback?.Invoke();
        callback = null;
    }

    public void PlayTakeDamageAnim()
    {
        animator.Play(TAKE_DAMAGE);
    }

    // anim event when enemy attacking
    public void RegisterDamageOnAttack()
    {
        if (enemyAI.DetectedPlayer != null)
        {
            enemyAI.DetectedPlayer.GetComponent<PlayerController>().TakeDamage(enemyAI.AttackDamage);
        }
    }
    #endregion

    private string DirToString(int direction)
    {
        switch (direction)
        {
            case 0: return "SW";
            case 1: return "NE";
            case 2: return "NW";
            case 3: return "SE";
            default: return "NE";
        }
    }
}
