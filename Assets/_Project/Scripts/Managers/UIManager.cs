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
    [SerializeField] private TextMeshProUGUI _textTime;
    [SerializeField] private TextMeshProUGUI _textScoreTeamA;
    [SerializeField] private TextMeshProUGUI _textScoreTeamB;

    [Header("Mensajes")]
    [SerializeField] private GameObject _panelGol;
    [SerializeField] private GameObject _panelPausa;
    [SerializeField] private GameObject _panelPantallaFinal;
    [SerializeField] private GameObject _panelPantallaInicial;

    public static UIManager Instance { get; private set; }
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
        
    }

    private void OnEnable()
    {
        GameStateManager.Instance.OnMatchStarted += StartMatch;
        GameStateManager.Instance.OnMatchPaused += ShowPauseMenu;
        GameStateManager.Instance.OnMatchRestart += RestartHud;
        GameManager.Instance.OnTimeChanged += UpdateTimer;
        GameManager.Instance.OnScoreChanged += UpdateScores;
    }
    private void OnDisable()
    {
        GameStateManager.Instance.OnMatchStarted += StartMatch;
        GameStateManager.Instance.OnMatchPaused += ShowPauseMenu;
        GameStateManager.Instance.OnMatchRestart += RestartHud;
        GameManager.Instance.OnTimeChanged += UpdateTimer;
        GameManager.Instance.OnScoreChanged += UpdateScores;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ShowPauseMenu()
    {
        
    }

    void RestartHud()
    {
        
    }

   

 

    void ShowHud()
    {
        
    }

    public void StartMatch()
    {
        //Mostrar Hud y lo que sea necesario
    }

    void UpdateTimer(float timeInSeconds)
    {   
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        _textTime.text = string.Format("{0:00}:{1:00}",minutes,seconds);

    }

    public void UpdateScores(int _goalsTeam_A,int _goalsTeam_B)
    {
        
    }


}
