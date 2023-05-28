using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    [SerializeField]
    MainPlayerScript playerScript;

    internal float moveX = 0f;
    internal float moveY = 0f;

    internal bool isJumping = false;
    internal bool jumPressed = false;
    internal int jumpPressedCounter = 0;
    internal bool holdingJump = false;

    private bool isFacingLeft = false;
    private Vector2 facingLeft;



    private void Awake()
    {
        facingLeft = new Vector2(-transform.localScale.x, transform.localScale.y);
    }

    // Update is called once per frame
    void Update()
    {

        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");

        Flip();

        jumPressed = Input.GetButtonDown("Jump");
        holdingJump = Input.GetButton("Jump");
    }

    private void Flip()
    {
        if (moveX > 0 && isFacingLeft)
        {
            isFacingLeft = false;
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
        else if (moveX < 0 && !isFacingLeft)
        {
            isFacingLeft = true;
            transform.localScale = facingLeft;
        }
    }


}
