/*using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{   
    public TeamController _myTeamController;
    public Animator _animator;
    public Rigidbody2D rigidBody;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer bodyspriteRenderer;
    public GoalTarget _targetGoal;
    public Team _team;
    public Role _role;
    public float _movementSpeed = 3.5f;
    public Vector2 _movementDirection;
    public Vector2 _spawnPosition;
    public Vector2 _position;
    public float _forceTowardsTheBall;
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

    private int verticalInput;
    private int horizontalInput;

    private void Awake() 
    {
        GetComponents();
    }

    private void Update()
    {
        UpdateDirections();
        UpdateAnimations();
    }

    public void UpdateDirections() 
    {
        if(! selected) return;
        verticalInput = GetVerticalInput();
        horizontalInput = GetHorizontalInput();
    }

    private void FixedUpdate()
    {
        rigidBody.velocity = Direction() * Velocity();
    }

    private void UpdateAnimations()
    {
        bool isRunning = !IsIdle();
      
        if(horizontalInput > 0)
        {
            spriteRenderer.flipX = false;
            bodyspriteRenderer.flipX = false;
        }
        else if(horizontalInput < 0)
        {
            spriteRenderer.flipX = true;
            bodyspriteRenderer.flipX = true;
        }
    } 

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
        return _movementSpeed * 1.7f;
    }

    return _movementSpeed;
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

    private bool IsIdle()
    {
        return horizontalInput == 0 && verticalInput == 0;
    }

    public bool IsMoving()
    {
        return horizontalInput != 0 || verticalInput != 0;
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

        _targetGoal = _myTeamController._targetGoal;
        _team = _myTeamController.team;
    
     
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

    public void OnPlayerReceivesBall(PlayerController previousOwner)
    {
        _myTeamController.SelectPlayerWhoReceiveBall(this,previousOwner);
    }

    public enum ControlSlot
    {
        None,
        Player1,
        Player2,
        Bot
    }
    public ControlSlot controlSlot = ControlSlot.Bot;
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
*/