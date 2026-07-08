using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
    [SerializeField] private float _intialTimeInSecods = 180;
    private float _timeLeft;

    public event Action<Sprite,Sprite,int,int> OnDrawGame;

    public event Action OnScoreTeamA;
    public event Action OnScoreTeamB;

    public event Action<int,int> OnEnded;
    public event Action<Sprite,Sprite> OnSelectedTeams;
   public event Action<int,int> OnScoreChanged;
   public event Action<float> OnTimeChanged;
   public event Action<Sprite,Sprite> OnResults;
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
    
    [Header("Flags")]
    public List<Sprite> Shirt;
    public List<Sprite> Flags;
    public int CountrySelectedTeamA = 1;
    public int CountrySelectedTeamB = 0;
    private Sprite SpriteShirtTeamA;
    private Sprite SpriteShirtTeamB;
    private Sprite SpriteFlagTeamA;
    private Sprite SpriteFlagTeamB;
    public event Action<Sprite> OnChangeFlagTeamA;
    public event Action<Sprite> OnChangeCountryTeamA;
    public event Action<Sprite> OnChangeFlagTeamB;
    public event Action<Sprite> OnChangeCountryTeamB;
    private bool botsAlreadySpawned = false;
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
        GameStateManager.Instance.OnMainSelectedTeamMenu += SelectTeam;
    }
    void OnDisable()
    {
        GameStateManager.Instance.OnMatchEnded -= FinishMatch;
        GameStateManager.Instance.OnMatchStarted -= StartMatch;
        GameStateManager.Instance.OnMatchPaused -= PauseMatch;
        GameStateManager.Instance.OnMatchRestart -= RestartGame;
        GameStateManager.Instance.OnGoalScored -= RegisterGoal;
        GameStateManager.Instance.OnMainSelectedTeamMenu -= SelectTeam;
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
        MatchEndUIDecition();
    }

    public float GetTiempoRestante()
{
    return _tiempoRestante;
}
    

    public void ChangedCountryUpTeamA()
    {
        CountrySelectedTeamA ++;
        if (CountrySelectedTeamA > 7)
        {
            CountrySelectedTeamA = 0;
        }
        SpriteShirtTeamA = Shirt[CountrySelectedTeamA];
        OnChangeCountryTeamA?.Invoke(SpriteShirtTeamA);
        SpriteFlagTeamA = Flags[CountrySelectedTeamA];
        OnChangeFlagTeamA?.Invoke(SpriteFlagTeamA);
    }
    public void ChangedCountryDownTeamA()
    {
        CountrySelectedTeamA --;
        if (CountrySelectedTeamA < 0)
        {
            CountrySelectedTeamA = 7;
        }
        SpriteShirtTeamA = Shirt[CountrySelectedTeamA];
        OnChangeCountryTeamA?.Invoke(SpriteShirtTeamA);
        SpriteFlagTeamA = Flags[CountrySelectedTeamA];
        OnChangeFlagTeamA?.Invoke(SpriteFlagTeamA);
    }
    public void ChangedCountryUpTeamB()
    {
        CountrySelectedTeamB ++;
        if (CountrySelectedTeamB > 7)
        {
            CountrySelectedTeamB = 0;
        }
        SpriteShirtTeamB = Shirt[CountrySelectedTeamB];
        OnChangeCountryTeamB?.Invoke(SpriteShirtTeamB);
        SpriteFlagTeamB = Flags[CountrySelectedTeamB];
        OnChangeFlagTeamB?.Invoke(SpriteFlagTeamB);
    }
    public void ChangedCountryDownTeamB()
    {
        CountrySelectedTeamB --;
        if (CountrySelectedTeamB < 0)
        {
            CountrySelectedTeamB = 7;
        }
        SpriteShirtTeamB = Shirt[CountrySelectedTeamB];
        OnChangeCountryTeamB?.Invoke(SpriteShirtTeamB);
        SpriteFlagTeamB = Flags[CountrySelectedTeamB];
        OnChangeFlagTeamB?.Invoke(SpriteFlagTeamB);
    }
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


        if (botsAlreadySpawned)
        {
            return;
        }

        _teamAController.SpawnBots();
        _teamBController.SpawnBots();

        botsAlreadySpawned = true;

    }
    public void SelectTeam()
    {
        SpriteShirtTeamA = Shirt[CountrySelectedTeamA];
        SpriteShirtTeamB = Shirt[CountrySelectedTeamB];
        SpriteFlagTeamB = Flags[CountrySelectedTeamB];
        SpriteFlagTeamA = Flags[CountrySelectedTeamA];
        OnSelectedTeams?.Invoke(SpriteShirtTeamA, SpriteShirtTeamB);
        OnChangeFlagTeamB?.Invoke(SpriteFlagTeamB);
        OnChangeFlagTeamA?.Invoke(SpriteFlagTeamA);
    }

    public void ActivateEndEvent()
    {
        
    }

    public string HexadecimalColorByCountryPosition(int countryPositionOnList)
    {   
        string hexcolor;
        switch (countryPositionOnList)
        {
            case 1:
                hexcolor = "#FFFFFF";
                break;
            case 2:
                hexcolor = "#70AFDA";
                break;
            case 3:
                hexcolor =  "#056E38";
                break;
            case 4:
                hexcolor = "#F7DB0D" ;
                break;
            case 5:
                hexcolor = "#09236F";
                break;
            case 6:
                hexcolor = "#A61327";
                break;
            case 7:
                hexcolor ="#2A3655";
                break;
            default:
                hexcolor = "#0A60AB";
                break;

        }
        return hexcolor;
    }

    public Color ObtainColorByHexa(string hexadecimalColor)
    {
     
        Color color;

        ColorUtility.TryParseHtmlString(hexadecimalColor, out color);

        return color;
    }

    public Team? WhoIsTheWinner()
    {   
        if (_goalsTeam_A > _goalsTeam_B)
        {
            return Team.A;
        }
        else if (_goalsTeam_B > _goalsTeam_A)
        {
            return Team.B;
        }
        else
        {
            return null;
        }
    } 

    public Sprite TeamCountryWinnerSprite()
    {
        if(WhoIsTheWinner() == Team.A)
        {
            return Flags[CountrySelectedTeamA];
        }
        else
        {
            return Flags[CountrySelectedTeamB];
        }
    }

    public Sprite TeamCountryLoserSprite()
    {   if(WhoIsLoser() == Team.A){ 

        return Flags[CountrySelectedTeamA] ;
        }
        else
        {
            return Flags[CountrySelectedTeamB]; 
        }
         
    }

    public Team WhoIsLoser()
    {
        Team loser;
        if(_goalsTeam_B > _goalsTeam_A)
        {
            loser = Team.A;
        }
        else
        {
            loser = Team.B;
        }
        return loser;
    }

    void MatchEndUIDecition()
    {
        if(WhoIsTheWinner() == null)
        {
            DrawFinish();
        }
        else
        {
            MatchEndedWithWinner();
        }
    }

    void DrawFinish()
    {
        OnDrawGame?.Invoke(Flags[CountrySelectedTeamA],Flags[CountrySelectedTeamB],_goalsTeam_A,_goalsTeam_B);
    }

    void MatchEndedWithWinner()
    {
        OnResults?.Invoke(TeamCountryWinnerSprite(),TeamCountryLoserSprite());
    }


    

}
