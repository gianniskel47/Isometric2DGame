using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "SO_GameInput", menuName = "Scriptable Objects/SO_GameInput")]
public class SO_GameInput : ScriptableObject, InputSystem_Actions.IPlayerActions
{
    public event UnityAction<Vector2> MoveEvent = delegate { };
    public event UnityAction AttackEvent = delegate { };
    public event UnityAction UseAbilityEvent = delegate { };

    private InputSystem_Actions gameInput;

    private void OnEnable()
    {
        if(gameInput == null)
        {
            gameInput = new InputSystem_Actions();
            gameInput.Player.SetCallbacks(this);
        }

        gameInput.Enable();
    }

    private void OnDisable()
    {
        gameInput.Disable();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            AttackEvent?.Invoke();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent.Invoke(context.ReadValue<Vector2>());
    }
}
