using UnityEngine;

public class ShootState : PlayerState
{
    // Start is called before the first frame update
    public override void Enter()
    {        base.Enter();
        _playerAnimator.Play("Shoot",0,0f);
    }

    public override void Tick()
    {
        
        if ( AnimationFinished("Shoot"))
        {
            _playerStateManager.ChangeState(_playerStateManager.idleState);
        }
    }
}