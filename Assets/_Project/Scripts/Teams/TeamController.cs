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
        
        public GameObject _thisTeamGoal;

        public GoalScript _OurGoalScript;

        public GameObject _OurScoreZone;

        private int _amountHumanPlayers = 0;
        public int _amountBots;

        [SerializeField] private GameObject _prefabPlayer;

        public PlayerController _currentSelectedPlayer1;
        public PlayerController _currentSelectedPlayer2;
        private string _player1ControlScheme;
        private InputDevice _player1Device;

        private string _player2ControlScheme;
        private InputDevice _player2Device;

        private int _currentBotPositionIndex = 0;

        void Awake()
        {
            _OurGoalScript = _thisTeamGoal.GetComponent<GoalScript>();
            _OurScoreZone = _thisTeamGoal.transform.GetChild(0).gameObject;
        }

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
            GameObject spawnPoint = _availablePositions[_currentBotPositionIndex];

            GameObject newPlayer = Instantiate(
                _prefabPlayer,
                spawnPoint.transform.position,
                Quaternion.identity
            );

            PlayerController playerController = newPlayer.GetComponent<PlayerController>();
            playerController._myTeamController = this;
            playerController._team = team;
            playerController._targetGoal = _OurGoalScript.GetEnemyGoalFirstTargetPosition();
            playerController._spawnPosition = spawnPoint.transform.position;
            playerController._position = spawnPoint.transform.position;
            playerController._formationSlot = _currentBotPositionIndex;
            if(_currentBotPositionIndex == 0)
            {
                playerController._role = PlayerController.Role.GoalKeeper;
            }
            else
            {
                playerController._role = PlayerController.Role.MidFilder;
            }
            playerController.selected = false;
            playerController.SetControlSlot(PlayerController.ControlSlot.Bot);

            _allPlayersControllers.Add(playerController);

            _currentBotPositionIndex += 1;
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
            if(receiver == null)
            {
            return;
            }
            
            if(receiver == _currentSelectedPlayer1 || receiver == _currentSelectedPlayer2)
            {
            return;
            }
            if(receiver._role == PlayerController.Role.GoalKeeper)
            {
            return;
            }

            if(previousOwner == null)
            {
                AssignPlayer1(receiver);
                return;
            }
           
            if(previousOwner == _currentSelectedPlayer1)
            {
                AssignPlayer1(receiver);
                return;
            }
            else if(previousOwner == _currentSelectedPlayer2)
            {
                AssignPlayer2(receiver);
                return;
            }
        } 
        void AssignPlayer1(PlayerController newPlayer)
        {
            if(_currentSelectedPlayer1 != null && _currentSelectedPlayer1 != newPlayer)
            {
                _currentSelectedPlayer1.selected = false;
                _currentSelectedPlayer1.SetControlSlot(PlayerController.ControlSlot.Bot);
            }
            _currentSelectedPlayer1 = newPlayer;
            _currentSelectedPlayer1.selected = true;
            _currentSelectedPlayer1.ConfigureInput(PlayerController.ControlSlot.Player1,_player1ControlScheme,_player1Device);
        }

        void AssignPlayer2(PlayerController newPlayer)
        {
            if(_currentSelectedPlayer2 != null && _currentSelectedPlayer2 != newPlayer)
            {
                _currentSelectedPlayer2.selected = false;
                _currentSelectedPlayer2.SetControlSlot(PlayerController.ControlSlot.Bot);
            }
            _currentSelectedPlayer2 = newPlayer;
            _currentSelectedPlayer2.selected = true;
            _currentSelectedPlayer2.ConfigureInput(PlayerController.ControlSlot.Player2,_player2ControlScheme,_player2Device);
        
        }

        private void SelectCurrentPlayersOnStart()
        {
            if (_currentSelectedPlayer1 != null)
            {
            _currentSelectedPlayer1.selected = true;
            _currentSelectedPlayer1.SetControlSlot(PlayerController.ControlSlot.Player1);
            }

            if (_currentSelectedPlayer2 != null)
            {
            _currentSelectedPlayer2.selected = true;
            _currentSelectedPlayer2.SetControlSlot(PlayerController.ControlSlot.Player2);
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
            int humanPositionIndex = _amountBots + _amountHumanPlayers;
            if (humanPositionIndex >= _availablePositions.Count)
            {
            return;
            }
            GameObject spawnPoint = _availablePositions[humanPositionIndex];

            GameObject newPlayer = Instantiate(
            _prefabPlayer,
            spawnPoint.transform.position,
            Quaternion.identity
            );

            if (team == Team.B)
            {
            newPlayer.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
            else
            {
            newPlayer.transform.rotation = Quaternion.identity;
            }

            PlayerController playerController = newPlayer.GetComponent<PlayerController>();

            playerController._myTeamController = this;
            playerController._team = team;
            playerController._targetGoal = _OurGoalScript.GetEnemyGoalFirstTargetPosition();
            playerController._spawnPosition = spawnPoint.transform.position;
            playerController._position = spawnPoint.transform.position;
            playerController._role = PlayerController.Role.Forward;
            _allPlayersControllers.Add(playerController);
            _amountHumanPlayers += 1;
    
            
            if (_amountHumanPlayers == 1)
            {
                _player1ControlScheme = controlScheme;
                _player1Device = device;
                AssignPlayer1(playerController);
            }
            else if (_amountHumanPlayers == 2)
            {
                _player2ControlScheme = controlScheme;
                _player2Device = device;
                AssignPlayer2(playerController);
            }
        }  
            
            public bool DoWeHaveTheBall()
            {
                return GameManager.Instance._ballController._currentOwnerController == _currentSelectedPlayer1 ||GameManager.Instance._ballController._currentOwnerController == _currentSelectedPlayer2 ;
            }
            public bool HasFreeHumanSlot()
            {
            return _amountHumanPlayers < 2;
            }

            

    }

    






        

