using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] Animator animator;

    [Header("Config")]
    [SerializeField] int maxHealth;
    
    [Header("Patrol State Variables")]
    [SerializeField] Transform[] patrolPoints;
    [SerializeField] float patrolSpeed = 1f;
    public float PatrolSpeed { get => patrolSpeed; }
    public Transform[] PatrolPoints { get => patrolPoints; }

    [Header("Ability Variables")]
    [SerializeField] float aoeRadius = 2f;
    [SerializeField] int abilityDamage = 7;
    [SerializeField] float abilityCooldownMin = 2f;
    [SerializeField] float abilityCooldownMax = 4f;
    public float AbilityCooldownMin { get => abilityCooldownMin; }
    public float AbilityCooldownMax { get => abilityCooldownMax; }


    [Header("Attack State Variables")]
    [SerializeField] int attackDamage = 3;
    [SerializeField] float attackCooldown = 3f;
    public int AttackDamage { get => attackDamage; }
    public float AttackCooldown { get => attackCooldown; }


    [Header("Chase State Variables")]
    [SerializeField] float chaseSpeed = 2f;
    public float ChaseSpeed { get => chaseSpeed; }


    [Header("Idle State Variables")]
    [SerializeField] float maxTimeIdling = 6f;
    [SerializeField] float minTimeIdling = 2f;
    public float MaxTimeIdling { get => maxTimeIdling; }
    public float MinTimeIdling { get => minTimeIdling; }


    [Header("State Machine Variables")]
    public EnemyStateMachine StateMachine { get; set; }
    public EnemyIdleState IdleState { get; set; }
    public EnemyChaseState ChaseState { get; set; }
    public EnemyAttackState AttackState { get; set; }
    public EnemyPatrolState PatrolState { get; set; }
    public EnemyDeathState DeathState { get; set; }


    [Header("Broadcasting to")]
    [SerializeField] SO_VoidEventChannel OnEnemyUpdateHealth;
    [SerializeField] SO_VoidEventChannel OnEnemyDied;

    public bool IsChasing { get; set; }
    public bool IsAttacking { get; set; }
    public Transform DetectedPlayer { get; set; }
    public int LastMoveDirection { get; set; }
    public int CurrentHealth { get; set; }

    private Rigidbody2D rb;
    private EnemyAnimationController enemyAnimationController;
    private CircleCollider2D enemyCollider;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyAnimationController = GetComponentInChildren<EnemyAnimationController>();
        enemyCollider = GetComponent<CircleCollider2D>();

        StateMachine = new EnemyStateMachine();
        IdleState = new EnemyIdleState(this, StateMachine, enemyAnimationController);
        ChaseState = new EnemyChaseState(this, StateMachine, enemyAnimationController);
        AttackState = new EnemyAttackState(this, StateMachine, enemyAnimationController);
        PatrolState = new EnemyPatrolState(this, StateMachine, enemyAnimationController);
        DeathState = new EnemyDeathState(this, StateMachine, enemyAnimationController);
    }

    private void Start()
    {
        //set the starting state
        StateMachine.Initialize(IdleState);
        CurrentHealth = maxHealth;

        OnEnemyUpdateHealth.RaiseEvent(CurrentHealth);
    }

    private void Update()
    {
        StateMachine.CurrentEnemyState.Update();
    }

    public void MoveEnemy(Vector2 dir, float speed)
    {
        rb.linearVelocity = dir * speed;
        if(dir.sqrMagnitude > 0.01f && speed > 0.01f)
        {
            int newDir = GetIsoDirection(dir);
            if(newDir != LastMoveDirection)
            {
                // setting this to help with the animations to face the right direction
                // just like what we did with the player
                LastMoveDirection = GetIsoDirection(dir);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        enemyAnimationController.PlayTakeDamageAnim();

        if(CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            Death();
        }

        OnEnemyUpdateHealth.RaiseEvent(CurrentHealth);
    }

    // ability logic to deal AOE damage on a circle around him
    public void DoAoeDamage()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, aoeRadius);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                hit.GetComponent<PlayerController>().TakeDamage(abilityDamage);
            }
        }
    }

    private void Death()
    {
        enemyCollider.enabled = false;
        OnEnemyDied.RaiseEvent();
    }

    // helping function to convert the input to the correcponding orientation
    // to assign to "LastMoveDirection" variable to be used for animations etc
    public int GetIsoDirection(Vector2 dir)
    {
        if (dir == Vector2.zero) return LastMoveDirection;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle = (angle + 360) % 360;
        if (angle >= 315 || angle < 45)
        {
            return 0;    // SW
        }

        if (angle >= 45 && angle < 135)
        {
            return 1;    // NE
        }

        if (angle >= 135 && angle < 225)
        {
            return 2;   // NW
        }

        if (angle >= 225 && angle < 315)
        {
            return 3;   // SE
        }
        return 0;
    }

    public void SetIsChasing(bool isChasing)
    {
        IsChasing = isChasing;
    }

    public void SetIsAttacking(bool isAttacking)
    {
        IsAttacking = isAttacking;
    }

    // helping function to visualize the AOE ability damage range
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aoeRadius);
    }
}
