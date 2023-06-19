using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    

    [SerializeField]
    private Animator enemy_animator;
    [SerializeField]
    private Rigidbody2D rb;

    private EnemyMove move;
    private Enemy_Health health;

    internal bool isAttacking = false;

    private string currentState;

    private void Awake()
    {
        move = GetComponent<EnemyMove>();
        health = GetComponent<Enemy_Health>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (!health.IsAlive)
        {
            ChangeAnimationState(StringConstants.DIE);
            if (IsAnimationFinished(StringConstants.DIE))
            {
                Destroy(this.gameObject);
                this.enabled = false;
            }
        }
        else
        {
            if (move.canAttack)
            {
                if (health.tookDamage)
                {
                    ChangeAnimationState(StringConstants.IDLE);
                    return;
                }
                if (!isAttacking)
                {
                    isAttacking = true;
                    ChangeAnimationState(StringConstants.ATTACK);
                    Invoke(nameof(AttackComplete), enemy_animator.GetCurrentAnimatorStateInfo(0).length);
                }
            }
            if(move.IsMoving)
            {
                ChangeAnimationState(StringConstants.RUN);
            }
            else
            {
                ChangeAnimationState(StringConstants.IDLE);
            }
        }

    }

    private bool IsAnimationFinished(string state)
    {
        AnimatorStateInfo stateInfo = enemy_animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName(state))
        {
            if (stateInfo.normalizedTime >= stateInfo.length)
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
