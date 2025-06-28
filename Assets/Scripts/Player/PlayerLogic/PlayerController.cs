using UnityEngine;

// this script is responsible for the player's logic
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
    [SerializeField] SO_VoidEventChannel OnPlayerUpdateHealth;
    [SerializeField] SO_VoidEventChannel OnPlayerDied;

    private int currentHealth;
    private float attackTimer = 0f;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private CircleCollider2D playerCollider;
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
        gameInput.AttackEvent -= OnAttackInput;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animationController = GetComponentInChildren<AnimationController>();
        playerCollider = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        currentHealth = maxHealth;

        OnPlayerUpdateHealth.RaiseEvent(currentHealth);
    }

    private void OnMoveInput(Vector2 input)
    {
        moveInput = input;
    }

    private void OnAttackInput()
    {
        // if he is attacking he cannot attack again on top of that
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
            //cooldown on attacks
            rb.linearVelocity = Vector2.zero;
            attackTimer -= Time.fixedDeltaTime;
            if (attackTimer <= 0)
            {
                isAttacking = false; //attacking is over
                attackTimer = attackDuration;
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

    // player loses health
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animationController.PlayTakeDamageAnim();

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Death();
        }

        OnPlayerUpdateHealth.RaiseEvent(currentHealth);
    }

    // called when player dies
    private void Death()
    {
        playerCollider.isTrigger = true;
        transform.tag = "Untagged";
        isDead = true;
        animationController.PlayDeathAnim();
        OnPlayerDied.RaiseEvent();
    }

    //slow effect
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("plant"))
        {
            isSlowed = true;
        }
    }

    //slow effect
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("plant"))
        {
            isSlowed = true;
        }
    }

    //slow effect
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("plant"))
        {
            isSlowed = false;
        }
    }
}
