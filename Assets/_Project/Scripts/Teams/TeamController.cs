    using UnityEngine.InputSystem;
    using System.Collections.Generic;
    using UnityEngine;
    using System.Linq;

    public class TeamController : MonoBehaviour
    {   
        [SerializeField] private float forceFalloff = 0.5f;

        public List<PlayerController> _allPlayersControllers = new List<PlayerController>();

        private List<PlayerController> _currentSelectedPlayers = new List<PlayerController>();
        [SerializeField] private List<GameObject> _availablePositions;

        public Team team;
        
        public GameObject _thisTeamGoal;

        public GoalScript _OurGoalScript;

        public GameObject _OurScoreZone;

        private int _amountHumanPlayers = 0;
        public int _amountBots;

        private bool botsSpawned = false;

        [SerializeField] private GameObject _prefabPlayer;

        public PlayerController _currentSelectedPlayer1;
        public PlayerController _currentSelectedPlayer2;
        private string _player1BindingGroup;
        private InputDevice _player1Device;

        private string _player2BindingGroup;
        private InputDevice _player2Device;
        [SerializeField] private float forceUpdateInterval = 0.15f;
        private float forceUpdateTimer = 0f;
        private int _currentBotPositionIndex = 0;

        void Awake()
        {
            _OurGoalScript = _thisTeamGoal.GetComponent<GoalScript>();
            _OurScoreZone = _thisTeamGoal.transform.GetChild(0).gameObject;
        }

    void OnEnable()
    {
        GameStateManager.Instance.OnGoalScored += ResetAllPlayers;
    }
    void OnDisable()
    {
         GameStateManager.Instance.OnGoalScored -= ResetAllPlayers;
    }


    void Update()
        {
            forceUpdateTimer += Time.deltaTime;

            if (forceUpdateTimer >= forceUpdateInterval)
            {
                forceUpdateTimer = 0f;
                ApplyBaseForceTowardsBall();
            }
        }
    public void SpawnBots()
        {   Debug.Log("entre");
             if (botsSpawned)
            {
                return;
            }
            
            for (int i = 0; i < _amountBots; i++)
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
                SelectPlayerShirtByTeam(newPlayer,playerController);
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
            botsSpawned = true;

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

            if (previousOwner == _currentSelectedPlayer1)
            {
                AssignPlayer1(receiver);
                return;
            }

            if (previousOwner == _currentSelectedPlayer2)
            {
                AssignPlayer2(receiver);
                return;
            }

            AssignReceiverToClosestHumanSlot(receiver);

            
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
            _currentSelectedPlayer1.ConfigureInput(PlayerController.ControlSlot.Player1,_player1BindingGroup,_player1Device);
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
            _currentSelectedPlayer2.ConfigureInput(PlayerController.ControlSlot.Player2,_player2BindingGroup,_player2Device);
        
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
    foreach (PlayerController player in _allPlayersControllers)
    {
        player.transform.position = player._spawnPosition;
        player.transform.rotation = Quaternion.identity;

        player.rigidBody.velocity = Vector2.zero;
        player.rigidBody.angularVelocity = 0f;

        player.selected = false;
    }

    SelectCurrentPlayersOnStart();
}

        /*void ApplyAbilityToCurrentPlayer()
            {
                
            }
            */

        public void AddPlayerToTeam(string bindingGroup, InputDevice device)
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

            PlayerController playerController = newPlayer.GetComponent<PlayerController>();
            SelectPlayerShirtByTeam(newPlayer,playerController);

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
                _player1BindingGroup = bindingGroup;
                _player1Device = device;
                AssignPlayer1(playerController);
            }
            else if (_amountHumanPlayers == 2)
            {
                _player2BindingGroup = bindingGroup;
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

        public List<PlayerController> SortedListOfPlayersByClosestToBall()
        {
        return BotsThatAreNotGoalkeepers().OrderBy(player => player.DistanceTo(GameManager.Instance._ballController.transform.position)).ToList();
        }
        public List<PlayerController> BotsThatAreNotGoalkeepers(){
        return _allPlayersControllers.Where(player => player.controlSlot == PlayerController.ControlSlot.Bot && player._role !=PlayerController.Role.GoalKeeper).ToList();
        }
        public void ApplyBaseForceTowardsBall()
        {
        List<PlayerController> bots = BotsThatAreNotGoalkeepers();
        List<PlayerController> sortedBots = SortedListOfPlayersByClosestToBall();

        for (int i = 0; i < sortedBots.Count; i++)
        {
            float force = Mathf.Pow(forceFalloff, i);
            sortedBots[i]._forceTowardsTheBall = force;
        }
        }

        void AssignReceiverToClosestHumanSlot(PlayerController receiver)
        {
        float distanceToPlayer1 = Vector2.Distance(
            receiver.transform.position,
            _currentSelectedPlayer1.transform.position
        );

        float distanceToPlayer2 = Vector2.Distance(
            receiver.transform.position,
            _currentSelectedPlayer2.transform.position
        );

        if (distanceToPlayer1 <= distanceToPlayer2)
        {
            AssignPlayer1(receiver);
        }
        else
        {
            AssignPlayer2(receiver);
        }
        }

        void SelectPlayerShirtByTeam(GameObject newPlayer, PlayerController playerController)
        {
           if(team == Team.B)
            {   
                playerController.spriteRenderer.flipX = true;
                ShirtAnimationController shirtAnimationController =
                newPlayer.GetComponentInChildren<ShirtAnimationController>();
                int playerBCountry = GameManager.Instance.CountrySelectedTeamB;
                string hexadecimalColor = GameManager.Instance.HexadecimalColorByCountryPosition(playerBCountry);
                Color playerShirtColor = GameManager.Instance.ObtainColorByHexa(hexadecimalColor);
                shirtAnimationController.playerController = playerController;
                shirtAnimationController.ChangeColor(playerShirtColor);


            }
            else
            {   
                ShirtAnimationController shirtAnimationController =
                newPlayer.GetComponentInChildren<ShirtAnimationController>();
                int playerACountry = GameManager.Instance.CountrySelectedTeamA;
                string hexadecimalColor = GameManager.Instance.HexadecimalColorByCountryPosition(playerACountry);
                Color playerShirtColor = GameManager.Instance.ObtainColorByHexa(hexadecimalColor);
                shirtAnimationController.playerController = playerController;
                shirtAnimationController.ChangeColor(playerShirtColor);
            }
        }

       


    }

    






        

