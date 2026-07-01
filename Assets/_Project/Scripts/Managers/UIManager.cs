using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem.Controls;
using Unity.Collections;
using UnityEngine.SocialPlatforms.Impl;

public class UIManager : MonoBehaviour
{
     // Responsabilidad:
    // Controla la interfaz del juego.
    // No decide la lógica de la partida, solo muestra información visual.

    [Header("Canvas principales")]
    [SerializeField] private GameObject _canvasHUD;
    [SerializeField] private GameObject _canvasMenuPausa;
    [SerializeField] private GameObject _canvasPantallaFinal;
    [SerializeField] private GameObject _canvasPantallaInicial;

    [Header("HUD")]
    [SerializeField] private TextMeshProUGUI _textTime;
    [SerializeField] private TextMeshProUGUI _textScoreTeamA;
    [SerializeField] private TextMeshProUGUI _textScoreTeamB;

    [Header("Mensajes")]
    [SerializeField] private GameObject _panelGolA;
    [SerializeField] private GameObject _panelGolB;
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
<<<<<<< Updated upstream
       GameStateManager.Instance.OnGameStateChanged += HandleGameStateChanged;
    }
    private void OnDisable()
    {
       GameStateManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
=======
       GameManager.Instance.OnScoreTeamA += ScoreTeamA;
       GameManager.Instance.OnScoreTeamB += ScoreTeamB;
       GameManager.Instance.OnTimeChanged += UpdateTimer;
       GameManager.Instance.OnScoreChanged += UpdateScores;
       GameStateManager.Instance.OnMatchStarted += StartMatch;
    }
    private void OnDisable()
    {
       GameManager.Instance.OnScoreTeamA -= ScoreTeamA;
       GameManager.Instance.OnScoreTeamA -= ScoreTeamB;
       GameManager.Instance.OnTimeChanged -= UpdateTimer;
       GameManager.Instance.OnScoreChanged -= UpdateScores;
       GameStateManager.Instance.OnMatchStarted -= StartMatch;
>>>>>>> Stashed changes
    }
    


    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateScores(int scoreA,int scoreB)
    {
        // ( _textScoreTeamA).text = scoreA.ToString();
        // ( _textScoreTeamB).text = scoreB.ToString();
    }
   

    void ShowPauseMenu()
    {
        
    }

    void HidePauseMenu()
    {
        
    }
    void ShowMainMenu()
    {
        
    }

    void HideMainMenu()
    {
        
    }
    private IEnumerator PanelGolA()
    {
       _panelGolA.SetActive(true);
       yield return new WaitForSeconds(3f);
       _panelGolA.SetActive(false);
    }
    private IEnumerator PanelGolB()
    {
       _panelGolB.SetActive(true);
       yield return new WaitForSeconds(3f);
       _panelGolB.SetActive(false);
    }
    void ScoreTeamA()
    {
        StartCoroutine(PanelGolA());
    }
    void ScoreTeamB()
    {
        StartCoroutine(PanelGolB());
    }

   

    void MostrarPantallaFinal()
    {
        
    }

    void ShowHud()
    {
        
    }

    public void UpdateTimer(float timeInSeconds)
    {   
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        _textTime.text = string.Format("{0:00}:{1:00}",minutes,seconds);
    }

     public void OcultarMenuPausa()
    {
<<<<<<< Updated upstream
        _panelPausa.SetActive(false);
    }

     public void OcultarPantallaFinal()
    {
        _panelPantallaFinal.SetActive(false);
    }

     public void OcultarPantallaInicial()
    {
        _panelPantallaInicial.SetActive(false);
    }

    private void HandleGameStateChanged(GameState newGameState)
    {
        switch (newGameState)
        {
            case GameState.Playing:
                ShowHud();
                break;
            case GameState.MainMenu:
                ShowMainMenu();
                break;
            //etc
            
        }
    }

=======
        _textScoreTeamA.text = _goalsTeam_A.ToString();
        _textScoreTeamB.text = _goalsTeam_B.ToString();
    }
>>>>>>> Stashed changes
}
