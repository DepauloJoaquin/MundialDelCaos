using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : InputHandler
{
    public TeamController _myTeam;
    public Team team;
    public Rigidbody2D rigidBody;
    public SpriteRenderer spriteRenderer;
    public ControlSlot controlSlot = ControlSlot.Bot;
    [SerializeField] private string bindingGroup = "Arrows";
    [SerializeField] private bool configureInputOnAwake = true;

    public float velocity = 3.5f;
    public float kickForce = 2f;

    private Ball ball;

    private void Awake()
    {
        GetComponents();

        if (configureInputOnAwake)
        {
            ConfigureInput(controlSlot, bindingGroup);
        }
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

    public void ConfigureInput(ControlSlot newSlot, string newBindingGroup)
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

        if (newBindingGroup == "WASD" || newBindingGroup == "Arrows")
        {
            pInput.SwitchCurrentControlScheme(newBindingGroup, Keyboard.current);
        }
        else if (newBindingGroup == "Gamepad" && Gamepad.current != null)
        {
            pInput.SwitchCurrentControlScheme(newBindingGroup, Gamepad.current);
        }

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

        ball.OnKick(this);
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
            transform.GetChild(0).GetComponent<ShirtAnimationController>().ChangeState("Pass");
            ball.PassBall(this);
        }
        else
        {
            Debug.Log(name + " hizo barrida");
            transform.GetChild(0).GetComponent<ShirtAnimationController>().ChangeState("Tackle");
            playerStateManager.ChangeState(playerStateManager.tackleState);
        }
    }

    public void OnPlayerReceivesBall(PlayerController previousOwner)
    {
        if (previousOwner == null)
        {
            _myTeam.SelectPlayerWhoReceiveBall(this);
            return;
        }

        _myTeam.SelectPlayerWhoReceiveBall(this, previousOwner);
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

    public void Action(InputAction.CallbackContext callbackContext)
    {
        print("movimiento de " + name); 
        print(callbackContext);
    }

    public Vector2 GetShootDirection() {
        if(bindingGroup == "Arrows" || bindingGroup == "WASD")
        {
            return GetShootDirectionKeyboard();
        }
        else
        {
            return GetShootDirectionGamepad();
        }
    }

    private Vector2 GetShootDirectionKeyboard()
    {
        float verticalDirection = verticalInput; //- GetShootOffset(verticalInput); //(1, 0, -1)
        float horizontalDirection = horizontalInput; //(1, 0, -1)
        return new Vector2(horizontalDirection, verticalDirection);
    }

    private Vector2 GetShootDirectionGamepad()
    {
        float verticalDirection = verticalInput; 
        float horizontalDirection = horizontalInput; 
        return new Vector2(horizontalDirection, verticalDirection);
    }

    private float GetShootOffset(float input)
    {
        if(Mathf.Sign(input) > 0)
        {
            return 0.4f;
        }
        else if (Mathf.Sign(input) < 0)
        {
            return -0.4f;
        }
        else
        {
            return 0f;
        }
        
    }
}