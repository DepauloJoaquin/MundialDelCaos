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
           playerController = GetComponentInParent<PlayerController>();
        }
    }

    /*private void Update()
    {
        //UpdateDirections();
        UpdateSpriteFlip();
        UpdateSelection();
    }*/

    private void LateUpdate()
    {
        UpdateSpriteFlip();
    }

    
    

   private void UpdateSpriteFlip()
    {   
    if (playerController == null) return;
    if (playerController.spriteRenderer == null) return;

    spriteRenderer.flipX = playerController.spriteRenderer.flipX;
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
        else if (state == "Tackle")
        {
            playerStateManager.ChangeState(playerStateManager.tackleState);
        }
        else if (state == "Stun")
        {
            playerStateManager.ChangeState(playerStateManager.stunState);
        }
    }
}
