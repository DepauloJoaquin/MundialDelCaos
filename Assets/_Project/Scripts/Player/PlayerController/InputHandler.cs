using UnityEngine;
using UnityEngine.InputSystem;

public abstract class InputHandler : MonoBehaviour
{
    [Header("Player Input/Animator Essentials")]
    public Animator animator;
    public PlayerStateManager playerStateManager;
    public PlayerInput pInput;
    public ControlSlot controlSlot = ControlSlot.Bot;
    public enum ControlSlot
    {
        None,
        Player1,
        Player2,
        Bot
    }

    [Header("Options")]
    public float verticalInput;
    public float horizontalInput;
    public bool isRunning;
    public bool selected;

    public abstract bool HasBall();

    public virtual void UpdateDirections()
    {
        if (pInput == null)
        {
            Debug.LogError(name + " no tiene PlayerInput.");
            ResetInput();
            return;
        }

        InputAction movementAction = pInput.actions.FindAction("Movement", false);
        InputAction runAction = pInput.actions.FindAction("Run", false);

        if (movementAction == null)
        {
            ResetInput();
            return;
        }

        Vector2 movement = movementAction.ReadValue<Vector2>();

        horizontalInput = movement.x;
        verticalInput = movement.y;

        isRunning = runAction != null && runAction.IsPressed();
    }

    protected void ResetInput()
    {
        horizontalInput = 0f;
        verticalInput = 0f;
        isRunning = false;
    }

    public virtual bool IsIdle()
    {
        return Mathf.Abs(horizontalInput) < 0.01f &&
               Mathf.Abs(verticalInput) < 0.01f;
    }
        public virtual bool IsRunning()
    {
        return isRunning;
    }

    public virtual bool IsMoving()
    {
        return !IsIdle();
    }

    public bool PassPressed()
    {
        if (!selected) return false;
        if (pInput == null) return false;

        InputAction passAction = pInput.actions.FindAction("PassTackle", false);

        if (passAction == null) return false;

        return passAction.IsPressed();
    }

    public bool ShootPressed()
    {
        if (!selected) return false;
        if (pInput == null) return false;

        InputAction shootAction = pInput.actions.FindAction("Shoot", false);

        if (shootAction == null) return false;

        return shootAction.IsPressed();
    }

   public bool AbilityPressed()
{
    if (!selected)
    {
        Debug.Log(name + " no está selected");
        return false;
    }

    if (pInput == null)
    {
        Debug.LogError(name + " no tiene PlayerInput asignado");
        return false;
    }

    InputAction abilityAction = pInput.actions.FindAction("Ability", false);

    if (abilityAction == null)
    {
        Debug.LogError("No existe la acción Ability en el Input Actions");
        return false;
    }

    if (abilityAction.IsPressed())
    {
        Debug.Log("Ability presionada");
        return true;
    }

    return false;
}

    public enum KeyPress
    {
        Pass,
        Tackle,
        Run,
        Ability,
        Shoot,
        None
    }
    public virtual void OnStateChanges(PlayerState newState)
    {
    }
}