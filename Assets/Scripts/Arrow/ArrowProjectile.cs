using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] int damage;

    private Rigidbody2D rb;
    private Vector2 dir;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();    
    }

    private void Update()
    {
        if(dir != null)
        {
            rb.linearVelocity = dir * speed;
        }
    }

    // when spawning we are giving the correct direction the player is facing
    public void SetDirection(Vector2 dir)
    {
        this.dir = dir;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyAI>().TakeDamage(damage);
            Destroy(gameObject);
        }

        //kill the arrow if it goes out of bounds
        if (collision.CompareTag("arrowKiller"))
        {
            Destroy(gameObject);
        }
    }
}
