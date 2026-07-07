
using UnityEngine;
public class Dive : PlayerState
{   
    private PlayerController _currentPlayerController;
    private Vector2 _ballPosition;
    [SerializeField] private float diveDuration = 0.5f;
    private float _diveTimer;
     public override void Enter()
    {   base.Enter();
        _currentPlayerController = _currentPlayer.GetComponent<PlayerController>();
        _ballPosition =GameManager.Instance._ball.transform.position;

        
        Vector2 target_Dive = new Vector2(_currentPlayerController._spawnPosition.x,_ballPosition.y);
        Vector2 direction = _currentPlayerController.DirectionTo(target_Dive);
        if(direction.y > 0)
        {
            _playerAnimator.Play("Dive_up");
        }
        else
        {
            _playerAnimator.Play("Dive_Down");
        }
        _currentPlayerController._movementDirection = direction * _currentPlayerController.velocity;
        _diveTimer = diveDuration;
    }

    public override void Tick()
    {
        _diveTimer -= Time.deltaTime;
       if(_diveTimer <= 0f)
        {
            _playerStateManager.ChangeState(_playerStateManager.idleState);
        }
    }
}