using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class GameManager : MonoBehaviour
{
    // Responsabilidad:
    // Controla el estado general de la partida.
    // Más adelante manejará inicio, pausa, goles, tiempo, reinicio y fin del partido.
   
   public static GameManager Instance { get; private set; }

   public event Action<float> OnTimeChanged;
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
        _timeLeft = _intialTimeInSecods;
        
    }
    public Ball _ball;
    private TeamController _teamAController;
    private TeamController _teamBController;
    private int _goalsTeam_A = 0;
    private int _goalsTeam_B = 0;

    [Header("Time")]
    private float _intialTimeInSecods;
    private float _timeLeft;


    void OnEnable()
    {
        GameStateManager.Instance.OnGameStateChanged += HandleGameStateChanged;
    }

    void OnDisable()
    {
        GameStateManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
    }

    
    void Update()
    {
        if (GameStateManager.Instance.IsOnPlayState())
        {
            _timeLeft -= Time.deltaTime;
            if(_timeLeft <= 0)
            {
                _timeLeft = 0;
                FinishMatch();
            }
            OnTimeChanged?.Invoke(_timeLeft);
        }
        
    }

    void HandleGameStateChanged(GameState newGameState)
    {
       
        switch(newGameState){
            case GameState.Start:
                 RestartGame();
                 break;
            case GameState.Pause:
                PauseMatch();
                break;
            case GameState.Playing:
                StartMatch();
                break;
            case GameState.End:
                FinishMatch();
                break;
        }
    }
    
    void PauseMatch()
    {
        
    }
    void RestartGame()
    {
        _goalsTeam_A = 0;
        _goalsTeam_B = 0;
    }
    void FinishMatch()
    {
        GameStateManager.Instance.ChangeState(GameState.End);
    }
    /*
    void ComenzarPartida()
    {


        // Más adelante:
        // UIManager.Instance.MostrarHUD();
        // AudioManager.Instance.ReproducirSonidoInicio();
        
    }
    */
    /*
    public void Pause()
    {
        OnMatchPaused?.Invoke();
        _gameStatemanager.ChangeToPause();
        
    }
    */
    
    /*
    public void Restart()
    {
        OnMatchRestarted?.Invoke();
        _gameStatemanager.ChangeToRestart();
    }
    */
    /*
    public void RegisterGoal()
    {
        OnScoreChanged?.Invoke(_goalsTeam_A,_goalsTeam_B);
        _gameStatemanager.ChangeToGoal();

    }*/

    public void RegisterTeam_A_Goal()
    {
        _goalsTeam_A += 1;
    }

    public void RegisterTeam_B_Goal()
    {
        _goalsTeam_B += 1;
    }

   

    /*
    public void ActivateMenuEvent()
    {
        OnMenuActive?.Invoke();
    }
    public void ActivatePlayEvent()
    {
        OnMatchStarted?.Invoke();
    }

    public void ActivateEndEvent()
    {
        
    }

    */



    /*
    void ReanudarPartida()
    {   
         if (_partidaTerminada)
        {
            return;
        }
 

        // Más adelante:
        // UIManager.Instance.OcultarMenuPausa();
        // AudioManager.Instance.ReanudarMusica();

    }

    */

    void StartMatch()
    {
        Time.timeScale =1f;
    }
    /*
    void TerminarPartida()
    {


        // Más adelante:
        // UIManager.Instance.MostrarPantallaFinal();
        // AudioManager.Instance.ReproducirSonidoFinPartida();
    }
    */
 

    
   // void CongelarComportamientosDeLosEquipos()
    //{
         /*
        Más adelante:
        - Avisar a los TeamController que bloqueen movimiento.
        - Bloquear disparos, barridas y habilidades.
        */
    //}
    
    
    
    /*
    public void ActualizarMarcadores()
    {   //UIManager
        /*_marcadorEquipoA.text = _golesEquipoA.ToString();
        _marcadorEquipoB.text = _golesEquipoB.ToString();*/

    //}
    
/*
    void PrepararPartida()
    {
        ReiniciarPartida();



        _tiempoRestante = _tiempoInicial;
    }
    */
    
    
    
    /*void ReiniciarEquipos()
    {
        ReiniciarEquipoA();
        ReiniciarEquipoB();
    }

    void ReiniciarEquipoA()
    {
        /*
        Reiniciar Posiciones
        Reiniciar habilidades
        Reiniciar UI Habilidad
        */
    //}
    /* void ReiniciarEquipoB()
    {
        /*
        Reiniciar Posiciones
        Reiniciar habilidades
        Reiniciar UI Habilidad
        */
    //}
    
    //void ReiniciarPosicionPelota()
    //{
          /*
        Más adelante:
        - Enviar la pelota al centro de la cancha.
        - Frenar su velocidad.
        */
    //}

//    void ReiniciarMarcadores()
    //{
        //Llamar a UIManager
    //}

//    void ReiniciarTiempo()
    //{
    //    
    //}//

    


}
