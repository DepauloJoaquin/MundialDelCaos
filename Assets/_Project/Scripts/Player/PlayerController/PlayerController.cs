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
    public ControlSlot controlSlot = ControlSlot.Bot;
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

    public override void UpdateDirections()
    {
        if (!selected) return;
        base.UpdateDirections();
    }

    private void FixedUpdate()
    {
        rigidBody.velocity = Direction() * Velocity();
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

        if (IsRunning())
        {
            return velocity * 1.7f;
        }

        return velocity;
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

    public void Shoot(InputAction.CallbackContext callbackContext)
    {
        if (! callbackContext.performed) { return; } 
        if (ball == null) { return; }
        if (! ShootPressed()) { return; }
        ball.KickBall(this);
        ball = null;
    }

    public void PassOrTackle(InputAction.CallbackContext callbackContext)
    {
        if (! callbackContext.performed) { return; } 
        if (ball == null) { return; }
        if (! PassPressed()) { return; }
        ball.PassBall(this);
    }
    public void OnPlayerReceivesBall(PlayerController previousOwner)
    {
        if (previousOwner == null)
        {
            _myTeam.SelectPlayerWhoReceiveBall(this);
        }
        _myTeam.SelectPlayerWhoReceiveBall(this, previousOwner);
    }
    public enum ControlSlot
    {
        None,
        Player1,
        Player2,
        Bot
    }

}
