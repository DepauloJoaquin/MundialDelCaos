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
        Perform_AI_Decisions2();
        if (_currentPlayer.playerStateManager.IsCurrentState(_currentPlayer.playerStateManager.diveState))
    {
        return;
    }
        Perform_AI_Movement();
    }

   void Perform_AI_Movement()
    {
    Vector2 totalMovement = GetGoalkeeperMovement();

    _currentPlayer._movementDirection = totalMovement * _currentPlayer.velocity;
    }

   void Perform_AI_Decisions()
{   

    if (Time.time < _lastDiveTime + diveCooldown)
    {
        return;
    }
    bool dangerous = BallIsDangerous();

    Debug.Log("BallIsDangerous = " + dangerous);

    if (BallIsDangerous())
    {
         Debug.Log("CAMBIO A DIVE");
        _lastDiveTime = Time.time;

        _currentPlayer.playerStateManager.ChangeState(
            _currentPlayer.playerStateManager.diveState
        );
    }
}
void Perform_AI_Decisions2()
{
    if (Input.GetKeyDown(KeyCode.K))
    {
        Debug.Log("TEST: CAMBIO FORZADO A DIVE");

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

    private  bool IsBallHeadedTowardsMyGoal()
    {

        RaycastHit2D hit =  Physics2D.Raycast(_ball.transform.position,_ball.velocityNormalized() , 1.5f,LayerMask.GetMask("Arco"));
         if (hit.collider == null)
        return false;
        return hit.collider.gameObject == _currentPlayer._myTeamController._OurScoreZone;
    }



    /*private bool ShouldGoalKeeperMove()
    {
        
    }*/

    //private 

    // HACER COMO UN TIMER EN PELOTA PRA SABER SI FUE PATEADA O NO PARA EL REYCAST
    //SEGUIR MAÑANA EL COMPORTAMIENTO
}