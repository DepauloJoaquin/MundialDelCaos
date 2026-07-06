using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShirtAnimationController : InputHandler
{
    [Header("Must Pass")]
    public PlayerController playerController;
    public SpriteRenderer spriteRenderer;
    
    void Start()
    {
        if(playerController == null)
        {
            playerController = GetComponent<PlayerController>();
        }
    }

    private void Update()
    {
        UpdateDirections();
        UpdateSpriteFlip();
        UpdateSelection();
    }

    public override void UpdateDirections()
    {
        if (!selected) return;
        base.UpdateDirections();
    }

    private void UpdateSpriteFlip()
    {
        if (horizontalInput > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (horizontalInput < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void UpdateSelection() 
    {
        selected = playerController.selected;
    }

    public void ChangeColor(Color nuevoColor) 
    {
        spriteRenderer.color = nuevoColor;
    }

    public override bool HasBall() 
    {
        return playerController.HasBall();
    }

    public void ChangeState(string state)
    {
        if (state == "Pass")
        {
            playerStateManager.ChangeState(playerStateManager.passState);
        }
        else
        {
            playerStateManager.ChangeState(playerStateManager.tackleState);
        }
    }
}
