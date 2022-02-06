using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConveyorPlayerController : MonoBehaviour
{
    [SerializeField] private float groundCheckRadius, jumpForce;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private int jumpCount;
    private int currentJump;

    private bool isGrounded, isJumping, canJump;

    private Vector2 newVelocity, newForce, colliderSize;

    private Rigidbody2D rb;
    private CapsuleCollider2D col;
    private Vector3 halfSize;
    private bool hitObject;

    private TimerManager timerManager;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
        halfSize = new Vector3(0.0f, col.size.y / 2f);
        timerManager = FindObjectOfType<TimerManager>();
        
    }

    private void FixedUpdate() => CheckGround();

    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(col.bounds.center - (halfSize / 2f), groundCheckRadius, groundLayer);

        if (rb.velocity.y <= 0.0f)
        {
            isJumping = false;
        }

        if (isGrounded && !isJumping)
        {
            canJump = true;
            currentJump = 0;
        }

        if (hitObject)
        {
            timerManager.ReduceTime(5.0f);
            timerManager.PreventScore();
        }

        hitObject = false;
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        if (!canJump) return;

        currentJump++;
        canJump = currentJump < jumpCount;
        isJumping = true;
        newVelocity.Set(0.0f, 0.0f);
        rb.velocity = newVelocity;
        newForce.Set(0.0f, jumpForce);
        rb.AddForce(newForce, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & groundLayer) != 0) return;

        hitObject = true;
    }
}