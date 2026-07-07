using UnityEngine;

public class PassState : PlayerState
{
    // Start is called before the first frame update
    public override void Enter()
    {        base.Enter();
        _playerAnimator.Play("Pass",0,0f);
    }

    public override void Tick()
    {
        if(AnimationFinished("Pass"))
        {
            _playerStateManager.ChangeState(_playerStateManager.idleState);
        }
    }
}