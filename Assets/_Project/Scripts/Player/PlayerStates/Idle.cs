using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class IdleState : PlayerState
{
    // Start is called before the first frame update
    public override void Enter()
    {       base.Enter();
        _playerAnimator.Play("Idle");
    }

    public override void Tick()
    {
       if (_playerController.PassPressed())
        {
            _playerStateManager.ChangeState(_playerStateManager.passState);
            return;
        }

        if (_playerController.ShootPressed())
        {
            _playerStateManager.ChangeState(_playerStateManager.shootState);
            return;
        }

        if (_playerController.TacklePressed())
        {
            _playerStateManager.ChangeState(_playerStateManager.tackleState);
            return;
        }

        if (_playerController.IsMoving())
        {
            if (_playerController.RunPressed())
            {
                _playerStateManager.ChangeState(_playerStateManager.runState);
                return;
            }

            _playerStateManager.ChangeState(_playerStateManager.walkState);
            return;
        }
    }
}
