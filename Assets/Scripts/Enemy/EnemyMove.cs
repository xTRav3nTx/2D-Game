using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField]
    Transform[] patrolPoints;
    Rigidbody2D rb;
    [SerializeField]
    private float speed = 10f;
    private float distance = 0f;
    private int changeDirection = -1;
    private int currentPoint = 0;
    Vector2 facingLeft;
    private bool isFacingLeft = false;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        facingLeft = new Vector2(-transform.localScale.x, transform.localScale.y);
    }

    private void Update()
    {
        Flip();
        GetDistanceToPoint(patrolPoints[currentPoint]);
        ChangePoint();
    }
    void FixedUpdate()
    {
        rb.velocity = new Vector2(speed * changeDirection * Time.deltaTime, 0);
    }

    private void GetDistanceToPoint(Transform point)
    {
        distance = Mathf.Abs(transform.position.x - point.position.x);
    }

    private void ChangePoint()
    {
        if(distance < .5f && rb.velocity.x < 0)
        {
            changeDirection = 1;
            currentPoint = 1;
        }
        if (distance < .5f && rb.velocity.x > 0)
        {
            changeDirection = -1;
            currentPoint = 0;
        }
    }

    private void Flip()
    {
        if (currentPoint == 1 && isFacingLeft)
        {
            isFacingLeft = false;
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
        else if (currentPoint == 0 && !isFacingLeft)
        {
            isFacingLeft = true;
            transform.localScale = facingLeft;
        }
    }

}
