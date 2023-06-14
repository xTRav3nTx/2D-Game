using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{

    [SerializeField]
    private Animator enemyAnim;
    [SerializeField]
    private Rigidbody2D enemyRB;

    private EnemyMove enemyMove;

    private void Awake()
    {
        enemyMove = GetComponent<EnemyMove>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyRB.velocity.x < -.01f || enemyRB.velocity.x > .01f)
        {
            enemyAnim.Play("Run");
        }
        if(enemyMove.canAttack)
        {
            enemyAnim.Play("Attack");
        }
    }
}
