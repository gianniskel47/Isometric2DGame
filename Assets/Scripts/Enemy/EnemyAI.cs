using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] Animator animator;

    [Header("Patrol State Variables")]
    [SerializeField] Transform[] patrolPoints;
    [SerializeField] float patrolSpeed = 1f;
    public float PatrolSpeed { get => patrolSpeed; }
    public Transform[] PatrolPoints { get => patrolPoints; }


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

    public bool IsChasing { get; set; }
    public bool IsAttacking { get; set; }
    public Transform DetectedPlayer { get; set; }
    public int LastMoveDirection { get; set; }

    private Rigidbody2D rb;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        StateMachine = new EnemyStateMachine();
        IdleState = new EnemyIdleState(this, StateMachine);
        ChaseState = new EnemyChaseState(this, StateMachine);
        AttackState = new EnemyAttackState(this, StateMachine);
        PatrolState = new EnemyPatrolState(this, StateMachine);
    }

    private void Start()
    {
        StateMachine.Initialize(IdleState);
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
                LastMoveDirection = GetIsoDirection(dir);
            }
        }
    }

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

    public void PlayIdleAnim()
    {
        animator.Play("Idle_" + DirToString(LastMoveDirection));
    }

    public void PlayPatrolAnim()
    {
        animator.Play("Walk_" + DirToString(LastMoveDirection));
    }

    public void PlayAttackAnim()
    {
        // normalized time var because if this the active animation already
        // then if I call it again it wont play from the start by default.
        animator.Play("Attack_" + DirToString(LastMoveDirection), 0, 0f);
    }

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
