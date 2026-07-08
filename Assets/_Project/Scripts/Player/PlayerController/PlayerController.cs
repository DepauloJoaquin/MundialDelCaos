using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class PlayerController : InputHandler
{
    public TeamController _myTeamController;
    public AIBehaviour _AIController;
    public AIGoalKeeperBehaviour _GoalKeeperController;
    public Rigidbody2D rigidBody;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer _indicadorControl;
    public Vector2 _targetGoal;
    public Team _team;
    public Role _role;
    public float _movementSpeed = 3.5f;
    public Vector2 _movementDirection;
    public Vector2 _spawnPosition;
    public Vector2 _position;
    public float _forceTowardsTheBall = 1f;
    public bool _isBotRunning;
    public float tackleForce = 6f;
    private bool isInTackleState = false;
    
    [SerializeField] private string bindingGroup = "";
    [SerializeField] private bool configureInputOnAwake = true;
    public int _formationSlot;
    public float velocity = 3.5f;
    public float kickForce = 2f;
    private bool _matchPaused = false;
    private Ball ball;
    public TextMeshPro _textoIndicador;

    [Header("Habilidad Súper Simplificada")]
    public bool _habilidadActiva = false;
    private float _tiempoRestanteHabilidad = 0f;
    [SerializeField] private float _extraVelocidad = 2f;


    private void Awake()
    {
        GetComponents();
    }

   private void Update()
{   
    if (!GameStateManager.Instance.IsOnPlayState())
    {
        return;
    }

    _position = transform.position;
    UpdateDirections();
    UpdateSpriteFlip();
    CountDown();
}

    public void Ability(InputAction.CallbackContext callbackContext)
{
    if (!callbackContext.performed) return;
    if (!CanUseAction()) return;

    if (GameManager.Instance._ballController._currentOwnerController != this)
    {
        return;
    }

    if (_myTeamController == null)
    {
        return;
    }

    if (!_myTeamController.TryUseFullStamina(this))
    {
        return;
    }

    Debug.Log(name + " activó su habilidad especial.");

    _habilidadActiva = true;
    _tiempoRestanteHabilidad = 4f;

    if (AudioManager.Instancia != null)
    {
        AudioManager.Instancia.ReproducirActivarHabilidad();
    }
}

    public void CountDown()
{
    if (!_habilidadActiva)
    {
        return;
    }

    _tiempoRestanteHabilidad -= Time.deltaTime;

    if (_tiempoRestanteHabilidad <= 0)
    {
        _habilidadActiva = false;

        if (_myTeamController != null)
        {
            _myTeamController.StopStaminaBoost(this);
        }
    }
}

    private void FixedUpdate()
    {
        if (!GameStateManager.Instance.IsOnPlayState())
        {
            CeroVelocity();
            return;
        }

        if(controlSlot == ControlSlot.Bot)
        {
            rigidBody.velocity = _movementDirection;
            return;
        }
        if (!CanBeControlled())
        {
            CeroVelocity();
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

    if (_habilidadActiva && _myTeamController != null)
    {
        return velocity * _myTeamController.GetStaminaSpeedMultiplier(this);
    }

    if (isRunning)
    {
        return velocity * 1.7f;
    }

    return velocity;
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

     
        pInput.actions = Instantiate(pInput.actions);
        pInput.neverAutoSwitchControlSchemes = true;

        pInput.ActivateInput();
        pInput.SwitchCurrentActionMap("Player");
       

        pInput.actions.bindingMask = InputBinding.MaskByGroup(newBindingGroup);

            if (device != null)
        {
            pInput.actions.devices = new InputDevice[] { device };
        }

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
        if (!GameStateManager.Instance.IsOnPlayState())
        {
            return;
        }

        if (!callbackContext.performed) return;
        if (!CanUseAction()) return;

        // Evaluamos si realmente el jugador tiene asignada la pelota
        if (ball != null && ball._currentOwnerController == this)
        {
            

            if (AudioManager.Instancia != null)
            {
                AudioManager.Instancia.ReproducirPatada();
            }

            playerStateManager.ChangeState(playerStateManager.shootState);
            ball.OnKick(this);
            ball = null; // Se limpia la referencia después de disparar
        }
        else
        {
            

            if (AudioManager.Instancia != null)
            {
                AudioManager.Instancia.ReproducirPatearSinPelota();
            }
        }
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
        float verticalDirection = verticalInput - GetShootVerticalOffset(verticalInput); //(1, 0, -1)
        float horizontalDirection = GetHorizontalForceKick(); //(1, 0, -1)
        return new Vector2(horizontalDirection, verticalDirection);
    }

    private Vector2 GetShootDirectionGamepad()
    {
        float verticalDirection = verticalInput; 
        float horizontalDirection = GetHorizontalForceKick(); 
        return new Vector2(horizontalDirection, verticalDirection);
    }
 private float GetHorizontalForceKick()
    {
        if(spriteRenderer.flipX)
        {
            return -1f;
        }
        else
        {
            return 1f;
        }
    }

private float GetShootVerticalOffset(float input)
    {
        if(Mathf.Sign(input) > 0)
        {
            if (input == 0)
            {
                return 0f;
            }

            return 0.4f;
        }
        else
        {
            return -0.4f;
        }
    }

    public void PassOrTackle(InputAction.CallbackContext callbackContext)
    {    if (!GameStateManager.Instance.IsOnPlayState())
        {
            return;
        }
        if (!callbackContext.performed) return;
        if (!CanUseAction()) return;

        if (ball != null && ball._currentOwnerController == this)
        {
            Debug.Log(name + " hizo pase");

            if (AudioManager.Instancia != null)
            {
                AudioManager.Instancia.ReproducirPase();
            }

            playerStateManager.ChangeState(playerStateManager.passState);
            transform.GetChild(0).GetComponent<ShirtAnimationController>().ChangeState("Pass");
            ball.PassBall(this);
        }
        else
        {
            Debug.Log(name + " hizo barrida");

            if (AudioManager.Instancia != null)
            {
                AudioManager.Instancia.ReproducirBarrida();
            }

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
        if (_indicadorControl == null)
{
    return;
}

if (controlSlot == ControlSlot.Bot || controlSlot == ControlSlot.None)
{
    DesactivateIndicatorIfBot(_indicadorControl);
    return;
}

ActivateIndicatorAndChangeTextByTeam(_indicadorControl);

          
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
    private bool ImFromTeamA()
    {
        return _myTeamController.team == Team.A;
    }

    void DesactivateIndicatorIfBot(SpriteRenderer indicator)
    {
        if(controlSlot != ControlSlot.Player1 &&controlSlot != ControlSlot.Player2)
        {
            indicator.enabled = false;

        if (_textoIndicador != null)
        {
            _textoIndicador.enabled = false;
        }

        return;
        }
    }

    void ActivateIndicator(SpriteRenderer indicator)
    {
        indicator.enabled = true;

        if (_textoIndicador != null)
        {
            _textoIndicador.enabled = true;
        }
    }

    private void ActivateIndicatorAndChangeTextByTeam(SpriteRenderer indicator)
{
    if (indicator == null)
    {
        return;
    }

    if (_myTeamController.team == Team.A)
    {
        ActivateIndicatorByTeamA(indicator);
        return;
    }

    if (_myTeamController.team == Team.B)
    {
        ActivateIndicatorByTeamB(indicator);
        return;
    }
}
private void ActivateIndicatorByTeamA(SpriteRenderer indicator)
{
    if (controlSlot == ControlSlot.Player1)
    {
        ActivateIndicatorPlayer1(indicator, "#FF0000", "J1");
        return;
    }

    if (controlSlot == ControlSlot.Player2)
    {
        ActivateIndicatorPlayer2(indicator, "#F8A139", "J2");
        return;
    }
}
private void ActivateIndicatorByTeamB(SpriteRenderer indicator)
{
    if (controlSlot == ControlSlot.Player1)
    {
        ActivateIndicatorPlayer1(indicator, "#39EEF8", "J3");
        return;
    }

    if (controlSlot == ControlSlot.Player2)
    {
        ActivateIndicatorPlayer2(indicator, "#F246F3", "J4");
        return;
    }
}

    private void ActivateIndicatorPlayer1(SpriteRenderer indicator, string hexColor, string text)
{
    indicator.enabled = true;

    if (ColorUtility.TryParseHtmlString(hexColor, out Color color))
    {
        indicator.color = color;
    }

    if (_textoIndicador != null)
    {
        _textoIndicador.enabled = true;
        _textoIndicador.text = text;
    }
}
    private void ActivateIndicatorPlayer2(SpriteRenderer indicator, string hexColor, string text)
{
    indicator.enabled = true;

    if (ColorUtility.TryParseHtmlString(hexColor, out Color color))
    {
        indicator.color = color;
    }

    if (_textoIndicador != null)
    {
        _textoIndicador.enabled = true;
        _textoIndicador.text = text;
    }
    }
    public void PauseBehaviours()
    {
    _matchPaused = true;

    if (rigidBody != null)
    {
        rigidBody.velocity = Vector2.zero;
        rigidBody.angularVelocity = 0f;
    }

    if (_AIController != null)
    {
        _AIController.enabled = false;
    }

    if (_GoalKeeperController != null)
    {
        _GoalKeeperController.enabled = false;
    }

    if (pInput != null)
    {
        pInput.DeactivateInput();
    }

    _movementDirection = Vector2.zero;
    horizontalInput = 0;
    verticalInput = 0;
    isRunning = false;
    }
    public void ResumeBehaviours()
    {
    _matchPaused = false;

    if (controlSlot == ControlSlot.Bot)
    {
        SetControlSlot(ControlSlot.Bot);
        return;
    }

    if (controlSlot != ControlSlot.None)
    {
        if (pInput != null)
        {
            pInput.ActivateInput();
            pInput.SwitchCurrentActionMap("Player");
        }

        SetControlSlot(controlSlot);
    }
    }

    public void CeroVelocity()
    {
        if(isInTackleState && rigidBody.velocity == Vector2.zero)
        {
            rigidBody.AddForce(new Vector2(GetHorizontalForceKick(), 0) * tackleForce, ForceMode2D.Impulse);
            return;
        }
        rigidBody.velocity = Vector2.zero;
    }
}