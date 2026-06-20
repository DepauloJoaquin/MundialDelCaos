using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public GameState _currentGameState { get; private set; }
    public event Action<GameState> OnGameStateChanged;
     public static GameStateManager Instance { get; private set; }
    private void Awake()
    {
        // Configuración del Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Evita que se destruya al cambiar de escena
        }
        else
        {
            Destroy(gameObject);
        }
    }
   
    void Start()
    {
        _currentGameState = GameState.MainMenu;
    }
    public void ChangeState(GameState newGameState)
    {
        if(_currentGameState == newGameState)
        {
            return;
        }
        _currentGameState = newGameState;

        OnGameStateChanged?.Invoke(newGameState);

    }

    public bool IsOnPlayState()
    {
        return _currentGameState == GameState.Playing;
    }

    

}
