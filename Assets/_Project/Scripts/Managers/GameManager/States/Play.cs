using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayState : GameState
{
    public override void Enter()
    {
        
    }

    public override void Update()
    {
        GameManager.Instance.ActualizarTiempo();
    }
}
