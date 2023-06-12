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
    private RaycastHit2D playerDetectRight;
    private RaycastHit2D playerDetectLeft;


    [SerializeField]
    private Transform playerPosition;
    private bool isChasing = false;
    private Vector2 playerDirection;
    [SerializeField]
    private float chaseSpeed = 12f;
    [SerializeField]
    private float attackDistance = 1f;

    private Vector2 previousVelocity;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        facingLeft = new Vector2(-transform.localScale.x, transform.localScale.y);
        previousVelocity = rb.velocity;
    }

    private void Update()
    { 

        playerDirection = playerPosition.position - transform.position;
        hit = Physics2D.Raycast(transform.position, new Vector2(1.5f * changeDirection, -.5f), rayLength, layersToDetect);
        playerDetectRight = Physics2D.Raycast(transform.position, transform.right, playerDetectDistance, playerLayer);
        playerDetectLeft = Physics2D.Raycast(transform.position, -transform.right, playerDetectDistance, playerLayer);
        Debug.DrawRay(transform.position, new Vector2(1.5f * changeDirection, -.5f), Color.magenta);
        Debug.DrawRay(transform.position, new Vector2(10f, 0f), Color.magenta);
        Debug.DrawRay(transform.position, new Vector2(-10f, 0f), Color.magenta);


        if (hit.collider != null)
        {
            string turnAroundCollider = hit.collider.tag;
            switch(turnAroundCollider)
            {
                case "Wall":
                case "Water":
                    Flip();
                    break;
            }
        }
        else
        {
            Flip();
        }

        if(playerDetectRight.collider != null)
        {
            string attackPlayer = playerDetectRight.collider.tag;
            if(attackPlayer.Equals("Player"))
            {
                Debug.Log("Player Detected");
                isChasing = true;
            }
        }
        else if(playerDetectLeft.collider != null)
        {
            string attackPlayer = playerDetectLeft.collider.tag;
            if (attackPlayer.Equals("Player"))
            {
                Debug.Log("Player Detected");
                isChasing = true;
            }
        }
    }

    void FixedUpdate()
    {
        ChasePlayer();
    }

    private void ChasePlayer()
    {
        if(Mathf.Abs(transform.position.x - playerPosition.position.x) < attackDistance)
        {
            rb.velocity = Vector2.zero;
        }
        else
        {
            if (isChasing)
            {
                rb.velocity = new Vector2(playerDirection.normalized.x * Time.deltaTime * chaseSpeed, 0f);
                if (Vector2.Dot(rb.velocity.normalized, previousVelocity.normalized) < 0f)
                {
                    Flip();
                }
                previousVelocity = rb.velocity;
            }
            else
            {
                rb.velocity = new Vector2(speed * changeDirection * Time.deltaTime, 0f);
            }
        }
    }

    private void Flip()
    {
        if (isFacingLeft)
        {
            bodySprite.sortingOrder = -1;
            isFacingLeft = false;
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            changeDirection = -1;
        }
        else if (!isFacingLeft)
        {
            bodySprite.sortingOrder = -1;
            isFacingLeft = true;
            transform.localScale = facingLeft;
            changeDirection = 1;
        }
    }
}
