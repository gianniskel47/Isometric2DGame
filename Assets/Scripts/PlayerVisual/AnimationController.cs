using Unity.VisualScripting;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private const string ISO_DIRECTION = "IsoDirection";
    private const string SPEED = "Speed";

    [SerializeField] Animator animator;
    [SerializeField] float deadZone = 0.1f;
    [SerializeField] SO_GameInput gameInput;

    private Vector2 input;
    private int lastDirIndex = 0;

    enum IsoDir { NE , NW, SE, SW }

    private void OnEnable()
    {
        gameInput.MoveEvent += OnMoveInput;
    }

    private void OnDisable()
    {
        gameInput.MoveEvent -= OnMoveInput;
    }

    private void Update()
    {
       float speed = input.magnitude;
       int dirIndex = lastDirIndex;

        if(speed > 0.01)
        {
            dirIndex = GetIsoDirection(input);
            lastDirIndex = dirIndex;
        }

        animator.SetFloat(ISO_DIRECTION, dirIndex);
        animator.SetFloat(SPEED, speed);
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

    private void OnMoveInput(Vector2 input)
    {
        this.input = input;
    }
}
