using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalState : GameState
{
    public override void Enter()
    {
        GameManager.Instance.ActualizarMarcadores();
    }
}
