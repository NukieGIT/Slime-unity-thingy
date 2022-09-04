using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float moveAcceleration = 0.1f;
    [SerializeField] private float moveDeceleration = 1f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float jumpBufferTime = 0.2f;
    [SerializeField] private float isGroundedExtraHeight = 0.1f;
    [Space]
    [Header("Obj Linking")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private BoxCollider2D boxCollider2D;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private LayerMask groundLayerMask;
    [Space]
    [Header("Events")]
    [SerializeField] private UnityEvent landedEvent;

    private bool isGrounded = false;
    private bool isFalling = false;

    private float moveHorizontal;


    public float MaxSpeed {
        get {return maxSpeed;}
        set {maxSpeed = value;}
    }

    public float MoveAcceleration
    {
        get { return moveAcceleration; }
        set { moveAcceleration = value; }
    }

    private bool isDashing;
    public bool IsDashing {
        set {
            isDashing = value;
        }
        get {
            return isDashing;
        }
    }

    private bool isFacingRight = false;

    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    public float CoyoteTimeCounter
    {
        get
        {
            return coyoteTimeCounter;
        }
        set
        {
            coyoteTimeCounter = value;
        }
    }

    // Update is called once per frame
    void Update()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        groundCheck();
        if (isDashing) return;

        handleInput();
        landCheck();

        animator.SetFloat("Speed", Mathf.Abs(moveHorizontal));
    }

    private void FixedUpdate()
    {
        if (isDashing) return;
        handleMovement();
    }

    private void handleInput() {
        handleCoyoteTime();
        handleJumpBuffer();
        handleSprite();
    }


    private void handleSprite() {
        if (moveHorizontal > 0 && isFacingRight) {
            Flip();
        } else if (moveHorizontal < 0 && !isFacingRight){
            Flip();
        }
    }

    private void handleCoyoteTime() {
        if (isGrounded) {
            coyoteTimeCounter = coyoteTime;
        } else {
            animator.SetBool("isFalling", true);
            isFalling = true;
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    private void handleJumpBuffer() {
        if (Input.GetButtonDown("Jump")) {
            jumpBufferCounter = jumpBufferTime;
        } else {
            jumpBufferCounter -= Time.deltaTime;
        }
    }

    private void handleMovement() {
        handleHorizontal();
        handleJump();
    }

    private void handleJump() {
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            rb2D.AddForce(Vector2.up * (jumpForce - rb2D.velocity.y), ForceMode2D.Impulse);
            coyoteTimeCounter = 0f;
            jumpBufferCounter = 0f;
        }
    }

    private void handleHorizontal() {
        float targetSpeed = moveHorizontal * maxSpeed;
        float speedDiff = targetSpeed - rb2D.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? moveAcceleration : moveDeceleration;
        float maxSpeedToAccelerationScale = maxSpeed == 0 ? 0 : 1 / maxSpeed;

        float movement = (Mathf.Abs(speedDiff) * accelRate * Mathf.Sign(speedDiff) * maxSpeedToAccelerationScale);

        rb2D.AddForce(movement * Vector2.right);
    }

    private void landCheck() {
        if (!isFalling) return;
        if (!isGrounded) return;

        isFalling = false;
        landedEvent.Invoke();
    }

    public void onLandedEvent() {
        animator.SetBool("isFalling", false);
        animator.SetTrigger("Landed");
    }

    private void Flip() {
        isFacingRight = !isFacingRight;
        spriteRenderer.flipX = isFacingRight;
    }

    private void groundCheck()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, isGroundedExtraHeight, groundLayerMask);
        Color raycolor;
        if (raycastHit.collider != null) {
            raycolor = Color.green;
        } else {
            raycolor = Color.red;
        }
        Debug.DrawRay(boxCollider2D.bounds.center + new Vector3(boxCollider2D.bounds.extents.x, 0), Vector2.down * (boxCollider2D.bounds.extents.y + isGroundedExtraHeight), raycolor);
        Debug.DrawRay(boxCollider2D.bounds.center - new Vector3(boxCollider2D.bounds.extents.x, 0), Vector2.down * (boxCollider2D.bounds.extents.y + isGroundedExtraHeight), raycolor);
        Debug.DrawRay(boxCollider2D.bounds.center - new Vector3(boxCollider2D.bounds.extents.x, boxCollider2D.bounds.extents.y + isGroundedExtraHeight), Vector2.right * boxCollider2D.bounds.extents.x * 2, raycolor);
        // Debug.Log(raycastHit.collider);

        isGrounded = raycastHit.collider != null;
    }

}
