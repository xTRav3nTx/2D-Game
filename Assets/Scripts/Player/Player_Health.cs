using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Health : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float playerHealth = 1f;
    public float Health => playerHealth;

    public bool IsAlive => playerHealth > 0f;

    public void TakeDamage(float dmgAmt)
    {
        playerHealth -= dmgAmt;
    }
}
