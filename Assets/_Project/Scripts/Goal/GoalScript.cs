using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalScript : MonoBehaviour
{   
    // Start is called before the first frame update
    public List<GameObject> _enemyGoalTargets;

    public List<GameObject> _currentGoalMovementPositionsForGoalkeeper;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2 GetEnemyGoalFirstTargetPosition()
    {
        return _enemyGoalTargets[0].transform.position;
    }

  
}
