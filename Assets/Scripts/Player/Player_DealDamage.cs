using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_DealDamage : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        switch(other.tag)
        {
            case "Enemy":
                Enemy_Health enemy_Health = other.gameObject.GetComponent<Enemy_Health>();
                enemy_Health.TakeDamage(.2f);
                break;
        }
    }
}
