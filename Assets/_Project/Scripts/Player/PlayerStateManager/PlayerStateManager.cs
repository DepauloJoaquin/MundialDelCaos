    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PlayerStateManager : MonoBehaviour
    {
        private PlayerState _currentState;
        public InputHandler _playerController;
        private GameObject _player;
        protected Animator _playerAnimator;
        private Animator shirtAnimator;


        [Header("States")]
        public RunState runState;
        public WalkState walkState;
        public IdleState idleState;
        public PassState passState;
        public ShootState shootState;
        public TackleState tackleState;
        public Dive diveState;
        public StunState stunState;

        void Awake()
{
    if (_playerController == null)
    {
        _playerController = GetComponent<PlayerController>();
    }
    _player = gameObject;
    if (shirtAnimator == null)
    {
    ShirtAnimationController shirtController = GetComponentInChildren<ShirtAnimationController>();

    if (shirtController != null)
    {
        shirtAnimator = shirtController.GetComponent<Animator>();
    }
    }


    _playerAnimator = _playerController.animator;

    idleState.Init(this, _playerController, _player, _playerAnimator);
    walkState.Init(this, _playerController, _player, _playerAnimator);
    runState.Init(this, _playerController, _player, _playerAnimator);
    passState.Init(this, _playerController, _player, _playerAnimator);
    shootState.Init(this, _playerController, _player, _playerAnimator);
    tackleState.Init(this, _playerController, _player, _playerAnimator);
    diveState.Init(this, _playerController, _player, _playerAnimator);
    stunState.Init(this, _playerController,_player,_playerAnimator);
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
            UpdateShirtAnimation(stateTochange);
        }
        public bool IsCurrentState(PlayerState state)
    {
    return _currentState == state;
    }
    private void UpdateShirtAnimation(PlayerState state)
{
    if (shirtAnimator == null)
    {
        return;
    }

    if (state == idleState)
    {
        shirtAnimator.Play("Idle");
    }
    else if (state == walkState)
    {
        shirtAnimator.Play("Walk");
    }
    else if (state == runState)
    {
        shirtAnimator.Play("Run");
    }
    else if (state == passState)
    {
        shirtAnimator.Play("Pass");
    }
    else if (state == shootState)
    {
        shirtAnimator.Play("Shoot");
    }
    else if (state == tackleState)
    {
        shirtAnimator.Play("Tackle");
    }
    else if (state == diveState)
    {
        shirtAnimator.Play("Dive");
    }
}
    }
