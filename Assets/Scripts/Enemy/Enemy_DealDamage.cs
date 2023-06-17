using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DealDamage : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Player":
                Player_Health player_Health = other.GetComponent<Player_Health>();
                player_Health.TakeDamage(.15f);
                break;
        }
    }
}
