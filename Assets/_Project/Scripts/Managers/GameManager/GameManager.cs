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

   [Header("Time")]
    private float _intialTimeInSecods = 180;
    private float _timeLeft;

    public event Action<int,int> OnDrawGame;

    public event Action OnScoreTeamA;
    public event Action OnScoreTeamB;

    public event Action<int,int> OnEnded;

   public event Action<int,int> OnScoreChanged;
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
        _ballController = _ball.GetComponent<Ball>();
        _teamAController = _Team_A.GetComponent<TeamController>();
        _teamBController = _Team_B.GetComponent<TeamController>();
        
    }
    public GameObject _ball;
    public Ball _ballController;

    public TeamController _teamAController;
    public TeamController _teamBController;
    private int _goalsTeam_A = 0;
    private int _goalsTeam_B = 0;

    void OnEnable()
    {
        GameStateManager.Instance.OnMatchEnded += FinishMatch;
        GameStateManager.Instance.OnMatchStarted += StartMatch;
        GameStateManager.Instance.OnMatchPaused += PauseMatch;
        GameStateManager.Instance.OnMatchRestart += RestartGame;
        GameStateManager.Instance.OnGoalScored += RegisterGoal;
    }
    void OnDisable()
    {
        GameStateManager.Instance.OnMatchEnded -= FinishMatch;
        GameStateManager.Instance.OnMatchStarted -= StartMatch;
        GameStateManager.Instance.OnMatchPaused -= PauseMatch;
        GameStateManager.Instance.OnMatchRestart -= RestartGame;
        GameStateManager.Instance.OnGoalScored -= RegisterGoal;
    }





    [Header("Marcadores")]
    private TextMeshProUGUI _marcadorEquipoA;
    private TextMeshProUGUI _marcadorEquipoB;

    [Header("Equipos")]
    public GameObject _Team_A;
    public GameObject _Team_B;

    //private TeamController _controladorEquipoA;
    //private TeamController _controladorEquipoB;



    [Header("Tiempo")]
    private float _tiempoInicial;
    private float _tiempoRestante;
    private TextMeshProUGUI _textoConValorDeTiempo;

    /*void Start()
    {
        GameStateManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
    }
    */
    
    void Update()
    {
        
        if (GameStateManager.Instance.IsOnPlayState())
        {
            _timeLeft -= Time.deltaTime;
            if(_timeLeft <= 0)
            {
                _timeLeft = 0;
               GameStateManager.Instance.ChangeState(new EndState());
            }
            OnTimeChanged?.Invoke(_timeLeft);
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
        

        //GameStateManager.Instance.ChangeState(GameState.End);
    }
    /*
    void ComenzarPartida()
    {


        // Más adelante:
        // UIManager.Instance.MostrarHUD();
        // AudioManager.Instance.ReproducirSonidoInicio();
        
    }

    /*
    public void Pause()
    {
        OnMatchPaused?.Invoke();
        _gameStatemanager.ChangeToPause();
        
    }

    public void Play()
    {
 
    }

  public void Restart()
    {
        
    }
    */
    public void RegisterGoal()
    {
        OnScoreChanged?.Invoke(_goalsTeam_A,_goalsTeam_B);

    }

    public void RegisterTeam_A_Goal()
    {
        _goalsTeam_A ++;
        RegisterGoal();
        OnScoreTeamA?.Invoke();
    }
    
    
    public void RegisterTeam_B_Goal()
    {
        _goalsTeam_B ++;
        RegisterGoal();
        OnScoreTeamB?.Invoke();
    }
       
    
    public void StartMatch()
    {
        OnScoreChanged?.Invoke(_goalsTeam_A,_goalsTeam_B);
    }

    public void ActivateEndEvent()
    {
        
    }

  

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


    void StartMatch()
    {
        Time.timeScale =1f;
    }
    
    void TerminarPartida()
    {


        // Más adelante:
        // UIManager.Instance.MostrarPantallaFinal();
        // AudioManager.Instance.ReproducirSonidoFinPartida();
   }
 

    
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
