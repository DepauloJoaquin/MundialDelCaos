using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] private TeamController _teamAController;
    [SerializeField] private TeamController _teamBController;
    
    private HashSet<Gamepad> joinedGamepads = new HashSet<Gamepad>();

    private bool wasdJoined = false;
    private bool arrowsJoined = false;
    private bool botsTeamASpawned = false;
    private bool botsTeamBSpawned = false;

     void Update()
     {
        if (Keyboard.current != null)
        {
            if (!wasdJoined && Keyboard.current.enterKey.wasPressedThisFrame)
            {
                _teamAController.AddPlayerToTeam("WASD", Keyboard.current);
                wasdJoined = true;
            }

            if (!arrowsJoined && Keyboard.current.numpad7Key.wasPressedThisFrame)
            {
                _teamAController.AddPlayerToTeam("Arrows", Keyboard.current);
                arrowsJoined = true;
            }
        }
        
          foreach (Gamepad gamepad in Gamepad.all)
        {
            if (joinedGamepads.Contains(gamepad))
            {
                continue;
            }

            if (gamepad.startButton.wasPressedThisFrame)
            {
                AddGamepadToAvailableTeam(gamepad);
                joinedGamepads.Add(gamepad);
            }
        }

     }

     private void AddGamepadToAvailableTeam(Gamepad gamepad)
    {
        if (_teamAController.HasFreeHumanSlot())
        {
            _teamAController.AddPlayerToTeam("Gamepad", gamepad);
            return;
        }

        if (_teamBController.HasFreeHumanSlot())
        {
            _teamBController.AddPlayerToTeam("Gamepad", gamepad);
            return;
        }

        Debug.Log("Ya hay 4 jugadores humanos en total.");
    }
}
