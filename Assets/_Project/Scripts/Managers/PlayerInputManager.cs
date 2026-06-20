using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform[] _spawnPoints;

    private bool wasdJoined = false;
    private bool arrowsJoined = false;



    // Update is called once per frame
    // void Update()
    // {
    //     if(Keyboard.current == null) return;

    //     if(!wasdJoined && Keyboard.current.enterKey.wasPressedThisFrame)
    //     {
    //         var player = PlayerInput.Instantiate(_playerPrefab,controlScheme: "WASD", pairWithDevice: Keyboard.current);

    //         if(_spawnPoints.Length > 0)
    //         {
    //             player.transform.position = _spawnPoints[0].position;
    //         }
    //         wasdJoined = true;
    //     }

    //     if(!arrowsJoined && Keyboard.current.rightCtrlKey.wasPressedThisFrame)
    //     {
    //         var player = PlayerInput.Instantiate(_playerPrefab,controlScheme: "Arrows", pairWithDevice: Keyboard.current);

    //         if(_spawnPoints.Length > 1)
    //         {
    //             player.transform.position = _spawnPoints[1].position;
    //         }
    //         arrowsJoined = true;
    //     }
        
    //     foreach(var gamePad in Gamepad.all)
    //     {
    //         if (gamePad.startButton.wasPressedThisFrame)
    //         {
    //             PlayerInput.Instantiate(_playerPrefab,controlScheme: "Gamepad",pairWithDevice : gamePad);
    //         }
    //     }
    // }
}
