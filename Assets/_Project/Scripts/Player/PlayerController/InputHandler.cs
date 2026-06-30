using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class InputHandler : MonoBehaviour
{
    [Header("Player Input/Animator Essentials")]
    public Animator animator;
    public PlayerStateManager playerStateManager;
    public PlayerInput pInput;
    [Header("Options")]
    public float verticalInput;
    public float horizontalInput;
    public bool isRunning;
    public bool passPressed = false;
    public bool shootPressed = false;
    public bool selected;
    public abstract bool HasBall();
    public virtual void UpdateDirections()
    {
        GetHorizontalInput();
        GetVerticalInput();
        IsRunning();
    }

    public float GetHorizontalInput()
    {
        horizontalInput = pInput.actions["Movement"].ReadValue<Vector2>().x;
        return horizontalInput;
    }

    public float GetVerticalInput()
    {
        verticalInput = pInput.actions["Movement"].ReadValue<Vector2>().y;
        return verticalInput;
    }

    public bool IsRunning()
    {
        isRunning = pInput.actions["Run"].IsPressed();
        return isRunning;
    }
    public virtual bool IsIdle()
    {
        return Mathf.Abs(horizontalInput) < 0.01f && Mathf.Abs(verticalInput) < 0.01f;
    }   
    public virtual bool IsMoving()
    {
        return !IsIdle();
    }

    public bool PassPressed()
    {
        if (!selected) return false;
        return pInput.actions["Shoot"].IsPressed();
    }

    public bool ShootPressed()
    {
        if (!selected) return false;
        return pInput.actions["Shoot"].IsPressed();
    }

    public enum KeyPress
    {
        Pass,
        Tackle,
        Run,
        Ability,
        Shoot,
        None
    }
}
