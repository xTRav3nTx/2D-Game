using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerScript : MonoBehaviour
{
    [SerializeField]
    internal PlayerMovement playerMovement;
    [SerializeField]
    internal PlayerInput playerInput;
    [SerializeField]
    internal PlayerCollision playerCollision;


    // Update is called once per frame
    void Update()
    {

        if (playerMovement.isGrounded)
        {
            playerMovement.SlopeCheck();
        }
        playerMovement.Jump();
    }

    private void FixedUpdate()
    {
        playerMovement.Movement();
    }
}
