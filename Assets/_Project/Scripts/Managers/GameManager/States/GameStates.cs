using UnityEngine;
using UnityEngine.SceneManagement;

public interface IGameState
{
    GameState StateType {get;}
    void Enter();
    void UpdateState();
    void Exit();
}

public abstract class BaseGameState : IGameState
{
    public abstract GameState StateType {get;}
    public virtual void Enter(){}
    public virtual void UpdateState(){}
    public virtual void Exit(){}
}

public class PauseState : BaseGameState
{
    public override GameState StateType => GameState.Paused;
    public override void Enter()
    {
        Time.timeScale = 0f;
    }

    public override void Exit()
    {
        Time.timeScale = 1f;
    }
}

public class StartState : BaseGameState
{
    public override GameState StateType => GameState.Start;
}

public class RestartState : BaseGameState
{
    public override GameState StateType => GameState.Restart;
    public override void Enter()
    {
       SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

public class PlayingState : BaseGameState
{
    public override GameState StateType => GameState.Playing;
}

public class MainMenuState : BaseGameState
{
    public override GameState StateType => GameState.MainMenu;
    public override void Enter()
    {
       Time.timeScale = 1f;
    }
}

public class OptionsMenuState : BaseGameState
{
    public override GameState StateType => GameState.OptionsMenu;
    public override void Enter()
    {
       Time.timeScale = 1f;
    }
}

public class SelecTeamState : BaseGameState
{
    public override GameState StateType => GameState.SelectTeamMenu;
    public override void Enter()
    {
       Time.timeScale = 1f;
    }
}

public class EndState : BaseGameState
{
    public override GameState StateType => GameState.End;
    public override void Enter()
    {
      Time.timeScale = 0f;
    }
}
public class GoalState : BaseGameState
{
    public override GameState StateType => GameState.Goal;
}




