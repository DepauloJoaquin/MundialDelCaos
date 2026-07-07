using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPassZone : MonoBehaviour
{
    public static List<PlayerController> playersTeamA = new List<PlayerController>();
    public static List<PlayerController> playersTeamB = new List<PlayerController>();
    private Collider2D _collider;
    private Ball _ball;

    public float offsetX = 0f;
    public static List<PlayerController> GetPlayersInZone(Team team)
    {
        if (team == Team.A)
        {
            return playersTeamA;
        }
        else if (team == Team.B)
        {
            return playersTeamB;
        }
        return new List<PlayerController>();
    }
    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        _ball = transform.parent.GetComponent<Ball>();
    }

    private void Update()
    {
	    transform.rotation = Quaternion.identity;
        MovePosition();
    }

    private void MovePosition() 
    {
        transform.position = new Vector2(CurrentXCoordenate() , _ball.transform.position.y);
    }

    private float CurrentXCoordenate()
    {
        if(BallRotationSpeed() > 0)
        {
            return _ball.transform.position.x + offsetX;
        }
        else 
        {
            return _ball.transform.position.x - offsetX;
        }   
    }

    private float BallRotationSpeed()
    {
        return _ball.rotationSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player == null) { return; }
        
        if (player._team == Team.A)
        {
            playersTeamA.Add(player);
        }
        else if (player._team == Team.B)
        {
            playersTeamB.Add(player);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player == null) { return; }
        
        if (player._team == Team.A)
        {
            playersTeamA.Remove(player);
        }
        else if (player._team == Team.B)
        {
            playersTeamB.Remove(player);
        }
    }

    public static PlayerController GetNearestPlayerPosition(Team team, PlayerController playerOrigin)
    {
        List<PlayerController> playersInZone = GetPlayersInZone(team);
        if (playersInZone.Count == 0)
        {
            return null;
        }

        PlayerController nearestPlayer = null;
        Vector2 playerOriginPosition = playerOrigin.transform.position;
        float nearestDistance = Mathf.Infinity;

        foreach (PlayerController playerInList in playersInZone)
        {
            Vector2 playerInListPosition = playerInList.transform.position;
            if (playerInListPosition == playerOriginPosition) { continue; } // Skip the player at the given position
            float distance = Vector2.Distance(playerInListPosition, playerOriginPosition);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestPlayer = playerInList;
            }
        }

        return nearestPlayer;
    }
}
