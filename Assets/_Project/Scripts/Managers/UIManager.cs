using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem.Controls;
using Unity.Collections;

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

    [SerializeField] private TextMeshProUGUI _textScoreTeamADraw;
    [SerializeField] private TextMeshProUGUI _textScoreTeamBDraw;

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

       GameManager.Instance.OnDrawGame += DrawFinish;
       GameManager.Instance.OnScoreTeamA += ScoreTeamA;
       GameManager.Instance.OnScoreTeamB += ScoreTeamB;
       GameManager.Instance.OnTimeChanged += UpdateTimer;
       GameManager.Instance.OnScoreChanged += UpdateScores;
       GameStateManager.Instance.OnMatchStarted += StartMatch;
    }
    private void OnDisable()
    {
       GameManager.Instance.OnDrawGame -= DrawFinish;
       GameManager.Instance.OnScoreTeamA -= ScoreTeamA;
       GameManager.Instance.OnScoreTeamA -= ScoreTeamB;
       GameManager.Instance.OnTimeChanged -= UpdateTimer;
       GameManager.Instance.OnScoreChanged -= UpdateScores;
       GameStateManager.Instance.OnMatchStarted -= StartMatch;
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
    void MatchEnd()
    {
        
    }


    void ShowHud()
    {
        
    }

    void DrawFinish(int TeamA,int TeamB)
    {
        
    }

    public void StartMatch()
    {
        //Mostrar Hud y lo que sea necesario
    }

    public void UpdateTimer(float timeInSeconds)
    {   Debug.Log("Timer activado");
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        _textTime.text = string.Format("{0:00}:{1:00}",minutes,seconds);

    }

    public void UpdateScores(int _goalsTeam_A,int _goalsTeam_B)
    {
        _textScoreTeamA.text = _goalsTeam_A.ToString();
        _textScoreTeamB.text = _goalsTeam_B.ToString();
    }


}
