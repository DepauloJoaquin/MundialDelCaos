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
    void Update()
    {
        if(Keyboard.current == null) return;
    }
}
