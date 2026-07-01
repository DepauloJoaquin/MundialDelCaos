using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine;

public class TeamController : MonoBehaviour
{   
    //private List<GameObject> _players;

    private List<PlayerController> _allPlayersControllers = new List<PlayerController>();

    private List<PlayerController> _currentSelectedPlayers = new List<PlayerController>();
    [SerializeField] private List<GameObject> _availablePositions;

    public Team team;

    public Team team;

    public GoalTarget _targetGoal;

    private int _amountHumanPlayers = 0;

    public int _amountBots;

    [SerializeField] private GameObject _prefabPlayer;

    public PlayerController _currentSelectedPlayer1;
    public PlayerController _currentSelectedPlayer2;

    private int _currentPositionIndex = 0;

    

    void OnEnable()
    {
       // GameStateManager.Instance.OnGameStateChanged += HandleGameStateChanged;
    }

    void OnDisable()
    {
       // GameStateManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
    }

    /*void HandleGameStateChanged(GameState newGameState)
    {
        switch (newGameState)
        {
            case GameState.Pause:
                DeselectAllPlayers();
                break;
            case GameState.Start:
                SpawnBots(_amountBots);
                break;
            case GameState.Playing:
                SelectCurrentPlayersOnStart();
                break;
            case GameState.Restart:
                 ResetAllPlayers();
                 break;
        }
    }*/
    public void SpawnBots(int amountBots)
    {
        for (int i = 0; i < amountBots; i++)
        {
       
        GameObject spawnPoint = _availablePositions[_currentPositionIndex];

        GameObject newPlayer = Instantiate(
            _prefabPlayer,
            spawnPoint.transform.position,
            Quaternion.identity
        );

        PlayerController playerController = newPlayer.GetComponent<PlayerController>();
        playerController._myTeamController = this;

        playerController.selected = false;
        playerController.controlSlot = PlayerController.ControlSlot.Bot;

        _allPlayersControllers.Add(playerController);

        _currentPositionIndex += 1;
        }
    }
    public (PlayerController,PlayerController) CurrentPlayers()
    {    PlayerController firstPlayer = null;
         PlayerController secondPlayer = null;
        foreach(PlayerController playerController in _allPlayersControllers)
        {
          
            if(playerController.selected == true)
            {   
                if(firstPlayer == null)
                {
                     firstPlayer = playerController;
                }
                else if(secondPlayer == null)
                {
                    secondPlayer = playerController;
                }
        
            }
        }
        return (firstPlayer,secondPlayer);    
    }
    public void SelectPlayerWhoReceiveBall(PlayerController receiver,PlayerController previousOwner)
    {
        if(previousOwner == null)
        {
            AssignPlayer1(receiver);
            return;
        }
        if(previousOwner == _currentSelectedPlayer1)
        {
            AssignPlayer1(receiver);
        }
        else if(previousOwner == _currentSelectedPlayer2)
        {
            AssignPlayer2(receiver);
        }
    }

    public void SelectPlayerWhoReceiveBall(PlayerController receiver)
    {
        AssignPlayer1(receiver);
        return;
    }

    void AssignPlayer1(PlayerController newPlayer)
    {
        if(_currentSelectedPlayer1 != null)
        {
            _currentSelectedPlayer1.selected = false;
            _currentSelectedPlayer1.controlSlot = PlayerController.ControlSlot.Bot;
        }
        _currentSelectedPlayer1 = newPlayer;
        _currentSelectedPlayer1.selected = true;
        _currentSelectedPlayer1.controlSlot = PlayerController.ControlSlot.Player1;
    }

    void AssignPlayer2(PlayerController newPlayer)
    {
          if(_currentSelectedPlayer2 != null)
        {
            _currentSelectedPlayer2.selected = false;
            _currentSelectedPlayer2.controlSlot = PlayerController.ControlSlot.Bot;
        }
        _currentSelectedPlayer2 = newPlayer;
        _currentSelectedPlayer2.selected = true;
        _currentSelectedPlayer2.controlSlot = PlayerController.ControlSlot.Player2;
    }

    private void SelectCurrentPlayersOnStart()
    {
         if (_currentSelectedPlayer1 != null)
        {
        _currentSelectedPlayer1.selected = true;
        _currentSelectedPlayer1.controlSlot = PlayerController.ControlSlot.Player1;
        }

        if (_currentSelectedPlayer2 != null)
        {
        _currentSelectedPlayer2.selected = true;
        _currentSelectedPlayer2.controlSlot = PlayerController.ControlSlot.Player2;
        }
    }
    public void DeselectAllPlayers()
    {
        foreach(PlayerController playerController in _allPlayersControllers)
        {
            playerController.selected = false;
        }
    }

    public void ResetAllPlayers()
    {
      for (int i = 2; i < _allPlayersControllers.Count  ; i++)
    {

        PlayerController player = _allPlayersControllers[i];

        player.transform.position = _availablePositions[i].transform.position;
        player.transform.rotation = Quaternion.identity;

        player.rigidBody.velocity = Vector2.zero;
        player.rigidBody.angularVelocity = 0f;

        player.selected = false;
    }
    }

    /*void ApplyAbilityToCurrentPlayer()
        {
            
        }
        */

    public void AddPlayerToTeam(string controlScheme, InputDevice device)
    {   
        if (_amountHumanPlayers >= 2)
        {
        return;
        }
        if (_currentPositionIndex >= _availablePositions.Count)
        {
        return;
        }
        GameObject spawnPoint = _availablePositions[_currentPositionIndex];
        PlayerInput playerInput = PlayerInput.Instantiate(_prefabPlayer,controlScheme: controlScheme,pairWithDevice: device);

        playerInput.transform.position = spawnPoint.transform.position;
        playerInput.transform.rotation = Quaternion.identity;
        PlayerController playerController = playerInput.GetComponent<PlayerController>();
        playerController._myTeam = this;
        _allPlayersControllers.Add(playerController);
        _currentPositionIndex += 1;
        _amountHumanPlayers += 1;
        
        if (_amountHumanPlayers == 1)
    {
    AssignPlayer1(playerController);
    playerController.ConfigureInput(PlayerController.ControlSlot.Player1, controlScheme);
    }
else if (_amountHumanPlayers == 2)
{
    AssignPlayer2(playerController);
    playerController.ConfigureInput(PlayerController.ControlSlot.Player2, controlScheme);
}
       
        
        public bool DoWeHaveTheBall()
        {
        return GameManager.Instance._ball._currentOwnerController == _currentSelectedPlayer1 ||GameManager.Instance._ball._currentOwnerController == _currentSelectedPlayer2 ;
        }


    }

   

   





    

