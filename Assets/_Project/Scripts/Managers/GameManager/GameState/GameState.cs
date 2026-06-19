using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameState : MonoBehaviour
{
    protected GameStateManager _gameStateManager;

    public virtual void Enter(){}
    
    public virtual void Update(){}
    public virtual void Exit(){}

}
