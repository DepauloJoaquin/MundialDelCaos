using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Responsabilidad:
    // Controla el estado general de la partida.
    // Más adelante manejará inicio, pausa, goles, tiempo, reinicio y fin del partido.
   [Header("Estados de Partida")]
   private bool _partidaEnJuego;
   private bool _partidaPausada;

   private bool _partidaTerminada;


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

    void Start()
    {
        PrepararPartida();
    }

    void Update()
    {
        if(!_partidaEnJuego || !_partidaPausada || !_partidaTerminada)
        {
            return;
            
        }
       ActualizarTiempo();
       ActualizarMarcadores();
    }


    void ComenzarPartida()
    {
        _partidaEnJuego = true;

        // Más adelante:
        // UIManager.Instance.MostrarHUD();
        // AudioManager.Instance.ReproducirSonidoInicio();
        
    }

    void PausarPartida()
    {   
         if (!_partidaEnJuego || _partidaTerminada)
        {
            return;
        }
        CongelarComportamientosDeLosEquipos();
        _partidaPausada = true;

          // Más adelante:
        // UIManager.Instance.MostrarMenuPausa();
        // AudioManager.Instance.PausarMusica();
    }

    void ReanudarPartida()
    {   
         if (_partidaTerminada)
        {
            return;
        }
        _partidaEnJuego = true;
        _partidaPausada = false;

        // Más adelante:
        // UIManager.Instance.OcultarMenuPausa();
        // AudioManager.Instance.ReanudarMusica();

    }

    void TerminarPartida()
    {
        _partidaEnJuego = false;
        _partidaPausada = false;
        _partidaTerminada = true;

        // Más adelante:
        // UIManager.Instance.MostrarPantallaFinal();
        // AudioManager.Instance.ReproducirSonidoFinPartida();
    }

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


    void CongelarComportamientosDeLosEquipos()
    {
         /*
        Más adelante:
        - Avisar a los TeamController que bloqueen movimiento.
        - Bloquear disparos, barridas y habilidades.
        */
    }

    void ActualizarTiempo()
    {
        
         _tiempoRestante -= Time.deltaTime;

        if (_tiempoRestante <= 0)
        {
            _tiempoRestante = 0;
            TerminarPartida();
        }
    }

    void ActualizarMarcadores()
    {   //UIManager
        /*_marcadorEquipoA.text = _golesEquipoA.ToString();
        _marcadorEquipoB.text = _golesEquipoB.ToString();*/

    }

    void PrepararPartida()
    {
        ReiniciarPartida();

        _partidaEnJuego = false;
        _partidaPausada = false;
        _partidaTerminada = false;

        _tiempoRestante = _tiempoInicial;
    }

    void ReiniciarPartida()
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
