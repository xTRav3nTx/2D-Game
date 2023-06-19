using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Health : MonoBehaviour
{
    [SerializeField]
    private float health = 1f;
    public float Health => health;
    public bool IsAlive => health > 0.001f;

    public bool tookDamage = false;

    private EnemyMove enemyMove;

    private void Awake()
    {
        enemyMove = GetComponent<EnemyMove>();
    }

    public void TakeDamage(float dmgAmt)
    {
        health -= dmgAmt;
        tookDamage = true;
    }

    public void Update()
    {
        if (!IsAlive)
        {
            enemyMove.enabled = false;
        }
        if(tookDamage)
        {
            enemyMove.transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x + 1.5f, transform.position.y), Time.deltaTime * 2f);
            Invoke(nameof(TookDamageComplete), .5f);
        }
    }

    private void TookDamageComplete()
    {
        tookDamage = false;
    }
}
