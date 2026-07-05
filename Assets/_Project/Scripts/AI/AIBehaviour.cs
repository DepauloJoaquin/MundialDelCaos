using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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

    const float spreadAssistFactor = 1f;
    [SerializeField] private float runDistance = 2.5f;
    [SerializeField] private float arriveDistance = 0.3f;
    [SerializeField] private float runMultiplier = 1.7f;

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
        //Perform_AI_Decisions();
    }
    void Perform_AI_Movement()
    {
        if (_currentPlayer._role == PlayerController.Role.GoalKeeper)
        {
        _currentPlayer._movementDirection = Vector2.zero;
        _currentPlayer._isBotRunning = false;
        return;
        }
         Vector2 totalMovement = Vector2.zero;
         bool shouldRun = false;
        
            if (isBallCarriedByTeamMate())
            {
                totalMovement += GetAssistFormationMovement();
                PlayerController ballOwner = _ball._currentOwnerController;
                shouldRun = ballOwner.IsRunning();

            }
            else
            {
                totalMovement += CalculateMovementToBall();
                shouldRun = _currentPlayer.DistanceTo(_ball.transform.position) > runDistance;
            }
            totalMovement = Vector2.ClampMagnitude(totalMovement,1f);
            ApplyBotMovement(totalMovement,shouldRun);
        
      
    }

    void Perform_AI_Decisions()
    {
   

    }

    public Vector2 CalculateMovementToBall()
    {
        return _currentPlayer._forceTowardsTheBall * _currentPlayer.DirectionTo(GameManager.Instance._ball.transform.position);
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

    /*public Vector2 GetMovementTowardsBall()
    {
        Vector2 target = _currentPlayer._targetGoal.get_center_target_position();
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


}
