using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : InputHandler
{
    public TeamController _myTeamController;
    public Team team;
    public Rigidbody2D rigidBody;
    public SpriteRenderer spriteRenderer;
    public GoalTarget _targetGoal;
    public Team _team;
    public Role _role;
    public float _movementSpeed = 3.5f;
    public Vector2 _movementDirection;
    public Vector2 _spawnPosition;
    public Vector2 _position;
    public float _forceTowardsTheBall;
    public ControlSlot controlSlot = ControlSlot.Bot;
    [SerializeField] private string bindingGroup = "Arrows";
    [SerializeField] private bool configureInputOnAwake = true;

    public float velocity = 3.5f;
    public float kickForce = 2f;

    private Ball ball;

    private void Awake()
    {
        GetComponents();
    }

    private void Update()
    {
        UpdateDirections();
        UpdateSpriteFlip();
    }

    private void FixedUpdate()
    {
        if (!CanBeControlled())
        {
            rigidBody.velocity = Vector2.zero;
            return;
        }

        rigidBody.velocity = GetMoveDirection() * GetMoveSpeed();
    }

    public override void UpdateDirections()
    {
        if (!CanBeControlled())
        {
            horizontalInput = 0;
            verticalInput = 0;
            isRunning = false;
            return;
        }

        base.UpdateDirections();
    }

    private bool CanBeControlled()
    {
        return controlSlot != ControlSlot.Bot && controlSlot != ControlSlot.None;
    }

    private bool CanUseAction()
    {
        return CanBeControlled() && selected;
    }

    private Vector2 GetMoveDirection()
    {
        if (IsIdle()) return Vector2.zero;

        return new Vector2(horizontalInput, verticalInput).normalized;
    }

    private float GetMoveSpeed()
{
    if (IsIdle()) return 0f;

    return isRunning ? velocity * 1.7f : velocity;
}

    private void UpdateSpriteFlip()
    {
        if (horizontalInput > 0.1f)
        {
            spriteRenderer.flipX = false;
        }
        else if (horizontalInput < -0.1f)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void GetComponents()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (rigidBody == null)
        {
            rigidBody = GetComponent<Rigidbody2D>();
        }

        if (pInput == null)
        {
            pInput = GetComponent<PlayerInput>();
        }
    }

    public void ConfigureInput(ControlSlot newSlot, string newBindingGroup, InputDevice device)
    {
        controlSlot = newSlot;
        bindingGroup = newBindingGroup;

        if (pInput == null)
        {
            pInput = GetComponent<PlayerInput>();
        }

        if (pInput == null)
        {
            Debug.LogError(name + " no tiene PlayerInput.");
            return;
        }

        if (!CanBeControlled())
        {
            selected = false;
            pInput.DeactivateInput();
            Debug.Log(name + " es Bot. Input desactivado.");
            return;
        }

        pInput.ActivateInput();
        pInput.SwitchCurrentActionMap("Player");
        pInput.SwitchCurrentControlScheme(newBindingGroup, device);

        pInput.actions.bindingMask = InputBinding.MaskByGroup(newBindingGroup);

        Debug.Log(name + " configurado como " + controlSlot + " usando grupo " + bindingGroup);
    }

    public void SetBall(Ball newBall)
    {
        ball = newBall;
    }

    public override bool HasBall()
    {
        return ball != null;
    }

    public void Shoot(InputAction.CallbackContext callbackContext)
    {
        if (!callbackContext.performed) return;
        if (!CanUseAction()) return;
        if (ball == null) return;

        Debug.Log(name + " pateó");

        animator.SetTrigger("Shoot");

        ball.KickBall(this);
        ball = null;
    }

    public void PassOrTackle(InputAction.CallbackContext callbackContext)
    {
        if (!callbackContext.performed) return;
        if (!CanUseAction()) return;

        if (ball != null)
        {
            Debug.Log(name + " hizo pase");

            playerStateManager.ChangeState(playerStateManager.passState);
            ball.PassBall(this);
        }
        else
        {
            Debug.Log(name + " hizo barrida");

            playerStateManager.ChangeState(playerStateManager.tackleState);
        }
    }

    public void OnPlayerReceivesBall(PlayerController previousOwner)
    {
        if (previousOwner == null)
        {
            _myTeamController.SelectPlayerWhoReceiveBall(this);
            return;
        }

        _myTeamController.SelectPlayerWhoReceiveBall(this, previousOwner);
    }
    public void setBall(Ball newBall)
    {
    SetBall(newBall);
    }

    public enum ControlSlot
    {
        None,
        Player1,
        Player2,
        Player3,
        Player4,
        Bot
    }
         public  enum Role
    {
        GoalKeeper,
        Forward,
        MidFilder,
    }
      public Vector2 DirectionTo(Vector2 someDirection)
    {
        

        Vector2 direction = someDirection - _position;

        return direction.normalized;
    }
}