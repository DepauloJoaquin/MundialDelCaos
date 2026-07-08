using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

public class AIBehaviour : MonoBehaviour
{   
    private PlayerController _currentPlayer;
    private float _timer = 0f;
    private float _timeBetweenUpdates = 0.2f;
    private Ball _ball;
    private PlayerController _player1;
    private PlayerController _player2;

    public float _followSpeed = 15f;
    public float _slowdownDistance = 1f;
    [SerializeField] private float stopChasingBallDistance = 0.5f;
    const float spreadAssistFactor = 1f;
    [SerializeField] private float runDistance = 2.5f;
    [SerializeField] private float arriveDistance = 0.3f;
    [SerializeField] private float runMultiplier = 1.7f;
    [SerializeField] private float stealDistance = 0.5f;

    Vector2 velocity = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        _currentPlayer = GetComponent<PlayerController>();
        _ball = GameManager.Instance._ball.GetComponent<Ball>();
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
         if(_timer>= _timeBetweenUpdates)
        {
            _timer = 0;
            Process_AI();
            _timeBetweenUpdates = Random.Range(0.15f,0.35f);
        }
    }
       
       
    void Process_AI()
    {
      
        Perform_AI_Movement();
       Perform_AI_Decisions();
       
    }
    void Perform_AI_Movement()
    {
        if (_currentPlayer._role == PlayerController.Role.GoalKeeper)
        {
            StopBot();
            return;
        }
        if (!TeamHasPlayer1AndPlayer2())
        {
        _currentPlayer._movementDirection = Vector2.zero;
        _currentPlayer._isBotRunning = false;
        return;
        }
        Vector2 totalMovement = GetBotMovement();
        bool shouldRun = ShouldBotRun();

        totalMovement = Vector2.ClampMagnitude(totalMovement, 1f);
        ApplyBotMovement(totalMovement, shouldRun);
        
      
    }

    void Perform_AI_Decisions()
    {
        TryStealBall();
    }

   public Vector2 CalculateMovementToBall()
{
    float distanceToBall = _currentPlayer.DistanceTo(GameManager.Instance._ball.transform.position);

    Vector2 directionToBall = _currentPlayer.DirectionTo(GameManager.Instance._ball.transform.position);

    float speedFactor = 1f;

    if (distanceToBall <= stopChasingBallDistance)
    {
        speedFactor = Mathf.Clamp(distanceToBall / stopChasingBallDistance, 0.25f, 1f);
    }

    return _currentPlayer._forceTowardsTheBall * speedFactor * directionToBall;
}

    public float GetBicircularWeight(Vector2 playerPosition, Vector2 centerTarget,float innerCircleWeight, float innerCircleRadius,float outerCircleWeight,float outerCircleRadius)
    {
        float distanceToCenter = Vector2.Distance(playerPosition,centerTarget);
        if(distanceToCenter > outerCircleRadius)
        {
            return outerCircleWeight;
        }
        else if(distanceToCenter < innerCircleRadius)
        {
            return innerCircleWeight;
        }
        else
        {
            float distanceToInnerRadius = distanceToCenter - innerCircleRadius;
            float closeRangeDistance =  outerCircleRadius - innerCircleRadius;
            return Mathf.Lerp(innerCircleWeight,outerCircleWeight,distanceToInnerRadius/closeRangeDistance);
        }
    }

    /*public Vector2 GetMovementTowardsGoal()
    {
        Vector2 target = _currentPlayer._myTeamController._OurGoalScript.GetEnemyGoalFirstTargetPosition();
        Vector2 direction = _currentPlayer.DirectionTo(target);
        float weight = GetBicircularWeight(_currentPlayer._position,target,100,0,150,1);
        return weight * direction;
    }*/

    public bool isBallCarriedByTeamMate()
    {
        return _ball._currentOwner != null && _ball._currentOwner != _currentPlayer && _ball._currentOwnerController._team == _currentPlayer._team;
    }

    public Vector2 GetAssistFormationMovement()
    {
        Vector2 spawn_difference = _ball._currentOwnerController._spawnPosition - _currentPlayer._spawnPosition;
        Vector2 assistDestination = _ball._currentOwnerController._position - spawn_difference * spreadAssistFactor;
         float distanceToAssistPosition = Vector2.Distance(
        _currentPlayer._position,
        assistDestination
        );
        if (distanceToAssistPosition <= arriveDistance)
        {
            return Vector2.zero;
        }
        Vector2 direction = _currentPlayer.DirectionTo(assistDestination);
        float weight = GetBicircularWeight(_currentPlayer._position,assistDestination,0.2f,arriveDistance,1f,2f);
        return weight * direction;
    }

    void ApplyBotMovement(Vector2 direction,bool shouldRun)
    {
        
        if (direction.magnitude < 0.01f)
        {
            _currentPlayer._movementDirection = Vector2.zero;
            _currentPlayer._isBotRunning = false;
            return;
        }

        direction = Vector2.ClampMagnitude(direction, 1f);

        _currentPlayer._isBotRunning = shouldRun;
        _currentPlayer._movementDirection = direction * _currentPlayer.velocity;

    }

    private bool TeamHasPlayer1AndPlayer2()
    {
    if (_currentPlayer == null) return false;
    if (_currentPlayer._myTeamController == null) return false;

    return _currentPlayer._myTeamController._currentSelectedPlayer1 != null &&
           _currentPlayer._myTeamController._currentSelectedPlayer2 != null;
    }

    private bool AmIClosestBotSpawnToBall()
    {
        return _currentPlayer._myTeamController.SortedListOfPlayersByClosestToBall()[0] == _currentPlayer.GetComponent<PlayerController>();
    }
    private Vector2 GetBotMovement()
    {
    if (isBallCarriedByTeamMate())
    {
        return GetAssistFormationMovement();
    }
    if (IsBallCarriedByEnemy())
    {
        return GetEnemyPossessionMovement();
    }

    if (AmIClosestBotSpawnToBall())
    {
        return CalculateMovementToBall();
    }

    return Vector2.zero;
    }

    void StopBot()
    {   _currentPlayer._movementDirection = Vector2.zero;
        _currentPlayer._isBotRunning = false;
    }

    private bool ShouldBotRun()
    {
        if (isBallCarriedByTeamMate())
        {
            return _ball._currentOwnerController.IsRunning();   
        }
        if (IsBallCarriedByEnemy())
        {
            return ShouldRunDuringEnemyPossession();
        }

        if (AmIClosestBotSpawnToBall())
        {
            float distanceToBall = _currentPlayer.DistanceTo(_ball.transform.position);

            if (distanceToBall <= stopChasingBallDistance)
            {
                return false;
            }

            return distanceToBall > runDistance;
        }

        return false;
    }
    private bool IsBallCarriedByEnemy()
    {
    return _ball._currentOwnerController != null &&
           _ball._currentOwnerController._team != _currentPlayer._team;
    }
    private Vector2 GetMovementToEnemyBallOwner()
    {   
    PlayerController enemyOwner = _ball._currentOwnerController;

    if (enemyOwner == null)
    {
        return Vector2.zero;
    }

    float distanceToEnemy = _currentPlayer.DistanceTo(enemyOwner._position);

    if (distanceToEnemy <= arriveDistance)
    {
        return Vector2.zero;
    }

    return _currentPlayer.DirectionTo(enemyOwner._position);
    }
    private void TryStealBall()
    {
    if (!IsBallCarriedByEnemy())
    {
        return;
    }
     if (!AmIClosestBotToEnemyOwner())
    {
        return;
    }

    PlayerController enemyOwner = _ball._currentOwnerController;

    float distanceToEnemy = _currentPlayer.DistanceTo(enemyOwner._position);

    if (distanceToEnemy > stealDistance)
    {
        return;
    }

    _ball.GiveBallTo(_currentPlayer);
    }
    private Vector2 GetMovementToSpawnPosition()
    {
        float distanceToSpawn = _currentPlayer.DistanceTo(_currentPlayer._spawnPosition);

        if (distanceToSpawn <= arriveDistance)
        {
            return Vector2.zero;
        }

        return _currentPlayer.DirectionTo(_currentPlayer._spawnPosition);
    }

    private bool AmIClosestBotToEnemyOwner()
    {
    PlayerController enemyOwner = _ball._currentOwnerController;


    if (enemyOwner == null)
    {
        return false;
    }
    List<PlayerController> botsClosestsToEnemy = _currentPlayer._myTeamController.BotsThatAreNotGoalkeepers().OrderBy(bot => bot.DistanceTo(enemyOwner._position)).ToList();
    return botsClosestsToEnemy[0] == _currentPlayer;
}

    private Vector2 GetEnemyPossessionMovement()
    {
        if (AmIClosestBotToEnemyOwner())
        {
            return GetMovementToEnemyBallOwner();
        }

        return GetMovementToSpawnPosition();
    }

    private bool  ShouldRunDuringEnemyPossession()
    {
        if (AmIClosestBotToEnemyOwner())
        {
        PlayerController enemyOwner = _ball._currentOwnerController;
        return _currentPlayer.DistanceTo(enemyOwner._position) > runDistance;
        }
        return _currentPlayer.DistanceTo(_currentPlayer._spawnPosition) > runDistance;
    }
 




}
