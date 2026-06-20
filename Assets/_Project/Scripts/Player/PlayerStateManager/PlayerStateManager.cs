using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    private PlayerState _currentState;
    public InputHandler _playerController;
    private Animator _animator;
    private GameObject _player;

    [Header("States")]
    public RunState runState ;
    public WalkState walkState;
    public IdleState idleState;
    public PassState passState;
    public ShootState shootState;
    public TackleState tackleState;

    void Awake()
    {
        if(_playerController == null ) 
        {
            _playerController = GetComponent<PlayerController>();
        }
        _animator = GetComponent<Animator>();
        _player = gameObject;
        idleState.Init(this, _playerController,_player,_animator);
        walkState.Init(this, _playerController,_player,_animator);
        runState.Init(this, _playerController,_player,_animator);
        passState.Init(this, _playerController,_player,_animator);
        shootState.Init(this, _playerController,_player,_animator);
        tackleState.Init(this, _playerController,_player,_animator);
    }
       
    void Start()
    {
        ChangeState(idleState);
    }

    // Update is called once per frame
    private void Update()
    {
        if (_currentState != null)
        {
            _currentState.Tick();
        }
    }
    public void ChangeState(PlayerState stateTochange)
    {
        
        if (_currentState == stateTochange)
        {
        return;
        }
          if (_currentState != null)
        {
            _currentState.Exit();
        }
        _currentState = stateTochange;
        _currentState.Enter();
    }
}
