using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class GameManager : MonoBehaviour
{
    // Responsabilidad:
    // Controla el estado general de la partida.
    // Más adelante manejará inicio, pausa, goles, tiempo, reinicio y fin del partido.
   
   public static GameManager Instance { get; private set; }
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

   [Header("Estados de Partida")]
   private GameStateManager _gameStatemanager;



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
    private float _tiempoInicial;
    private float _tiempoRestante;
    private TextMeshProUGUI _textoConValorDeTiempo;


    //Events
    public event Action OnMatchStarted;
    public event Action OnMatchPaused;
    public event Action OnMatchResumed;
    public event Action OnMatchRestarted;
    public event Action OnMatchEnded;
    public event Action<string> OnGoalScored;

    public event Action<int,int> OnScoreChanged;
    public event Action<float> OnTimeChanged;
    public event Action<GameState> OnGameStateChanged;

    public event Action OnMenuActive;

    void Start()
    {
        Restart();
    }

    /*void Update()
    {  
      // ActualizarTiempo();
       ActualizarMarcadores();
    }
    */

    /*
    void ComenzarPartida()
    {


        // Más adelante:
        // UIManager.Instance.MostrarHUD();
        // AudioManager.Instance.ReproducirSonidoInicio();
        
    }
    */
    public void Pause()
    {
        OnMatchPaused?.Invoke();
        _gameStatemanager.Pause();
        
    }

    public void Play()
    {
        OnMatchStarted?.Invoke();
        _gameStatemanager.Play();
    }

    public void Restart()
    {
        OnMatchRestarted?.Invoke();
        //_gameStatemanager.Restart();
        ReiniciarPartida();
    }


    public void GoalScored()
    {
       
    }

    public void End()
    {
        OnMatchEnded?.Invoke();
        _gameStatemanager.End();
    }

    public void ChangeState(GameState newGameState)
    {
        OnGameStateChanged?.Invoke(newGameState);
    }

    public void Menu()
    {
        OnMenuActive?.Invoke();
        _gameStatemanager.Menu();
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

    void TerminarPartida()
    {


        // Más adelante:
        // UIManager.Instance.MostrarPantallaFinal();
        // AudioManager.Instance.ReproducirSonidoFinPartida();
    }
    */
     public void RegistrarGolEquipoA()
    {
        _golesEquipoA +=1;

        ActualizarMarcadores();

        // Más adelante:
        // Reiniciar después de gol.
        // AudioManager.Instance.ReproducirSonidoGol();
    }

    public void RegistrarGolEquipoB()
    {
        _golesEquipoB +=1;

        ActualizarMarcadores();

        // Más adelante:
        // Reiniciar después de gol.
        // AudioManager.Instance.ReproducirSonidoGol();
    }

    
   // void CongelarComportamientosDeLosEquipos()
    //{
         /*
        Más adelante:
        - Avisar a los TeamController que bloqueen movimiento.
        - Bloquear disparos, barridas y habilidades.
        */
    //}
    
    
    public void ActualizarTiempo()
    {
        
         _tiempoRestante -= Time.deltaTime;

        if (_tiempoRestante <= 0)
        {
            _tiempoRestante = 0;
            //TerminarPartida();
        }
    }

    public void ActualizarMarcadores()
    {   //UIManager
        /*_marcadorEquipoA.text = _golesEquipoA.ToString();
        _marcadorEquipoB.text = _golesEquipoB.ToString();*/

    }
    
/*
    void PrepararPartida()
    {
        ReiniciarPartida();



        _tiempoRestante = _tiempoInicial;
    }
    */
    
    public void ReiniciarPartida()
    {
        ReiniciarEquipos();
        ReiniciarPosicionPelota();
        ReiniciarMarcadores();
        ReiniciarTiempo();
        //ReiniciarEscena no se como hacerlo averiguar
    }

    void ReiniciarEquipos()
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
    }

    void ReiniciarEquipoB()
    {
        /*
        Reiniciar Posiciones
        Reiniciar habilidades
        Reiniciar UI Habilidad
        */
    }

    void ReiniciarPosicionPelota()
    {
          /*
        Más adelante:
        - Enviar la pelota al centro de la cancha.
        - Frenar su velocidad.
        */
    }

    void ReiniciarMarcadores()
    {
        //Llamar a UIManager
    }

    void ReiniciarTiempo()
    {
        
    }

    











}
