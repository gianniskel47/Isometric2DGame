using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private const string ISO_DIRECTION = "IsoDirection";
    private const string SPEED = "Speed";
    private const string TAKE_DAMAGE = "TakeDamage";

    [SerializeField] Animator animator;
    [SerializeField] GameObject arrowPrefab;

    private int lastDirIndex = 0;
    private Vector2 lastMoveVector = Vector2.right;

    enum IsoDir { NE , NW, SE, SW };

    public void PlayWalkAnim(Vector2 input)
    {
        float speed = input.magnitude;

        if(speed > 0.01f)
        {
            int dirIndex = GetIsoDirection(input);
            lastDirIndex = dirIndex;
            lastMoveVector = input.normalized;
        }
        animator.SetFloat(ISO_DIRECTION, lastDirIndex);
        animator.SetFloat(SPEED, speed);
    }

    public void PlayIdleAnim()
    {
        animator.SetFloat(ISO_DIRECTION, lastDirIndex);
        animator.SetFloat(SPEED, 0f);
    }

    public void PlayAttackAnim()
    {
        animator.SetFloat(ISO_DIRECTION, lastDirIndex);
        animator.SetTrigger("Attack");
    }

    public void PlayDeathAnim()
    {
        animator.SetFloat(ISO_DIRECTION, lastDirIndex);
        animator.SetTrigger("Death");
    }

    public void PlayTakeDamageAnim()
    {
        animator.SetTrigger(TAKE_DAMAGE);
    }

    private int GetIsoDirection(Vector2 input)
    {
        float angle = Mathf.Atan2(input.y , input.x) * Mathf.Rad2Deg;
        angle = (angle + 360) % 360;

        if (angle >= 315 || angle < 45)
        {
            return (int)IsoDir.SE;
        }
        if (angle >= 45 && angle < 135) 
        {
            return (int)IsoDir.NE;
        }
        
        if (angle >= 125 && angle < 225)
        {
            return (int)IsoDir.NW;
        }

        if (angle >= 225 && angle < 315)
        {
            return (int)IsoDir.SW;
        }
        return 0;
    }

    // anim event at bow shoot
    public void SpawnArrow()
    {
        float angle = Mathf.Atan2(lastMoveVector.y, lastMoveVector.x) * Mathf.Rad2Deg;
        GameObject arrowInstance = Instantiate(arrowPrefab, transform.position, Quaternion.Euler(0,0,angle + 140));
        arrowInstance.GetComponent<ArrowProjectile>().SetDirection(lastMoveVector);
    }
}
