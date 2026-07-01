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
    private float _intialTimeInSecods;
    private float _timeLeft;

   public event Action<float> OnTimeChanged;
   public event Action<int,int> OnScoreChanged;
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
    public GameObject _ball;
    private TeamController _teamAController;
    private TeamController _teamBController;
    private int _goalsTeam_A = 0;
    private int _goalsTeam_B = 0;

    


    void OnEnable()
    {   // Evitamos que un GameManager duplicado intente suscribirse ya que se destrruye al final del frame
        if (Instance != null && Instance != this) return;
        GameStateManager.Instance.OnMatchStarted += StartMatch;
        GameStateManager.Instance.OnMatchPaused += PauseMatch;
        GameStateManager.Instance.OnMatchRestart += RestartGame;
        GameStateManager.Instance.OnGoalScored += RegisterGoal;
    }





    [Header("Marcadores")]
    private TextMeshProUGUI _marcadorEquipoA;
    private TextMeshProUGUI _marcadorEquipoB;

    [Header("Equipos")]
    //TODO: Realizar el componente TeamController

    //private TeamController _controladorEquipoA;
    //private TeamController _controladorEquipoB;
    private int _golesEquipoA = 0;
    private int _golesEquipoB = 0;



    [Header("Tiempo")]
    private bool terminoLaPartida = false;
    private bool seReprodujoLaCuentaAtras = false;
    private float _tiempoInicial;
    private float _tiempoRestante;
    private TextMeshProUGUI _textoConValorDeTiempo;

    /*void Start()
    {
         GameStateManager.Instance.OnMatchStarted -= StartMatch;
        GameStateManager.Instance.OnMatchPaused -= PauseMatch;
        GameStateManager.Instance.OnMatchRestart -= RestartGame;
    }
*/
    
    void Update()
    {
        // Si el GameStateManager no está listo, salimos del Update
        if (GameStateManager.Instance == null) return;
        if (GameStateManager.Instance.IsOnPlayState())
        {
            _timeLeft -= Time.deltaTime;
            if(_tiempoRestante <= 10 && !seReprodujoLaCuentaAtras)
            {
                seReprodujoLaCuentaAtras = true;
                AudioManager.Instancia.ReproducirCuentaRegresiva();
            }
            if(_timeLeft <= 0 && !terminoLaPartida)
            {
                _timeLeft = 0;
                terminoLaPartida = true;

                AudioManager.Instancia.ReproducirSilvatoFinal();
                AudioManager.Instancia.DetenerMusica();
                // GameStateManager.Instance.ChangeState(GameState.End);
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
        
    }
    
 

    /*
    public void Pause()
    {
        
    }

    public void Play()
    {
 
    }

  public void Restart()
    {
        
    }
    
    public void RegisterGoal()
    {
        OnScoreChanged?.Invoke(_goalsTeam_A,_goalsTeam_B);
    }

    public void RegisterTeam_A_Goal()
    {
        _goalsTeam_A += 1;
    }
    */
    
    public async void RegisterGoal()
    {
        
        AudioManager.Instancia.ReproducirGol();
        await System.Threading.Tasks.Task.Delay(1000); // Simula un retraso
        AudioManager.Instancia.ReproducirSilvatoInicial();
    }
    public void StartMatch()
    {
        //Hinchada del partido de fondo
        // Al iniciar la escena del partido (en el Start)
        AudioManager.Instancia.ActivarMusicaPartida();
        AudioManager.Instancia.ReproducirSilvatoInicial();
    }
    void TerminarPartida()
    {
        // UIManager.Instance.MostrarPantallaFinal();
        // En la cuenta regresiva del final
        AudioManager.Instancia.ReproducirCuentaRegresiva();
        //luego cuando finaliza la cuenta regresiva, se reproduce el silbato final y la música de victoria
        AudioManager.Instancia.ReproducirSilvatoFinal();
        AudioManager.Instancia.ReproducirPantallaFinalVictoria();

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
