using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private float rotationSpeed = 300;
    private bool rotating;

    private Rigidbody2D rb;

    void Awake()
    {
      rb = GetComponent<Rigidbody2D>();
    
    }

    void OnCollisionEnter2D(Collision2D collision)
     {
        rotating = true;
     }

    private void FixedUpdate()
    {
        if (rotating)
        {
           rb.angularVelocity = rotationSpeed;
        }
   
    }
}

   





