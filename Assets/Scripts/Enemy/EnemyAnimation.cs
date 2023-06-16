using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    const string ATTACK = "Attack";
    const string IDLE = "Idle";
    const string RUN = "Run";
    

    [SerializeField]
    private Animator enemy_animator;
    [SerializeField]
    private Rigidbody2D rb;

    private EnemyMove move;

    internal bool isAttacking = false;

    private string currentState;

    private void Awake()
    {
        move = GetComponent<EnemyMove>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {

        if (rb.velocity.x < -.01f || rb.velocity.x > .01f && !isAttacking)
        {
            ChangeAnimationState(RUN);
        }
        if (rb.velocity.x == 0 && !isAttacking)
        {
            ChangeAnimationState(IDLE);
        }
        if (move.canAttack)
        {
            if(!isAttacking)
            {
                isAttacking = true;
                ChangeAnimationState(ATTACK);
                Invoke("AttackComplete", enemy_animator.GetCurrentAnimatorStateInfo(0).length);
            }
            
        }

    }

    private bool IsAnimationFinished(string state)
    {
        AnimatorStateInfo stateInfo = enemy_animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName(state))
        {
            if (stateInfo.normalizedTime >= 1f)
            {
                return true;
            }
        }

        return false;
    }

    private void AttackComplete()
    {
        isAttacking = false;
    }

    private void ChangeAnimationState(string newState)
    {
        if(currentState == newState)
        {
            return;
        }
        enemy_animator.Play(newState);
        currentState = newState;
    }


}
