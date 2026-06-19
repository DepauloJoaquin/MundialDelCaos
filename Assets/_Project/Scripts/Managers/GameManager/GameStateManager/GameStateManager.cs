using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    private GameState _currentGameState;

    private GameManager _gameManager;

    [Header("States")]
    
    private PlayState _play;
    //private RestartState _restart;
    private PauseGameState _pause;
    private EndState _end;
    private MenuState _menu;
    private GoalState goal;

    void Awake()
    {
        _gameManager = GetComponent<GameManager>();
        //_play = GetComponent<PlaySta>();
    }
    void Start()
    {
        _currentGameState = _menu;
    }
    public void ChangeState(GameState newGameState)
    {
        if(_currentGameState == newGameState)
        {
            return;
        }
        _currentGameState.Exit();
        _currentGameState = newGameState;
        _currentGameState.Enter();

        
    }
    void OnEnable()
    {
        _gameManager.OnGameStateChanged += ChangeState;
    }

    public void Pause()
    {
        ChangeState(_pause);
    }

    public void Play()
    {
        ChangeState(_play);
    }

   /* public void Restart()
    {
         ChangeState(_restart);
    }*/

    public void End()
    {
        ChangeState(_end);
    }

    public void Menu()
    {
        ChangeState(_menu);
    }

}
