using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShirtAnimationController : InputHandler
{
    [Header("Must Pass")]
    public PlayerController playerController;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    
    void Start()
    {
        if(playerController == null)
        {
            playerController = GetComponent<PlayerController>();
        }
        ImitatePlayerControllerProperties();
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

    private void UpdateSelection() 
    {
        selected = playerController.selected;
    }

    private void ImitatePlayerControllerProperties()
    {
        upKey = playerController.upKey;
        downKey = playerController.downKey;
        leftKey = playerController.leftKey;
        rightKey = playerController.rightKey;
        passKey = playerController.passKey;
        shootKey = playerController.shootKey;
        runKey = playerController.runKey;
        abilityKey = playerController.abilityKey;
        tackleKey = playerController.tackleKey;
    }

    public void CambiarColor(Color nuevoColor) 
    {
        spriteRenderer.color = nuevoColor;
    }

    public override bool HasBall() 
    {
        return playerController.HasBall();
    }
}
