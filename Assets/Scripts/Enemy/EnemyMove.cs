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
    public bool canAttack = false;
    private EnemyAnimation anim;

    [SerializeField]
    public bool IsMoving = false;

    [SerializeField]
    private Canvas canvas;

    private Player_Health playerHealth;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        facingLeft = new Vector2(-transform.localScale.x, transform.localScale.y);
        anim = GetComponent<EnemyAnimation>();
        playerHealth = FindAnyObjectByType<Player_Health>();
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
            case StringConstants.WALL:
            case StringConstants.WATER:
                return false;
            default:
                return true;
        }
    }

    //fix attack animation and movement
    private void EnemyMovement()
    {
        aggroCircle = Physics2D.OverlapCircle(transform.position, playerDetectDistance, playerLayer);
        if(aggroCircle != null && playerHealth.IsAlive)
        {
            isChasing = true;
            if (CanAttack())
            {
                IsMoving = false;
                rb.velocity = new Vector2(0f, 0f);
            }
            else
            {
                if (!IsPlayerReachable())
                {
                    IsMoving = false;
                    rb.velocity = new Vector2(0f, 0f);
                }
                else if(!anim.isAttacking)
                {

                    IsMoving = true;
                    rb.velocity = new Vector2((direction = playerDirection.normalized.x < 0 ? -1f : 1f) * Time.fixedDeltaTime * chaseSpeed, 0f);
                }
            }
        }
        else
        {
            IsMoving = true;
            canAttack = false;   
            isChasing = false;
            rb.velocity = new Vector2(speed * changeDirection * Time.fixedDeltaTime, 0f);
        }

    }

    private bool CanAttack()
    {
        if (Math.Abs(playerDirection.x) < attackDistance)
        {
            canAttack = true;
        }
        else
        {
            canAttack = false;
        }
        return canAttack;
    }

    private void Flip()
    {
        bodySprite.sortingOrder = 0;
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
        canvas.transform.localScale = new Vector2(-canvas.transform.localScale.x, canvas.transform.localScale.y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerDetectDistance);
    }
}
