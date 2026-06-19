
using UnityEngine;

public class PauseGameState : GameState
{
    public override void Enter()
    {
        Time.timeScale = 0f;
    }

    public override void Exit() {
        Time.timeScale = 1f;
    }

   
}
