using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
     // Responsabilidad:
    // Controla la interfaz del juego.
    // No decide la lógica de la partida, solo muestra información visual.

    [Header("Canvas principales")]
    [SerializeField] private Canvas _canvasHUD;
    [SerializeField] private Canvas _canvasMenuPausa;
    [SerializeField] private Canvas _canvasPantallaFinal;
    [SerializeField] private Canvas _canvasPantallaInicial;

    [Header("HUD")]
    [SerializeField] private TextMeshProUGUI _textoTiempo;
    [SerializeField] private TextMeshProUGUI _textoMarcadorEquipoA;
    [SerializeField] private TextMeshProUGUI _textoMarcadorEquipoB;

    [Header("Mensajes")]
    [SerializeField] private GameObject _panelGol;
    [SerializeField] private GameObject _panelPausa;
    [SerializeField] private GameObject _panelPantallaFinal;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ActualizarMarcadores()
    {

    }
    void ActualizarTiempo()
    {
        
    }

    void MostrarMenuPausa()
    {
        
    }

    void MostrarPantallaInicial()
    {
        
    }

    void MostrarPantallaFinal()
    {
        
    }

     public void OcultarMenuPausa()
    {
        _panelPausa.SetActive(false);
    }

     public void OcultarPantallaFinal()
    {
        _panelPantallaFinal.SetActive(false);
    }

     public void OcultarPantallaInicial()
    {
        _panelPantallaFinal.SetActive(false);
    }

}
