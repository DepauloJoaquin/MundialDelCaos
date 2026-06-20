using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : MonoBehaviour
{
    private GameObject _currentPlayer;
    protected InputHandler _playerController;
    protected Animator _playerAnimator;

    protected PlayerStateManager _playerStateManager;
    protected float _stateEnterTime;


    public void Init(PlayerStateManager stateManager, InputHandler controller, GameObject player, Animator playerAnimator)
    {
        _playerStateManager = stateManager;
        _playerController = controller;
        _playerAnimator = playerAnimator;
        _currentPlayer = player;
    }



    public virtual void Enter()
    {
        _stateEnterTime = Time.time;
    }
    public abstract void Tick();
    public void Exit()
    {
        _playerAnimator.speed = 1;
    }

    public bool AnimationFinished(string animationName)
    {
        if (Time.time - _stateEnterTime < 0.05f)
        {
            return false;
        }
        AnimatorStateInfo info = _playerAnimator.GetCurrentAnimatorStateInfo(0);
        Debug.Log(info.fullPathHash + " " + info.normalizedTime + " " + info.IsName(animationName));
        return info.IsName(animationName) && info.normalizedTime >= 1f;
    }
}
