using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    private float timeBetweenAttacks = .3f;
    private float timer = 0f;

    [SerializeField]
    private Character_Controller playerController;
    [SerializeField]
    private Animator playerAnim;

    internal bool isAttacking = false;

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0) && timer == 0)
        {
            isAttacking = true;
            timer = timeBetweenAttacks;
            playerAnim.Play("Attack");
        }

        StartTimer();

        if (IsAnimationFinished("Attack"))
        {
            isAttacking = false;
            timer = 0f;
        }

        if(!isAttacking)
        {
            if (playerController.Grounded && playerController.Input.X != 0)
            {
                playerAnim.Play("Run");
            }
            else if (!playerController.Grounded)
            {
                playerAnim.Play("Jump");
            }
            else
            {
                playerAnim.Play("Idle");
            }
        }
    }

    private bool IsAnimationFinished(string state)
    {
        AnimatorStateInfo stateInfo = playerAnim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName(state))
        {
            if (stateInfo.normalizedTime >= 1f)
            {
                return true;
            }
        }

        return false; 
    }

    private void StartTimer()
    {
        if(isAttacking)
        {
            timer -= Time.deltaTime;
        }
    }
}
