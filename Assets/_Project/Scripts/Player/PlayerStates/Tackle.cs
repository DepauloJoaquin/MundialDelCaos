using UnityEngine;

public class TackleState : PlayerState
{
    // Start is called before the first frame update
    public override void Enter()
    {        base.Enter();
        _playerAnimator.Play("Tackle",0,0f);
        AudioManager.Instancia.ReproducirBarrida();
    }

    public override void Tick()
    {
       
        if (AnimationFinished("Tackle"))
        {
            _playerStateManager.ChangeState(_playerStateManager.idleState);
        }
        
    }
}