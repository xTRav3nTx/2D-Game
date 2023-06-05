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


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        facingLeft = new Vector2(-transform.localScale.x, transform.localScale.y);
    }

    private void Update()
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(1.5f * changeDirection, -.5f), rayLength, layersToDetect);
        RaycastHit2D playerDetect = Physics2D.Raycast(transform.position, transform.right * changeDirection, playerDetectDistance, playerLayer);
        Debug.DrawRay(transform.position, new Vector2(1.5f * changeDirection, -.5f), Color.magenta);
        Debug.DrawRay(transform.position, new Vector2(10f * changeDirection, 0f), Color.magenta);
        if(hit.collider != null)
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
        if(playerDetect.collider != null)
        {
            string attackPlayer = playerDetect.collider.tag;
            if(attackPlayer.Equals("Player"))
            {
                Debug.Log("PlayerDetected");
            }
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(speed * changeDirection * Time.deltaTime, 0);
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
