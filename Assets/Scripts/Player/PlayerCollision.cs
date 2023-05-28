using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private Character_Controller player;
    public bool canClimb = false;

    private void Start()
    {
        player = GetComponent<Character_Controller>();
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Climb"))
        {
            canClimb = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Climb"))
        {
            canClimb = false;
            player.isClimbing = false;
        }
    }
}
