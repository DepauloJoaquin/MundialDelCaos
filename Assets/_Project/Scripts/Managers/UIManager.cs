using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem.Controls;
using Unity.Collections;
using System;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
     // Responsabilidad:
    // Controla la interfaz del juego.
    // No decide la lógica de la partida, solo muestra información visual.
    [Header("HUD")]
    [SerializeField] private TextMeshProUGUI _textTime;
    [SerializeField] private TextMeshProUGUI _textScoreTeamA;
    [SerializeField] private TextMeshProUGUI _textScoreTeamB;

    [SerializeField] private TextMeshProUGUI _textScoreTeamADraw;
    [SerializeField] private TextMeshProUGUI _textScoreTeamBDraw;

    [SerializeField] private Sprite EquipoA;
    [SerializeField] private Sprite EquipoB;

    [Header("Mensajes")]
    [SerializeField] private GameObject _panelGolA;
    [SerializeField] private GameObject _panelGolB;
    [SerializeField] private GameObject _panelPausa;
    [SerializeField] private GameObject _panelPantallaFinal;
    [SerializeField] private GameObject _panelPantallaInicial;
    [SerializeField] private GameObject _panelPantallaEmpate;


    [Header("Country")]
    [SerializeField] private GameObject _shirtTeamA;
    [SerializeField] private GameObject _shirtTeamB;
    [SerializeField] private GameObject _flagTeamAHub;
    [SerializeField] private GameObject _flagTeamBHub;
    [SerializeField] private GameObject _flagGoalTeamA;
    [SerializeField] private GameObject _flagGoalTeamB;
    [SerializeField] private GameObject _flagWinnerTeam;
    [SerializeField] private GameObject _flagLoserTeam;
    [SerializeField] private GameObject _flagDrawTeamA;
    [SerializeField] private GameObject _flagDrawTeamB;

    
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
       GameManager.Instance.OnChangeCountryTeamA += UpdateShirtTeamA;
       GameManager.Instance.OnChangeCountryTeamB += UpdateShirtTeamB;
       GameManager.Instance.OnChangeFlagTeamA += UpdateFlagsTeamA;
       GameManager.Instance.OnChangeFlagTeamB += UpdateFlagsTeamB;
       GameManager.Instance.OnDrawGame += DrawFinish;
       GameManager.Instance.OnScoreTeamA += ScoreTeamA;
       GameManager.Instance.OnScoreTeamB += ScoreTeamB;
       GameManager.Instance.OnTimeChanged += UpdateTimer;
       GameManager.Instance.OnScoreChanged += UpdateScores;
       GameManager.Instance.OnSelectedTeams += SelectedTeams;
       GameManager.Instance.OnResults += MatchEnd;

    }
    private void OnDisable()
    {
       GameManager.Instance.OnChangeCountryTeamA -= UpdateShirtTeamA;
       GameManager.Instance.OnChangeCountryTeamB -= UpdateShirtTeamB;
       GameManager.Instance.OnChangeFlagTeamA -= UpdateFlagsTeamA;
       GameManager.Instance.OnChangeFlagTeamB -= UpdateFlagsTeamB;
       GameManager.Instance.OnDrawGame -= DrawFinish;
  
       GameManager.Instance.OnScoreTeamA -= ScoreTeamA;
       GameManager.Instance.OnScoreTeamA -= ScoreTeamB;
       GameManager.Instance.OnTimeChanged -= UpdateTimer;
       GameManager.Instance.OnScoreChanged -= UpdateScores;
       GameManager.Instance.OnSelectedTeams -= SelectedTeams;
       GameManager.Instance.OnResults -= MatchEnd;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateFlagsTeamA(Sprite SpriteTeamA)
    {
        _flagTeamAHub.GetComponent<Image>().sprite = SpriteTeamA;
        _flagGoalTeamA.GetComponent<Image>().sprite = SpriteTeamA;
    }
    void UpdateFlagsTeamB(Sprite SpriteTeamB)
    {
        _flagTeamBHub.GetComponent<Image>().sprite = SpriteTeamB;
        _flagGoalTeamB.GetComponent<Image>().sprite = SpriteTeamB;
    }

    void ShowPauseMenu()
    {
        
    }

    void RestartHud()
    {
        
    }

    void ShowEndMenu()
    {
        _panelPantallaFinal.SetActive(true);
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
    void MatchEnd(Sprite winnerTeam,Sprite loserTeam)
    {    ShowEndMenu();
        _flagWinnerTeam.GetComponent<Image>().sprite = winnerTeam;
        _flagLoserTeam.GetComponent<Image>().sprite = loserTeam;
        
    }
    void SelectedTeams(Sprite TeamADefaut, Sprite TeamBDefaut)
    {
        _shirtTeamA.GetComponent<Image>().sprite = TeamADefaut;
        _shirtTeamB.GetComponent<Image>().sprite = TeamBDefaut;
    }
    void UpdateShirtTeamA(Sprite TeamA)
    {
        _shirtTeamA.GetComponent<Image>().sprite = TeamA;
    }
    void UpdateShirtTeamB(Sprite TeamB)
    {
        _shirtTeamB.GetComponent<Image>().sprite = TeamB;
    }


    void ShowHud()
    {
        
    }

    void DrawFinish(Sprite spriteTeamA,Sprite SpriteTeamB,int TeamAScore,int TeamBScore)
    {   
        _flagDrawTeamA.GetComponent<Image>().sprite = spriteTeamA;
        _flagDrawTeamB.GetComponent<Image>().sprite = SpriteTeamB;
        _textScoreTeamADraw.text = TeamAScore.ToString();
        _textScoreTeamBDraw.text = TeamBScore.ToString();
        ShowDrawUI();


    }

    void ShowDrawUI()
    {
        _panelPantallaEmpate.SetActive(true);
    }

    public void StartMatch()
    {
        
    }

    public void UpdateTimer(float timeInSeconds)
    {   
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
