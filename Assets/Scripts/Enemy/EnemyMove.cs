using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D rb;
    [SerializeField]
    private float speed = 10f;
    internal int changeDirection = -1;
    Vector2 facingLeft;
    private bool isFacingLeft = false;

    [SerializeField]
    private SpriteRenderer bodySprite;

    [SerializeField]
    private float rayLength = 5f;
    [SerializeField]
    private float playerDetectDistance = 10f;
    [SerializeField]
    private LayerMask layersToDetect;
    [SerializeField]
    private LayerMask playerLayer;
    private RaycastHit2D hit;


    [SerializeField]
    private Transform playerPosition;
    private Vector2 playerDirection;
    [SerializeField]
    private float chaseSpeed = 12f;
    [SerializeField]
    private float attackDistance = 1f;
    private bool isChasing = false;
    Collider2D aggroCircle;
    float direction = 0f;
    internal bool canAttack = false;
    private EnemyAnimation anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        facingLeft = new Vector2(-transform.localScale.x, transform.localScale.y);
        anim = GetComponent<EnemyAnimation>();
    }

    private void Update()
    {
        ThingsToCalculate();

        if(!isChasing)
        {
            if (hit.collider != null)
            {
                string turnAroundCollider = hit.collider.tag;
                switch (turnAroundCollider)
                {
                    case "Wall":
                    case "Water":
                        Flip();
                        break;
                }
            }
        }
        
        if (isChasing && Vector2.Dot(playerDirection.normalized, transform.right * changeDirection) < 0f && !anim.isAttacking)
        {
            Flip();
        }
        
    }

    private void FixedUpdate()
    {
        EnemyMovement();
    }

    private void ThingsToCalculate()
    {
        playerDirection = playerPosition.position - transform.position;
        hit = Physics2D.Raycast(transform.position, new Vector2(1.5f * changeDirection, -.5f), rayLength, layersToDetect);
        Debug.DrawRay(transform.position, new Vector2(1.5f * changeDirection, -.5f), Color.magenta);
    }

    private bool IsPlayerReachable()
    {
        switch(hit.collider?.tag)
        {
            case "Wall":
            case "Water":
                return false;
            default:
                return true;
        }
    }

    private void EnemyMovement()
    {
        aggroCircle = Physics2D.OverlapCircle(transform.position, playerDetectDistance, playerLayer);
        if(aggroCircle != null)
        {
            isChasing = true;
            if (Math.Abs(playerDirection.x) < attackDistance)
            {
                canAttack = true;
                rb.velocity = new Vector2(0f, 0f);
            }
            else if(!anim.isAttacking)
            {
                canAttack = false;
                rb.velocity = new Vector2((direction = playerDirection.normalized.x < 0 ? -1f : 1f) * Time.deltaTime * chaseSpeed, 0f);
            }

            if (!IsPlayerReachable())
            {
                rb.velocity = new Vector2(0f, 0f);
            }
        }
        else
        {
            canAttack = false;   
            isChasing = false;
            rb.velocity = new Vector2(speed * changeDirection * Time.deltaTime, 0f);
        }

    }

    private void Flip()
    {
        bodySprite.sortingOrder = -1;
        if (isFacingLeft)
        {
            isFacingLeft = false;
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            changeDirection = -1;
        }
        else if (!isFacingLeft)
        {
            isFacingLeft = true;
            transform.localScale = facingLeft;
            changeDirection = 1;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerDetectDistance);
    }
}
