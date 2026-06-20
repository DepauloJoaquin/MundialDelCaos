using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : InputHandler
{
    public TeamController _myTeam;
    public Rigidbody2D rigidBody;
    public SpriteRenderer spriteRenderer;
    public Animator _animator;
    
    
    public float velocity = 3.5f;
    //public InputActionReference move; 

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
        bool isRunning = !IsIdle();

        if (horizontalInput > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (horizontalInput < 0)
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

        if (RunPressed())
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

    public void OnPlayerReceivesBall(PlayerController previousOwner)
    {
        _myTeam.SelectPlayerWhoReceiveBall(this,previousOwner);
    }

    public enum ControlSlot
    {
        None,
        Player1,
        Player2,
        Bot
    }
    public ControlSlot controlSlot = ControlSlot.Bot;
}
