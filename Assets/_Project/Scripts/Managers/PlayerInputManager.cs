using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private TeamController _teamAController;
    [SerializeField] private TeamController _teamBController;

    private HashSet<Gamepad> joinedGamepads = new HashSet<Gamepad>();

    private bool wasdJoined = false;
    private bool arrowsJoined = false;



    
     void Update()
     {
         if(Keyboard.current == null) return;

        if (!wasdJoined && Keyboard.current.enterKey.wasPressedThisFrame)
        {
                _teamAController.AddPlayerToTeam("WASD", Keyboard.current);
                wasdJoined = true;
        }
        if(!arrowsJoined && Keyboard.current.rightCtrlKey.wasPressedThisFrame)
         {
             var player = PlayerInput.Instantiate(_playerPrefab,controlScheme: "Arrows", pairWithDevice: Keyboard.current);

             if(_spawnPoints.Length > 1)
             {
                 player.transform.position = _spawnPoints[1].position;
             }
             arrowsJoined = true;
         }
        
         foreach(var gamePad in Gamepad.all)
         {
             if (gamePad.startButton.wasPressedThisFrame)
             {
                 PlayerInput.Instantiate(_playerPrefab,controlScheme: "Gamepad",pairWithDevice : gamePad);
             }
         }
     }
}
