using UnityEngine;

public class RunState : PlayerState
{
    // Start is called before the first frame update
    public override void Enter()
    {        base.Enter();
     _playerAnimator.speed = 1;
        _playerAnimator.Play("Run",0,0f);
    }

    public override void Tick()
    {
        if (_playerController.PassPressed())
        {
            if (! _playerController.HasBall())
            {
                _playerStateManager.ChangeState(_playerStateManager.tackleState);
                return;
            }
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

        if (!_playerController.IsMoving())
        {
            _playerStateManager.ChangeState(_playerStateManager.idleState);
            return;
        }

        if (!_playerController.RunPressed())
        {
            _playerStateManager.ChangeState(_playerStateManager.walkState);
            return;
        }
    }
}