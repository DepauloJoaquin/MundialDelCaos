    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PlayerStateManager : MonoBehaviour
    {
        private PlayerState _currentState;
        public InputHandler _playerController;
        private GameObject _player;
        protected Animator _playerAnimator;

        [Header("States")]
        public RunState runState;
        public WalkState walkState;
        public IdleState idleState;
        public PassState passState;
        public ShootState shootState;
        public TackleState tackleState;
        public Dive diveState;

        void Awake()
{
    if (_playerController == null)
    {
        _playerController = GetComponent<PlayerController>();
    }

    if (_playerController == null)
    {
        Debug.LogError(name + " no tiene PlayerController.");
        return;
    }

    _player = gameObject;

    if (_playerController.animator == null)
    {
        Debug.LogError(name + " no tiene Animator asignado en PlayerController.");
        return;
    }

    _playerAnimator = _playerController.animator;

    if (idleState == null) Debug.LogError(name + " no tiene IdleState.");
    if (walkState == null) Debug.LogError(name + " no tiene WalkState.");
    if (runState == null) Debug.LogError(name + " no tiene RunState.");
    if (passState == null) Debug.LogError(name + " no tiene PassState.");
    if (shootState == null) Debug.LogError(name + " no tiene ShootState.");
    if (tackleState == null) Debug.LogError(name + " no tiene TackleState.");
    if (diveState == null) Debug.LogError(name + " no tiene DiveState.");

    if (idleState == null || walkState == null || runState == null ||
        passState == null || shootState == null || tackleState == null || diveState == null)
    {
        return;
    }

    idleState.Init(this, _playerController, _player, _playerAnimator);
    walkState.Init(this, _playerController, _player, _playerAnimator);
    runState.Init(this, _playerController, _player, _playerAnimator);
    passState.Init(this, _playerController, _player, _playerAnimator);
    shootState.Init(this, _playerController, _player, _playerAnimator);
    tackleState.Init(this, _playerController, _player, _playerAnimator);
    diveState.Init(this, _playerController, _player, _playerAnimator);
}
        
        void Start()
        {
            ChangeState(idleState);
        }
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
        public bool IsCurrentState(PlayerState state)
    {
    return _currentState == state;
    }
    }
