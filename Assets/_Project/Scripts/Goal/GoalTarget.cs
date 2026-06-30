using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTarget : MonoBehaviour
{   
    // Start is called before the first frame update
    public List<GameObject> _targets;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2 get_center_target_position()
    {
        return _targets[1].transform.position;
    }
}
