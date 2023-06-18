using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    const string ATTACK = "Attack";
    const string IDLE = "Idle";
    const string RUN = "Run";
    const string DIE = "Die";
    

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
            ChangeAnimationState(DIE);
            if (IsAnimationFinished(DIE))
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
                    ChangeAnimationState(IDLE);
                    return;
                }
                if (!isAttacking)
                {
                    isAttacking = true;
                    ChangeAnimationState(ATTACK);
                    Invoke(nameof(AttackComplete), enemy_animator.GetCurrentAnimatorStateInfo(0).length);
                }

            }
            if(move.IsMoving)
            {
                ChangeAnimationState(RUN);
            }
            else
            {
                ChangeAnimationState(IDLE);
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
