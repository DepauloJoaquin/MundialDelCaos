using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : InputHandler
{
    public TeamController _myTeamController;
    public AIBehaviour _AIController;
    public AIGoalKeeperBehaviour _GoalKeeperController;
    public Rigidbody2D rigidBody;
    public SpriteRenderer spriteRenderer;
    public Vector2 _targetGoal;
    public Team _team;
    public Role _role;
    public float _movementSpeed = 3.5f;
    public Vector2 _movementDirection;
    public Vector2 _spawnPosition;
    public Vector2 _position;
    public float _forceTowardsTheBall = 1f;
    public bool _isBotRunning;
    public ControlSlot controlSlot = ControlSlot.Bot;
    [SerializeField] private string bindingGroup = "";
    [SerializeField] private bool configureInputOnAwake = true;
    public int _formationSlot;
    public float velocity = 3.5f;
    public float kickForce = 2f;

    private Ball ball;

    private void Awake()
    {
        GetComponents();
    }

    private void Update()
    {   _position = transform.position;
        UpdateDirections();
        UpdateSpriteFlip();
    }

    private void FixedUpdate()
    {
        
        if(controlSlot == ControlSlot.Bot)
        {
            rigidBody.velocity = _movementDirection;
            return;
        }
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
        float horizontalMovement;

        if (controlSlot == ControlSlot.Bot)
        {
            horizontalMovement = _movementDirection.x;
        }
        else
        {
            horizontalMovement = horizontalInput;
        }
        if (horizontalMovement > 0.1f)
        {
            spriteRenderer.flipX = false;
        }
        else if (horizontalMovement < -0.1f)
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
        SetControlSlot(newSlot);
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
        pInput.actions = Instantiate(pInput.actions);
        pInput.SwitchCurrentActionMap("Player");
       

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

        playerStateManager.ChangeState(playerStateManager.shootState);

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
        _myTeamController.SelectPlayerWhoReceiveBall(this,previousOwner);
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

    public void SetControlSlot(ControlSlot newslot)
    {
        controlSlot = newslot;

        if(_AIController == null)
        {
            _AIController = GetComponent<AIBehaviour>();
        }

         if (_GoalKeeperController == null)
        {
            _GoalKeeperController = GetComponent<AIGoalKeeperBehaviour>();
        }

        bool isBot = controlSlot == ControlSlot.Bot;
        bool isGoalKeeper = _role == Role.GoalKeeper;
        if (_AIController != null)
        {
            _AIController.enabled = isBot && !isGoalKeeper;
        }

        if (_GoalKeeperController != null)
        {
            _GoalKeeperController.enabled = isBot && isGoalKeeper;
        }
        if (controlSlot != ControlSlot.Bot)
        {
        _movementDirection = Vector2.zero;
        }

    }

    public override bool IsIdle()
    {   if(controlSlot == ControlSlot.Bot)
        {
            return _movementDirection.magnitude <= 0.01f;
        }
        return base.IsIdle();
    }
    public override bool IsMoving()
    {   if(controlSlot == ControlSlot.Bot)
        {
            return _movementDirection.magnitude >= 0.01f;
        }
        return base.IsMoving();
    }
    public override bool IsRunning()
    {   if(controlSlot == ControlSlot.Bot)
        {
            return _isBotRunning;
        }
        return base.IsRunning();
    }

    public float DistanceTo(Vector2 targetPosition)
    {
        return Vector2.Distance(_position,targetPosition);
    }
}