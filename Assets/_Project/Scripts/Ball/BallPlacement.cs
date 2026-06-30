using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPlacement : MonoBehaviour
{
    private PlayerController playerController;
    private void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    private void Update()
    {
        if(IsPlayerFlipped())
        {
            Vector3 position = transform.localPosition;
            transform.localPosition = new Vector3(-Mathf.Abs(position.x), position.y, position.z);
        }
        else
        {
            Vector3 position = transform.localPosition;
            transform.localPosition = new Vector3(Mathf.Abs(position.x), position.y, position.z);
        }
    }

    private bool IsPlayerFlipped() 
    {
        return playerController.spriteRenderer.flipX;
    }


}
