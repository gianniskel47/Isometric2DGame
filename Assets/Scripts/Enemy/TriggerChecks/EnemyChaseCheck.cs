using UnityEngine;

public class EnemyChaseCheck : MonoBehaviour
{
    private EnemyAI enemyAI;

    private void Awake()
    {
        enemyAI = GetComponentInParent<EnemyAI>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemyAI.DetectedPlayer = collision.transform;
            enemyAI.SetIsChasing(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemyAI.SetIsChasing(false);
            enemyAI.DetectedPlayer = null;
        }
    }
}
