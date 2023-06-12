using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Player_Health playerhealth = other.gameObject.GetComponent<Player_Health>();
            playerhealth.TakeDamage(.15f);
        }
    }


}
