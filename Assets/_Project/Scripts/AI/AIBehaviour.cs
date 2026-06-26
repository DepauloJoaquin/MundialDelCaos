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

    const float spreadAssistFactor = 0.8f;

    Vector2 velocity = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        
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
        Vector2 totalMovement = Vector2.zero;
        if (_currentPlayer._myTeamController.DoWeHaveTheBall())
        {
            totalMovement += GetCarrierSteeringforce();
        }
        else if(_currentPlayer._role != PlayerController.Role.GoalKeeper)
        {
            totalMovement += CalculateMovementToBall();
            if (isBallCarriedByTeamMate())
            {
                totalMovement += GetAssistFormationMovement();
            }
            totalMovement = Vector2.ClampMagnitude(totalMovement,0.1f);
            _currentPlayer._movementDirection = totalMovement * _currentPlayer._movementSpeed;
        }
      
    }

    void Perform_AI_Decisions()
    {
   

    }

    public Vector2 CalculateMovementToBall()
    {
        return _currentPlayer._forceTowardsTheBall * _currentPlayer.PlayerDirectionToBall();
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

    public Vector2 GetCarrierSteeringforce()
    {
        Vector2 target = _currentPlayer._targetGoal.get_center_target_position();
        Vector2 direction = _currentPlayer.DirectonTo(target);
        float weight = GetBicircularWeight(_currentPlayer._position,target,100,0,150,1);
        return weight * direction;
    }

    public bool isBallCarriedByTeamMate()
    {
        return _ball._carrier != null && _ball._carrier != _currentPlayer && _ball._carrier._team == _currentPlayer._team;
    }

    public Vector2 GetAssistFormationMovement()
    {
        Vector2 spawn_difference = _ball._carrier._spawnPosition - _currentPlayer._spawnPosition;
        Vector2 assistDestination = _ball._carrier._position - spawn_difference * spreadAssistFactor;
        Vector2 direction = _currentPlayer.DirectonTo(assistDestination);
        float weight = GetBicircularWeight(_currentPlayer._position,assistDestination,30,0.2f,60,1);
        return weight * direction;
    }


}
