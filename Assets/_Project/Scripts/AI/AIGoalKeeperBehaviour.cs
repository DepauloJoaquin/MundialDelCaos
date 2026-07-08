using System.Collections.Generic;
using UnityEngine;

public class AIGoalKeeperBehaviour : MonoBehaviour
{
    private List<GameObject> _MyGoalPositions;
    private PlayerController _currentPlayer;
    private TeamController _enemyTeamController;
    [SerializeField] private float arriveDistance = 0.6f;
    private Ball _ball;
    private bool _initialized = false;
    [SerializeField] private float diveCooldown = 1.2f;
    private float _lastDiveTime = -999f;


    public void Initialize()
    {
        _currentPlayer = GetComponent<PlayerController>();

        _initialized = true;

        _ball = GameManager.Instance._ballController;
         _MyGoalPositions = _currentPlayer
        ._myTeamController
        ._OurGoalScript
        ._currentGoalMovementPositionsForGoalkeeper;

    }



    void Update()
    {
    if (!_initialized)
    {
        Initialize();
        return;
    }
    Process_AI();
    }   
    void Process_AI()
    {
        if (_currentPlayer.playerStateManager.IsCurrentState(_currentPlayer.playerStateManager.diveState))
        {
        return;
        }
        Perform_AI_Decisions();
        if (_currentPlayer.playerStateManager.IsCurrentState(_currentPlayer.playerStateManager.diveState))
    {
        return;
    }
        Perform_AI_Movement();
    }

   void Perform_AI_Movement()
    {   Vector2 totalMovement;
        if (ShouldGoalkeeperMove())
        {
            totalMovement = GetGoalkeeperMovement();
        }
        else
        {
            totalMovement = GetGoalKeeperReturnToCenterMovement();
        }
        _currentPlayer._movementDirection = totalMovement * _currentPlayer.velocity;
    }

   void Perform_AI_Decisions()
{   

    if (Time.time < _lastDiveTime + diveCooldown)
    {
        return;
    }
  

    //Debug.Log("BallIsDangerous = " + dangerous);

    if (BallIsDangerous())
    {
        _lastDiveTime = Time.time;

        _currentPlayer.playerStateManager.ChangeState(
            _currentPlayer.playerStateManager.diveState
        );
    }
}

    public Vector2 GetGoalkeeperMovement()
    {
        Vector2 topPosition = _MyGoalPositions[0].transform.position;
        Vector2 centerPosition = _MyGoalPositions[1].transform.position;
        Vector2 bottomPosition = _MyGoalPositions[2].transform.position;
        float target_y = Mathf.Clamp(_ball.transform.position.y,bottomPosition.y,topPosition.y);
        Vector2 destination =  new Vector2(centerPosition.x,target_y);
        Vector2 direction = _currentPlayer.DirectionTo(destination);
        float distanceToDestination = _currentPlayer.DistanceTo(destination);
        float weight = Mathf.Clamp(distanceToDestination/arriveDistance,0,1);
        return weight * direction;
    }
private bool BallIsDangerous()
{
    if (IsBallFreeAndNearMyGoal())
    {
        return true;
    }

    if (IsBallHeadedTowardsMyGoal())
    {
        return true;
    }

    return false;
}

    private bool IsBallFreeAndNearMyGoal()
    {   float distanceBetweenBallAndGoal = Vector2.Distance(_ball.transform.position,_currentPlayer._myTeamController._thisTeamGoal.transform.position);

        return _ball._currentOwner == null && distanceBetweenBallAndGoal <= 5f;
    }

    private bool IsBallHeadedTowardsMyGoal()
{
    Vector2 direction = _ball.GetRaycastDirection();

    //Debug.Log("Ball velocity direction = " + direction);

    if (direction.magnitude < 0.01f)
    {
       // Debug.Log("La pelota no tiene velocidad suficiente para raycast.");
        return false;
    }

    RaycastHit2D hit = Physics2D.Raycast(
        _ball.transform.position,
        direction.normalized,
        _ball.RayDistance,
        LayerMask.GetMask("Arco")
    );
     
    if (hit.collider == null)
    {
        return false;
    }
    Transform hitTransform = hit.collider.transform;
    Transform myGoalTransform = _currentPlayer._myTeamController._OurScoreZone.transform;

    Debug.Log("Raycast tocó: " + hit.collider.gameObject.name);
    Debug.Log("OurScoreZone es: " + _currentPlayer._myTeamController._OurScoreZone.name);

    return hitTransform == myGoalTransform ;
}



    private bool ShouldGoalkeeperMove()
    {
    if (IsBallNearMyGoal())
    {
        return true;
    }

    if (IsBallHeadedTowardsMyGoal())
    {
        return true;
    }

    return false;
    }
    public Vector2 GetGoalKeeperReturnToCenterMovement()
    {
        Vector2 centerPoint = _MyGoalPositions[1].transform.position;
        Vector2 direction = _currentPlayer.DirectionTo(centerPoint);
        float distanceToDestination = _currentPlayer.DistanceTo(centerPoint);
        float weight = Mathf.Clamp01(distanceToDestination / arriveDistance);
        return weight * direction;

    }
    private bool IsBallNearMyGoal()
{
    float distanceBetweenBallAndGoal = Vector2.Distance(
        _ball.transform.position,
        _currentPlayer._myTeamController._thisTeamGoal.transform.position
    );

    return distanceBetweenBallAndGoal <= 6f;
}

}