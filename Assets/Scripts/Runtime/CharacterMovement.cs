using System;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed,
        groundCheckRadius,
        jumpForce,
        slopeCheckDistance,
        slopeAngleThreshold;

    [SerializeField] private PhysicsMaterial2D fullFrictionSlope, noFrictionSlope;
    [SerializeField] private LayerMask groundLayer;

    private float horizontalInput, slopeDownAngle, slopeSideAngle, lastSlopeAngle;
    private bool isGrounded, onSlope, isJumping, canWalkOnSlope, canJump;

    private Vector2 newVelocity, newForce, colliderSize, slopePerpendicular;

    private Rigidbody2D rb;
    private BoxCollider2D col;

    private Vector3 halfSize;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        
        colliderSize = col.size;

        halfSize = new Vector3(0.0f, colliderSize.y / 2f, 0.0f);
    }

    private void Update()
    {
        ProcessInput();
    }

    private void FixedUpdate()
    {
        GroundCheck();
        SlopeCheck();
        ProcessMovement();
    }

    private void ProcessInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void Jump()
    {
        if (!canJump) return;
        
        canJump = false;
        isJumping = true;
        newVelocity.Set(0.0f, 0.0f);
        rb.velocity = newVelocity;
        newForce.Set(0.0f, jumpForce);
        rb.AddForce(newForce, ForceMode2D.Impulse);
    }

    private void GroundCheck()
    {
        isGrounded = Physics2D.OverlapCircle(transform.position - halfSize, groundCheckRadius, groundLayer);
        Debug.Log(isGrounded);
        
        if (rb.velocity.y <= 0.0f)
        {
            isJumping = false;
        }

        if (isGrounded && !isJumping && slopeDownAngle <= slopeAngleThreshold)
        {
            canJump = true;
        }
    }

    private void SlopeCheck()
    {
        Vector2 checkPos = transform.position - halfSize;
        SlopeCheckHorizontal(checkPos);
        SlopeCheckVertical(checkPos);
    }

    private void SlopeCheckHorizontal(Vector2 checkPos)
    {
        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transform.right, slopeCheckDistance, groundLayer);
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -transform.right, slopeCheckDistance, groundLayer);

        if (slopeHitFront)
        {
            onSlope = true;
            slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);
        }
        else if(slopeHitBack)
        {
            onSlope = true;
            slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
        }
        else
        {
            slopeSideAngle = 0.0f;
            onSlope = false;
        }
    }

    private void SlopeCheckVertical(Vector2 checkPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, groundLayer);

        if (hit)
        {
            slopePerpendicular = Vector2.Perpendicular(hit.normal).normalized;
            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (slopeDownAngle != lastSlopeAngle)
            {
                onSlope = true;
            }

            lastSlopeAngle = slopeDownAngle;
            
            Debug.DrawRay(hit.point, slopePerpendicular, Color.blue);
            Debug.DrawRay(hit.point, hit.normal, Color.gray);
        }

        if (slopeDownAngle > slopeAngleThreshold || slopeSideAngle > slopeAngleThreshold)
        {
            canWalkOnSlope = false;
        }
        else
        {
            canWalkOnSlope = true;
        }

        if (onSlope && canWalkOnSlope && horizontalInput == 0.0f)
        {
            rb.sharedMaterial = fullFrictionSlope;
        }
        else
        {
            rb.sharedMaterial = noFrictionSlope;
        }
    }

    private void ProcessMovement()
    {
        switch (isGrounded)
        {
            case true when !onSlope && !isJumping:
                newVelocity.Set(movementSpeed * horizontalInput, 0.0f);
                rb.velocity = newVelocity;
                Debug.Log("Simple move");
                break;
            case true when onSlope && canWalkOnSlope && !isJumping:
                newVelocity.Set(movementSpeed * slopePerpendicular.x * -horizontalInput,
                    movementSpeed * slopePerpendicular.y * -horizontalInput);
                rb.velocity = newVelocity;
                break;
            case false:
                newVelocity.Set(movementSpeed * horizontalInput, rb.velocity.y);
                rb.velocity = newVelocity;
                break;
        }
    }
}