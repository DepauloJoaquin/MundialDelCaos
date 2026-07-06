using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float rotationSpeed = 300f;
    public float cooldownAfterKick = 0.5f;
    public GameObject _currentOwner;
    public GameObject _lastOwner;
    public GameObject _lastKickPlayer;
    public GameObject _passTarget;
    public bool _isFree = true;
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
            Debug.DrawRay(transform.position, GetCurrentOwnerController().GetShootDirection() * 10, Color.green); 
        }
        else
        {
            Debug.DrawRay(transform.position, Vector2.zero, Color.red); //DEBUG
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
        if(! _isFree) { return; }
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
        receiver.OnPlayerReceivesBall(previousOwnerController); 
        ResetBallMovement();
    }

    // --------------------------------------

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

    private void ResetBallMovement()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        isRotating = false;
        _isFree = false;
    }

    private void ContinueBallMovement()
    {
        _isFree = true;
        _lastKickPlayer = _currentOwner;
        _currentOwner = null;
    }

    public void OnKick(PlayerController currentOwnerController)
    {
        float currentKickForce = currentOwnerController.kickForce;
        Vector2 shootDirection = currentOwnerController.GetShootDirection();
        ContinueBallMovement();
        StartCoroutine(BlockBallCoroutine());
        rb.AddForce(shootDirection * currentKickForce, ForceMode2D.Impulse);
    }

    private IEnumerator BlockBallCoroutine()
    {
        _isFree = false;
        yield return new WaitForSeconds(cooldownAfterKick);
        _isFree = true;
    }

    public void PassBall(PlayerController currentOwnerController)
    {
        PlayerController nearestPlayer = BallPassZone.GetNearestPlayerPosition(currentOwnerController.team, currentOwnerController);
        if(nearestPlayer == null) { print("NO"); return; }

        currentOwnerController.setBall(null);
        _passTarget = nearestPlayer.transform.GetChild(1).gameObject;
        Vector2 shootDirection = (_passTarget.transform.position - transform.position);
        ContinueBallMovement();
        float passSpeed = currentOwnerController.kickForce * 0.8f;
        rb.AddForce(shootDirection * passSpeed, ForceMode2D.Impulse);
        Debug.DrawRay(transform.position, (shootDirection * passSpeed) * 10, Color.red, 6000f); //DEBUG
    }

    private void MoveBallTowardsCurrentOwner()
    {
        if (CurrentOwnerIsNull()) { return; }

        Vector2 CurrentOwnerPosition = _currentOwner.transform.GetChild(1).position;
        SetRotation(GetCurrentOwnerController().IsMoving());   
        transform.position = CurrentOwnerPosition;
        ApplyMovingEffect(GetCurrentOwnerController());
    }

    private void SetRotation(bool isMoving)
    {
        isRotating = isMoving;
    }

    private void ApplyMovingEffect(PlayerController currentOwnerController)
    {
        if(!currentOwnerController.IsMoving()) { return; }
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
        if (GetCurrentOwnerController().IsMoving())
        {
            return RotationSpeedIfRunning(GetCurrentOwnerController());
        }
        else
        {
            return 0f;
        }
    }

    private float RotationSpeedIfRunning(PlayerController pc)
    {
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
        if (GetCurrentOwnerController().IsRunning())
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
        if (GetCurrentOwnerController().IsRunning())
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