using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] SO_GameInput gameInput;

    [Header("Config")]
    [SerializeField] float maxSpeed = 1.5f;
    [SerializeField] float slowedSpeed = 0.5f;
    [SerializeField] float attackDuration = 0.5f;
    [SerializeField] int maxHealth = 8;

    [Header("Broadcasting to")]
    [SerializeField] SO_VoidEventChannel OnPlayerTakeDamage;
    [SerializeField] SO_VoidEventChannel OnPlayerDied;

    private int currentHealth;
    private float attackTimer = 0f;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private CircleCollider2D collider;
    private float moveSpeed = 0f;
    private bool isSlowed;
    private bool isAttacking;
    private AnimationController animationController;
    private bool isDead;

    private void OnEnable()
    {
        gameInput.MoveEvent += OnMoveInput;
        gameInput.AttackEvent += OnAttackInput;
    }

    private void OnDisable()
    {
        gameInput.MoveEvent -= OnMoveInput;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animationController = GetComponentInChildren<AnimationController>();
        collider = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        currentHealth = maxHealth;

        OnPlayerTakeDamage.RaiseEvent(currentHealth);
    }

    private void OnMoveInput(Vector2 input)
    {
        moveInput = input;
    }

    private void OnAttackInput()
    {
        if (isAttacking) return;

        isAttacking = true;
        attackTimer = attackDuration;
        animationController.PlayAttackAnim();
    }

    private void FixedUpdate()
    {
        if (isDead) return;

        if (isAttacking)
        {
            rb.linearVelocity = Vector2.zero;
            attackTimer -= Time.fixedDeltaTime;
            if (attackTimer <= 0)
            {
                isAttacking = false; //attacking is over
            }
            return;
        }

        Vector2 isoMove = Vector2.zero;
        isoMove += moveInput.y * new Vector2(1,1).normalized;
        isoMove += moveInput.x * new Vector2(1, -1).normalized;

        moveSpeed = isSlowed ? slowedSpeed : maxSpeed;
        rb.linearVelocity = isoMove.normalized * moveSpeed;

        if (moveInput.sqrMagnitude > 0.01f)
        {
            animationController.PlayWalkAnim(moveInput);
        }
        else
        {
            animationController.PlayIdleAnim();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animationController.PlayTakeDamageAnim();

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Death();
        }

        OnPlayerTakeDamage.RaiseEvent(currentHealth);
    }

    private void Death()
    {
        collider.isTrigger = true;
        transform.tag = "Untagged";
        isDead = true;
        animationController.PlayDeathAnim();
        OnPlayerDied.RaiseEvent();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("plant"))
        {
            isSlowed = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("plant"))
        {
            isSlowed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("plant"))
        {
            isSlowed = false;
        }
    }
}
