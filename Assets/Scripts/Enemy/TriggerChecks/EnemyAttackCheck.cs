using UnityEngine;

public class EnemyAttackCheck : MonoBehaviour
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
            enemyAI.SetIsAttacking(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemyAI.SetIsAttacking(false);
        }
    }
}
