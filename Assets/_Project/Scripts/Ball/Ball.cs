using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float rotationSpeed = 300f;
    public GameObject _currentOwner;
    public PlayerController _currentOwnerController;
    public GameObject _lastOwner;
    public GameObject _lastKickPlayer;
    public GameObject _passTarget;
    public bool _isFree;
    [Header("X Variation")]
    public float frecuency = 1.5f;
    public float intensity = 0.2f;
    private Rigidbody2D rb;
    private float time;
    private bool isRotating;

    // Unity Methods
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        time = Time.deltaTime;
    }

    private void Update() 
    {
        FlipBallRotation();
        if(_currentOwner != null) 
        { 
            Debug.DrawRay(transform.position, GetShootDirection(GetCurrentOwnerController()), Color.green); 
        }
        
    }

    private void FixedUpdate()
    {
        time += Time.deltaTime;
        RotateBall();
        MoveBallTowardsCurrentOwner();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController receiver = collision.gameObject.GetComponent<PlayerController>();
        if (receiver == null) { return; }
        
        receiver.setBall(this);
        isRotating = true;
        PlayerController previousOwnerController = null;
        if (! CurrentOwnerIsNull())
        {
            previousOwnerController = _currentOwner.GetComponent<PlayerController>();
        }
        _lastOwner = _currentOwner;
        _currentOwner = collision.gameObject;
        _currentOwnerController = receiver;
        receiver.OnPlayerReceivesBall(previousOwnerController); 
        MakeTheBallControlled();
    }

    
    private void FlipBallRotation()
    {
        if(_currentOwner == null) { return; }
        bool IsOwnerFlipped = GetCurrentOwnerController().spriteRenderer.flipX;
        if(IsOwnerFlipped)
        {
            rotationSpeed = Mathf.Abs(rotationSpeed);
        }
        else
        {
            rotationSpeed = - Mathf.Abs(rotationSpeed);
        }
    }

    private void RotateBall()
    {
        if(isRotating)
        {
            rb.angularVelocity = RotationSpeedIfMoving();
        }
    }

    private void MakeTheBallControlled()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        isRotating = false;
        //rb.isKinematic = true;
        _isFree = false;
    }

    public void KickBall(PlayerController currentOwnerController)
    {
        float currentKickForce = currentOwnerController.kickForce;
        Vector2 shootDirection = GetShootDirection(currentOwnerController);
        rb.isKinematic = false;
        _isFree = true;
        _lastKickPlayer = _currentOwner;
        _currentOwner = null;
        _currentOwnerController = null;
        rb.AddForce(shootDirection * currentKickForce, ForceMode2D.Impulse);
    }

    public void PassBall(PlayerController currentOwnerController)
    {
        PlayerController nearestPlayer = BallPassZone.GetNearestPlayerPosition(currentOwnerController._team, currentOwnerController);
        if(nearestPlayer == null) { print("NO"); return; }
        currentOwnerController.setBall(null);
        _passTarget = nearestPlayer.gameObject;
        Vector2 shootDirection = (_passTarget.transform.position - transform.position).normalized;
        rb.isKinematic = false;
        _isFree = true;
        _lastKickPlayer = _currentOwner;
        _currentOwner = null;
        _currentOwnerController = null;

        float passSpeed = currentOwnerController.kickForce * 0.8f;
        rb.AddForce(shootDirection * passSpeed, ForceMode2D.Impulse);
    }

    private Vector2 GetShootDirection(PlayerController currentOwnerController) //buscar keyword out
    {
        float verticalDirection = currentOwnerController.verticalInput;
        float horizontalDirection = currentOwnerController.horizontalInput;
        return new Vector2(horizontalDirection, verticalDirection).normalized;
    }

    private void MoveBallTowardsCurrentOwner()
    {
        if (CurrentOwnerIsNull()) { return; }
        Vector2 CurrentOwnerPosition = _currentOwner.transform.GetChild(1).position; // This is "BallPlacement"
        PlayerController currentOwnerController = _currentOwner.GetComponent<PlayerController>();
        bool isMoving = currentOwnerController.IsMoving();
        if (isMoving)
        {
            isRotating = true;
        }
        else
        {
            isRotating = false;
        }
        transform.position = CurrentOwnerPosition;
        ApplyMovingEffect(currentOwnerController);
    }

    private void ApplyMovingEffect(PlayerController currentOwnerController)
    {
        if(! currentOwnerController.IsMoving()) { return; }
        if(currentOwnerController.horizontalInput == 0) { return; }
        float variableXPosition = Mathf.Cos(getFrecuency() * time) * getIntensity();
        transform.position = new Vector2(variableXPosition + transform.position.x, transform.position.y);
    }

    private float RotationSpeedIfMoving()
    {
        if (CurrentOwnerIsNull())
        {
            return rotationSpeed;
        }
        PlayerController pc = _currentOwner.GetComponent<PlayerController>();
        if (pc.IsMoving())
        {
            return RotationSpeedIfRunning(pc);
        }
        else
        {
            return 0f;
        }
    }

    private float RotationSpeedIfRunning(PlayerController pc)
    {
        //print(isMoving);
        if (pc.IsRunning())
        {
            return rotationSpeed + 50f;
        }
        else
        {
            return rotationSpeed;
        }
    }

    private float getIntensity()
    {
        PlayerController pc = _currentOwner.GetComponent<PlayerController>();
        if (pc.IsRunning())
        {
            return intensity * 1.5f;
        }
        else
        {
            return intensity;
        }
    }

    private float getFrecuency()
    {
        PlayerController pc = _currentOwner.GetComponent<PlayerController>();
        if (pc.IsRunning())
        {
            return frecuency + 0.15f;
        }
        else
        {
            return frecuency;
        }
    }

    private bool CurrentOwnerIsNull()
    {
        return _currentOwner == null;
    }

    private PlayerController GetCurrentOwnerController()
    {
        return _currentOwner.GetComponent<PlayerController>();
    }
}