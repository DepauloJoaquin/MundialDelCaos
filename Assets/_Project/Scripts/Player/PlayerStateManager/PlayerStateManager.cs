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

        void Awake()
        {
            if(_playerController == null)
            {
                _playerController = GetComponent<PlayerController>();
            } 
            _player = gameObject;
            _playerAnimator = _playerController.animator;
            idleState.Init(this, _playerController,_player,_playerAnimator);
            walkState.Init(this, _playerController,_player,_playerAnimator);
            runState.Init(this, _playerController,_player,_playerAnimator);
            passState.Init(this, _playerController,_player,_playerAnimator);
            shootState.Init(this, _playerController,_player,_playerAnimator);
            tackleState.Init(this, _playerController,_player,_playerAnimator);
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
    }
