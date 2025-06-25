using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] SO_GameInput gameInput;
    [SerializeField] float maxSpeed = 1.5f;
    [SerializeField] float slowedSpeed = 0.5f;

    private Vector2 moveInput;
    private Rigidbody2D rb;
    private float moveSpeed = 0f;
    private bool isSlowed = false;

    private void OnEnable()
    {
        gameInput.MoveEvent += OnMoveInput;
    }

    private void OnDisable()
    {
        gameInput.MoveEvent -= OnMoveInput;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnMoveInput(Vector2 input)
    {
        moveInput = input;
    }

    private void FixedUpdate()
    {
        Vector2 isoMove = Vector2.zero;
        isoMove += moveInput.y * new Vector2(1,1).normalized;
        isoMove += moveInput.x * new Vector2(1, -1).normalized;

        moveSpeed = isSlowed ? slowedSpeed : maxSpeed;
        rb.linearVelocity = isoMove.normalized * moveSpeed;
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
