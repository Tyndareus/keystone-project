using System;
using Unity.Mathematics;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed,
        groundCheckRadius,
        jumpForce,
        slopeCheckDistance,
        maxSlopeAngle;

    [SerializeField] private PhysicsMaterial2D fullFrictionSlope, noFrictionSlope;
    [SerializeField] private LayerMask groundLayer;

    private float horizontalInput, slopeDownAngle, slopeSideAngle, lastSlopeAngle;
    private bool isGrounded, onSlope, isJumping, canWalkOnSlope, canJump;

    private Vector2 newVelocity, newForce, colliderSize, slopePerpendicular;

    private Rigidbody2D rb;
    private CapsuleCollider2D col;
    
    private Vector3 halfSize;

    private int spriteDirection = 1;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();

        halfSize = new Vector2(0.0f, col.size.y / 2f);
    }

    private void Update()
    {
        CheckInput();     
    }

    private void FixedUpdate()
    {
        CheckGround();
        SlopeCheck();
        ApplyMovement();
    }

    private void CheckInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        switch (horizontalInput)
        {
            case 1 when spriteDirection == -1:
                spriteDirection *= -1;
                transform.Rotate(0.0f, 180.0f, 0.0f);
                break;
            case -1 when spriteDirection == 1:
                spriteDirection *= -1;
                transform.Rotate(0.0f, 180.0f, 0.0f);
                break;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }
    
    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(col.bounds.center - (halfSize / 2f), groundCheckRadius, groundLayer);

        if(rb.velocity.y <= 0.0f)
        {
            isJumping = false;
        }

        if(isGrounded && !isJumping && slopeDownAngle <= maxSlopeAngle)
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
        else if (slopeHitBack)
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

            if(slopeDownAngle != lastSlopeAngle)
            {
                onSlope = true;
            }                       

            lastSlopeAngle = slopeDownAngle;
            
            Debug.DrawRay(hit.point, hit.normal, Color.green);
            Debug.DrawRay(hit.point, slopePerpendicular, Color.red);
        }

        if (slopeDownAngle > maxSlopeAngle || slopeSideAngle > maxSlopeAngle)
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

    private void ApplyMovement()
    {
        if (isGrounded && !onSlope && !isJumping) //if not on slope
        {
            newVelocity.Set(movementSpeed * horizontalInput, 0.0f);
            rb.velocity = newVelocity;
        }
        else if (isGrounded && onSlope && canWalkOnSlope && !isJumping) //If on slope
        {
            newVelocity.Set(movementSpeed * slopePerpendicular.x * -horizontalInput, movementSpeed * slopePerpendicular.y * -horizontalInput);
            rb.velocity = newVelocity;
        }
        else if (!isGrounded) //If in air
        {
            newVelocity.Set(movementSpeed * horizontalInput, rb.velocity.y);
            rb.velocity = newVelocity;
        }
    }

    private void OnDrawGizmos()
    {
        CapsuleCollider2D bb = GetComponent<CapsuleCollider2D>();

        Vector3 size = new Vector2(0.0f, (bb.size.y / 2f) / 2f);
        Gizmos.DrawWireSphere(bb.bounds.center - size, groundCheckRadius);
    }
}