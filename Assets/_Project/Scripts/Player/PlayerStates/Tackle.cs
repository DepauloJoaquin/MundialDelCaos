using UnityEngine;

public class TackleState : PlayerState
{
    PlayerController.ControlSlot controlSlot;
    // Start is called before the first frame update
    public override void Enter()
    {        base.Enter();
        controlSlot = _playerController.controlSlot;
        _playerController.controlSlot = PlayerController.ControlSlot.None;
        _playerAnimator.Play("Tackle",0,0f);
        _playerController.OnStateChanges(this);
    }

    public override void Tick()
    { 
        if(AnimationFinished("Tackle"))
        {
            _playerStateManager.ChangeState(_playerStateManager.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        _playerController.controlSlot = controlSlot;
    }
}