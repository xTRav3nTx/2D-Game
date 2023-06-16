using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Health : MonoBehaviour
{
    [SerializeField]
    private float health = 1f;
    public float Health => health;
    public bool IsAlive => health > 0f;


    public void TakeDamage(float dmgAmt)
    {
        health -= dmgAmt;
    }
}
