using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float moveAcceleration = 0.1f;
    [SerializeField] private float movementDeacceleration = 1f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float jumpBufferTime = 0.2f;
    [SerializeField] private float isGroundedExtraHeight = 0.1f;
    [SerializeField] private float dashingPower = 10f;
    [SerializeField] private float DashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;
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

    // private Rigidbody2D rb2D;
    // private BoxCollider2D boxCollider2D;
    private bool isGrounded = false;
    private bool isFalling = false;

    private float moveHorizontal;
    private float _moveAccel;
    // private float currentHorizontalInput;
    // private float smoothInputVel;

    private bool canDash = true;
    private bool isDashing;

    private bool isFacingRight = false;

    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    // Update is called once per frame
    void Update()
    {
        isGrounded = groundCheck();
        if (isDashing) return;

        landCheck();
        wallCheck();

        animator.SetFloat("Speed", Mathf.Abs(moveHorizontal));

        if (moveHorizontal > 0 && isFacingRight) {
            Flip();
        } else if (moveHorizontal < 0 && !isFacingRight){
            Flip();
        }

        if (isGrounded) {
            coyoteTimeCounter = coyoteTime;
        } else {
            animator.SetBool("isFalling", true);
            isFalling = true;
            coyoteTimeCounter -= Time.deltaTime;
        }
        moveHorizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump")) {
            jumpBufferCounter = jumpBufferTime;
        } else {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (Input.GetMouseButtonDown(1) && canDash) {
            StartCoroutine(dash());
        }

    }

    void FixedUpdate()
    {
        if (isDashing) return;

        // currentHorizontalInput = Mathf.SmoothDamp(currentHorizontalInput, moveHorizontal, ref smoothInputVel, moveSmooth);
        if (moveHorizontal > 0.01f || moveHorizontal < -0.01f)
        {
            _moveAccel = Mathf.Lerp(_moveAccel, maxSpeed * moveHorizontal, moveAcceleration * Time.deltaTime);

            // if (Mathf.Abs(rb2D.velocity.x) <= maxSpeed)
            // {
                // rb2D.AddForce(new Vector2(moveHorizontal * moveSpeed, 0f), ForceMode2D.Force);
                // rb2D.velocity = new Vector2(moveHorizontal * moveSpeed, rb2D.velocity.y);
            // }
        } else {
            _moveAccel = Mathf.Lerp(_moveAccel, 0, movementDeacceleration * Time.deltaTime);
        }

        rb2D.velocity = new Vector2(_moveAccel, rb2D.velocity.y);
        // rb2D.velocity = new Vector2(currentHorizontalInput * maxSpeed, rb2D.velocity.y);

        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            // rb2D.AddForce(new Vector2(0f, jumpForce + Mathf.Abs(rb2D.velocity.y)), ForceMode2D.Impulse);
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
            coyoteTimeCounter = 0f;
            jumpBufferCounter = 0f;
        }
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

    private void wallCheck() {

    }

    private bool groundCheck()
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

        return raycastHit.collider != null;
    }

    private IEnumerator dash() {
        canDash = false;
        isDashing = true;

        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseWorldPoint2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

        float originalGravity = rb2D.gravityScale;
        rb2D.gravityScale = 0f;

        // int direction = isFacingRight ? -1 : 1;
        rb2D.velocity = (mouseWorldPoint2D - rb2D.position).normalized * dashingPower;

        trailRenderer.emitting = true;

        yield return new WaitForSeconds(DashingTime);

        trailRenderer.emitting = false;

        rb2D.velocity = Vector2.zero;
        _moveAccel = 0;
        rb2D.gravityScale = originalGravity;
        isDashing = false;

        yield return new WaitForSeconds(dashingCooldown);

        canDash = true;
    }

}
