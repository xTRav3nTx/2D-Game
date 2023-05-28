using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.TerrainTools;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField]
    MainPlayerScript playerScript;

    [SerializeField]
    private float speed = 5f;


    [SerializeField]
    private float jumpSpeed = 5f;

    [SerializeField]
    private float jumpTime = .35f;
    private float jumpTimeCounter = 0f;

    internal bool inWater = false;
    private float swimSpeed = .5f;

    private Rigidbody2D rb;
    private CapsuleCollider2D playerCollider;

    private Vector2 slopeNormalPerp;
    private float slopeDownAngle = 0f;
    private float slopeCheckDistance = 1f;
    private bool isOnSlope = false;
    private float slopeDownAngle_Old = 0f;
    private float slopeSideAngle = 0f;
    [SerializeField]
    private float maxSlopeAngle = 0f;
    private bool canWalkOnSlope = true;

    [SerializeField]
    private Transform groundCheckSphere;
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private LayerMask waterLayer;
    [SerializeField]
    internal bool isGrounded = false;
    [SerializeField]
    private float groundRadius = .1f;

    [SerializeField]
    private PhysicsMaterial2D fullFriction;
    [SerializeField]
    private PhysicsMaterial2D noFriction;

    internal bool canClimb = false;
    private bool isClimbing = false;

    
    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckSphere.position, groundRadius, groundLayer);
        inWater = Physics2D.OverlapCircle(groundCheckSphere.position, groundRadius, waterLayer);
    }

    internal void Jump()
    {
        if(playerScript.playerInput.jumPressed)
        {
            playerScript.playerInput.jumpPressedCounter++;
            jumpTimeCounter = jumpTime;
            if (playerScript.playerMovement.isGrounded || playerScript.playerMovement.inWater && !playerScript.playerInput.isJumping)
            {
                playerScript.playerInput.isJumping = true;
                rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            }
        }
        if (playerScript.playerInput.holdingJump && playerScript.playerInput.jumpPressedCounter == 1)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            }
            jumpTimeCounter -= Time.deltaTime;
        }
    }

    internal void Movement()
    {
        Climb();
        if ((!isClimbing || !canClimb) && !playerScript.playerInput.isJumping)
        {
            if (isGrounded && isOnSlope && canWalkOnSlope && !playerScript.playerInput.isJumping)
            {
                rb.velocity = new Vector2(-playerScript.playerInput.moveX * speed * slopeNormalPerp.x * Time.deltaTime, -playerScript.playerInput.moveX * speed * slopeNormalPerp.y * Time.deltaTime);
            }
            else if (!isGrounded && !inWater && !playerScript.playerInput.isJumping)
            {
                rb.velocity = new Vector2(playerScript.playerInput.moveX * speed * Time.deltaTime, rb.velocity.y);
            }
            else if (isGrounded && !playerScript.playerInput.isJumping)
            {
                rb.velocity = new Vector2(playerScript.playerInput.moveX * speed * Time.deltaTime, -5f);
            }
            else if (inWater && !playerScript.playerInput.isJumping)
            {
                rb.velocity = new Vector2(playerScript.playerInput.moveX * speed * swimSpeed * Time.deltaTime, rb.velocity.y);
            }
        }
        if (!isGrounded && playerScript.playerInput.isJumping)
        {
            rb.velocity = new Vector2(playerScript.playerInput.moveX * speed * Time.deltaTime, rb.velocity.y);
        }
    }

    private void Climb()
    {
        if (playerScript.playerInput.moveY != 0 && canClimb)
        {
            isClimbing = true;
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(playerScript.playerInput.moveX * speed * Time.fixedDeltaTime, playerScript.playerInput.moveY * speed * Time.fixedDeltaTime);
        }
        if (isClimbing && playerScript.playerInput.moveY == 0)
        {
            rb.velocity = new Vector2(playerScript.playerInput.moveX * speed * Time.fixedDeltaTime, 0f);
        }
        if (!canClimb)
        {
            isClimbing = false;
        }
    }

    internal void SlopeCheck()
    {
        Vector2 checkpos = transform.position - new Vector3(0f, playerCollider.size.y / 2);

        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkpos, transform.right, slopeCheckDistance, LayerMask.GetMask("Ground"));
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkpos, -transform.right, slopeCheckDistance, LayerMask.GetMask("Ground"));

        if(slopeHitFront)
        {
            isOnSlope = true;
            slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);
        }
        else if(slopeHitBack)
        {
            isOnSlope = true;
            slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
        }
        else
        {
            slopeSideAngle = 0f;
            isOnSlope = false;
        }

        RaycastHit2D hit = Physics2D.Raycast(checkpos, Vector2.down, slopeCheckDistance, groundLayer);
        
        if(hit)
        {
            slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;

            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if(slopeDownAngle != slopeDownAngle_Old)
            {
                isOnSlope = true;
            }
            

            slopeDownAngle_Old = slopeDownAngle;

            Debug.DrawRay(hit.point, slopeNormalPerp, Color.red);
            Debug.DrawRay(hit.point, hit.normal, Color.green);
        }

        if(slopeDownAngle > maxSlopeAngle || slopeSideAngle > maxSlopeAngle)
        {
            canWalkOnSlope = false;
        }
        else
        {
            canWalkOnSlope = true;
        }

        if (playerScript.playerInput.moveX == 0)
        {
            rb.sharedMaterial = fullFriction;
        }
        else if (isOnSlope && playerScript.playerInput.moveX == 0f && canWalkOnSlope)
        {
            rb.sharedMaterial = fullFriction;
        }
        else
        {
            rb.sharedMaterial = noFriction;
        }
    }

}
