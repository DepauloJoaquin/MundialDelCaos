using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputHandler : MonoBehaviour
{
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

    public int verticalInput;
    public int horizontalInput;
    public bool selected;
    

    public virtual void UpdateDirections()
    {
        verticalInput = GetVerticalInput();
        horizontalInput = GetHorizontalInput();
    }
    public int GetVerticalInput()
    {
        return Get_Input(upKey, downKey);
    }

    public int GetHorizontalInput()
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

    public bool IsIdle()
    {
        return horizontalInput == 0 && verticalInput == 0;
    }

    public bool IsMoving()
    {
        return horizontalInput != 0 || verticalInput != 0;
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
}
