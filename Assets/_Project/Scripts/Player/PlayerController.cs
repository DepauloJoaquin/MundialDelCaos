using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : InputHandler
{
    public TeamController _myTeam;
    public Team team;
    public Rigidbody2D rigidBody;
    public SpriteRenderer spriteRenderer;
    public Animator _animator;
    public ControlSlot controlSlot = ControlSlot.Bot;
    public float velocity = 3.5f;
    public float kickForce = 2f;
    //public InputActionReference move; 
    private Ball ball; 

    private void Awake()
    {
        GetComponents();
    }

    private void Update()
    {
        UpdateDirections();
        UpdateSpriteFlip();
        KickBall();
        PassBall();
    }

    public override void UpdateDirections()
    {
        if (!selected) return;
        base.UpdateDirections();
    }
    
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

    private void UpdateSpriteFlip()
    {
       /* bool isRunning = !IsIdle();
      
        if(horizontalInput > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (horizontalInput < 0)
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
    private Vector2 Velocity() 
{
        if (IsIdle())
        {
            return Vector2.zero;
        }

        return new Vector2(horizontalInput, verticalInput).normalized;
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
    }

    public void setBall(Ball ball)
    {
        this.ball = ball;
    }

    public override bool HasBall() 
    {
        return ball != null;
    }

    public void KickBall()
    {
        if (ball == null) { return; }
        if (!ShootPressed()) { return; }
        ball.KickBall(this);
        ball = null;
    }

    public void PassBall()
    {
        if (ball == null) { return; }
        if (!PassPressed()) { return; }
        ball.PassBall(this);
    }

    public void OnPlayerReceivesBall(PlayerController previousOwner)
    {
        if(previousOwner == null) 
        {
            _myTeam.SelectPlayerWhoReceiveBall(this); 
        }
        _myTeam.SelectPlayerWhoReceiveBall(this,previousOwner);
    }

    public enum ControlSlot
    {
        None,
        Player1,
        Player2,
        Bot
    }
    
}
