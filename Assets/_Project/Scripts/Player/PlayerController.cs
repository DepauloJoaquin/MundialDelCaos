using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{   
    private Vector2 moveInput;
    private bool runInput;

    private float verticalInput;
    private float horizontalInput;
    public Animator _animator;
    public Rigidbody2D rigidBody;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer bodyspriteRenderer;
    public TeamController _myTeam;
    public float velocity = 3.5f;
    public bool selected;
    //public InputActionReference move; 
    [Header("Movement Keys")]
    public KeyCode upKey;
    public KeyCode downKey;
    public KeyCode leftKey;
    public KeyCode rightKey;

    [Header("Action Keys")]

    public KeyCode passKey;
    public KeyCode shootKey;
    public KeyCode runKey;
    public KeyCode abilityKey;
    public KeyCode tackleKey;

    private int _verticalInput;
    private int _horizontalInput;

    public ControlSlot controlSlot;

    private void Awake() 
    {
        GetComponents();
    }

    private void Update()
    {
        UpdateDirections();
        UpdateAnimations();
    }
    /*
    public void UpdateDirections() 
    {
        if(! selected) return;
        verticalInput = GetVerticalInput();
        horizontalInput = GetHorizontalInput();
    }
    */
    public void UpdateDirections() 
{
    if (!selected)
    {
        moveInput = Vector2.zero;
        horizontalInput = 0f;
        verticalInput = 0f;
        return;
    }

    horizontalInput = moveInput.x;
    verticalInput = moveInput.y;
}

    private void FixedUpdate()
    {
        rigidBody.velocity = Direction() * Velocity();
    }

    private void UpdateAnimations()
    {
       /* bool isRunning = !IsIdle();
      
        if(horizontalInput > 0)
        {
            spriteRenderer.flipX = false;
            bodyspriteRenderer.flipX = false;
        }
        else if(horizontalInput < 0)
        {
            spriteRenderer.flipX = true;
            bodyspriteRenderer.flipX = true;
        }*/
        if (horizontalInput > 0.1f)
{
    spriteRenderer.flipX = false;
    bodyspriteRenderer.flipX = false;
}
else if (horizontalInput < -0.1f)
{
    spriteRenderer.flipX = true;
    bodyspriteRenderer.flipX = true;
}
    } 
    /*
   private Vector2 Direction()
{
    if (IsIdle())
    {
        return Vector2.zero;
    }

    return new Vector2(horizontalInput, verticalInput).normalized;
}*/
private Vector2 Direction()
{
    if (IsIdle())
    {
        return Vector2.zero;
    }

    return new Vector2(horizontalInput, verticalInput).normalized;
}
    private float Velocity() 
{
    if (IsIdle())
    {
        return 0f;
    }

    if (RunPressed())
    {
        return velocity * 1.7f;
    }

    return velocity;
}

    private int GetVerticalInput()
    {
       return Get_Input(upKey, downKey);
    }

    private int GetHorizontalInput()
    {
       return Get_Input(rightKey, leftKey);
    }

    private int Get_Input(KeyCode firstKey, KeyCode secondKey)
    {
        if (Input.GetKey(firstKey))
        {
            return 1;
        }
        else if (Input.GetKey(secondKey))
        {
            return -1;
        }
        return 0;
    }
    /*
    private bool IsIdle()
    {
        return horizontalInput == 0 && verticalInput == 0;
    }
    */

    private bool IsIdle()
{
     return Mathf.Abs(horizontalInput) < 0.01f && Mathf.Abs(verticalInput) < 0.01f;
}   /*
    public bool IsMoving()
    {
        return horizontalInput != 0 || verticalInput != 0;
    }*/
    public bool IsMoving()
{
    return !IsIdle();
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
        if (bodyspriteRenderer == null)
        {
            bodyspriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        }
    
     
    }

      public bool PassPressed()
    {
        if (!selected) return false;

        return Input.GetKeyDown(passKey);
    }

    public bool ShootPressed()
    {
        if (!selected) return false;

        return Input.GetKeyDown(shootKey);
    }

    public bool ShootReleased()
    {
         if (!selected) return false;
         return Input.GetKeyUp(shootKey);
    }

    public bool TacklePressed()
    {
         if (!selected) return false;
         return Input.GetKeyDown(tackleKey);
    }
    public bool RunPressed()
    {
         if (!selected) return false;
         return Input.GetKey(runKey);
    }
    public bool RunReleased()
    {
    if (!selected) return false;
    return Input.GetKeyUp(runKey);
    }
    public enum ControlSlot
    {
        Bot,
        Player1,
        Player2
    }
    public void OnMovement(InputValue value)
    {
    moveInput = value.Get<Vector2>();

    Debug.Log(gameObject.name + " Movement recibido: " + moveInput);
    }

    public void OnPlayerReceivesBall(PlayerController previousOwner)
    {
        if(previousOwner == null) 
        {
            _myTeam.SelectPlayerWhoReceiveBall(this); 
        }
        _myTeam.SelectPlayerWhoReceiveBall(this,previousOwner);
    }
}
