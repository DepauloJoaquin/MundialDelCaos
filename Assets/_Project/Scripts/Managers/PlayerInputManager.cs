using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private TeamController _teamController;

    private HashSet<Gamepad> joinedGamepads = new HashSet<Gamepad>();

    private bool wasdJoined = false;
    private bool arrowsJoined = false;



    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current != null)
    {
        if (!wasdJoined && Keyboard.current.enterKey.wasPressedThisFrame)
        {
            _teamController.AddPlayerToTeam("WASD", Keyboard.current);
            wasdJoined = true;
        }

        if (!arrowsJoined && Keyboard.current.rightCtrlKey.wasPressedThisFrame)
        {
            _teamController.AddPlayerToTeam("Arrows", Keyboard.current);
            arrowsJoined = true;
        }
    }

    foreach (var gamePad in Gamepad.all)
    {
        if (!joinedGamepads.Contains(gamePad) && gamePad.startButton.wasPressedThisFrame)
        {
            Debug.Log("Se unió gamepad: " + gamePad.displayName);

            _teamController.AddPlayerToTeam("Gamepad", gamePad);
            joinedGamepads.Add(gamePad);
        }
    }
    }
}
