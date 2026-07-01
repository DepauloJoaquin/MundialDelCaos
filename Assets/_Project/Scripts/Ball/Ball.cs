using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private float rotationSpeed = 300;
    private bool rotating;

    public GameObject _currentOwner;
    public GameObject _lastOwner;
    public GameObject _lastKickPlayer;
    public GameObject _passTarget;
    public bool _isFree;


    private Rigidbody2D rb;

    void Awake()
    {
      rb = GetComponent<Rigidbody2D>();
    
    }

    void OnCollisionEnter2D(Collision2D collision)
     {
         PlayerController receiver = collision.gameObject.GetComponent<PlayerController>();
         if (receiver == null)
        {
            return;
        }
        rotating = true;
        PlayerController previousOwnerController = null;
        if(_currentOwner != null)
        {   
            previousOwnerController = _currentOwner.GetComponent<PlayerController>();
        }
        _lastOwner = _currentOwner;
        _currentOwner = collision.gameObject;
        receiver.OnPlayerReceivesBall(previousOwnerController);
     }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ScoreForJ1"))
        {
            GameManager.Instance.RegisterTeam_A_Goal();
        }
        else if (collision.CompareTag("ScoreForJ2"))
        {
            GameManager.Instance.RegisterTeam_B_Goal();
        }
    }

    private void FixedUpdate()
    {
        if (rotating)
        {
           rb.angularVelocity = rotationSpeed;
        }
   
    }
}

   





