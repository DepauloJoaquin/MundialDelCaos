using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameStateManager : MonoBehaviour
{
    public IGameState _currentGameState;
    public event Action OnGameStateChanged;
    public event Action OnMatchStarted;
    public event Action OnMatchEnded;
    public event Action OnMatchPaused;
    public event Action OnMatchResumed;
    public event Action OnMatchRestart;
    public event Action OnMainMenu;
    public event Action OnMainSelectedTeamMenu;
    public event Action OnOptionsMenu;

    public event Action OnGoalScored;
     public static GameStateManager Instance { get; private set; }
  
    void Start()
    {
       ChangeState(new MainMenuState());
    }
    public void ChangeState(IGameState newGameState)
    {   
         if (newGameState == null)
        {
        return;
        }
        if (_currentGameState != null &&
        _currentGameState.StateType == newGameState.StateType)
        {
        return;
        }
        if(_currentGameState == newGameState)
        {
            return;
        }
        _currentGameState?.Exit();
        _currentGameState = newGameState;
        _currentGameState.Enter();
        HandleGameEventByGameState(_currentGameState.StateType);
    }
    private void Awake()
{
    if (Instance != null && Instance != this)
    {
        Destroy(gameObject);
        return;
    }

    Instance = this;
}

    void HandleGameEventByGameState(GameState state)
    {
        switch (state)
        {
            case GameState.MainMenu:
                 OnMainMenu?.Invoke();
                 break;
            case GameState.Paused:
                OnMatchPaused?.Invoke();
                break;
            case GameState.Start:
                OnMatchStarted?.Invoke();
                break;
            case GameState.Restart:
                OnMatchRestart?.Invoke();
                break;
            case GameState.OptionsMenu:
                 OnOptionsMenu?.Invoke();
                 break;
            case GameState.End:
                OnMatchEnded?.Invoke();
                break;
            case GameState.SelectTeamMenu:
                OnMainSelectedTeamMenu?.Invoke();
                break;
            case GameState.Playing:
                OnMatchResumed?.Invoke();
                break;
            case GameState.Goal:
                OnGoalScored?.Invoke();
                break;            
        }
    }

    // Methods for buttons

    public void GoToMainMenu()
    {
        ChangeState(new MainMenuState());
    }

    public void GoToSelectTeamMenu()
    {
        ChangeState(new SelecTeamState());
    }

    public void GoToOptionsMenu()
    {
        ChangeState(new OptionsMenuState());
    }

    public void StartMatch()
    {
        ChangeState(new StartState());
    }

    public void SetPlaying()
    {
        ChangeState(new PlayingState());
    }

    public void PauseMatch()
    {
        ChangeState(new PauseState());
    }

    public void RestartMatch()
    {
        ChangeState(new RestartState());
    }

    public void EndMatch()
    {
        ChangeState(new EndState());
    }

    public void GoalScored()
    {
        ChangeState(new GoalState());
    }


    public bool IsOnPlayState()
    {
        return _currentGameState.StateType == GameState.Playing;
    }

 

}
