using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< Updated upstream
public abstract class GameState : MonoBehaviour
{
    protected GameStateManager _gameStateManager;

    public virtual void Enter(){}
    
    public virtual void Update(){}
    public virtual void Exit(){}

}
=======
public enum GameState
{
    Start,
    Playing,
    Restart,
    End,
    MainMenu,
    SelectTeamMenu,
    Paused,
    OptionsMenu,
    Goal

}
>>>>>>> Stashed changes
