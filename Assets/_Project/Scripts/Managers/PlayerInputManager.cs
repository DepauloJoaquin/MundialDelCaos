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
    public GameObject _waitingRoom;

    private int nextPlayerImageIndex = 1;
    private const int maxPlayers = 4;

     void Update()
     {
        if (Keyboard.current != null)
        {
            if (!wasdJoined && Keyboard.current.enterKey.wasPressedThisFrame)
            {
                _teamAController.AddPlayerToTeam("WASD", Keyboard.current);
                wasdJoined = true;
                ActivateNextPlayerImage();
             
            }

            if (!arrowsJoined && Keyboard.current.numpad7Key.wasPressedThisFrame)
            {
                _teamAController.AddPlayerToTeam("Arrows", Keyboard.current);
                arrowsJoined = true;
                ActivateNextPlayerImage();
               
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
                bool added = AddGamepadToAvailableTeam(gamepad);
                 if (added)
                {
                    joinedGamepads.Add(gamepad);
                    ActivateNextPlayerImage();
                }
            }
        }

     }

       private bool AddGamepadToAvailableTeam(Gamepad gamepad)
    {
        if (_teamAController.HasFreeHumanSlot())
        {
            _teamAController.AddPlayerToTeam("Gamepad", gamepad);
            return true;
        }

        if (_teamBController.HasFreeHumanSlot())
        {
            _teamBController.AddPlayerToTeam("Gamepad", gamepad);
            return true;
        }

        Debug.Log("Ya hay 4 jugadores humanos en total.");
        return false;
    }

    void ActivatePlayer(GameObject playerImage)
    {
        playerImage.SetActive(true);    
    }
    void DesactivatePlayer(GameObject playerImage)
    {
        playerImage.SetActive(false);    
    }

    private GameObject ImageOfPlayer(int numberOfplayer)
    {
        return _waitingRoom.transform.Find($"Jugador {numberOfplayer}").gameObject;
    }

      private void ActivateNextPlayerImage()
    {
        if (nextPlayerImageIndex > maxPlayers)
        {
            return;
        }

        ActivatePlayer(ImageOfPlayer(nextPlayerImageIndex));
        nextPlayerImageIndex++;
    }
    public void DesactivateAllPlayers()
    {
    for (int i = 1; i <= 4; i++)
    {
        GameObject playerImage = ImageOfPlayer(i);

        if (playerImage != null)
        {
            DesactivatePlayer(playerImage);
        }
    }

    nextPlayerImageIndex = 1;
    }
}
