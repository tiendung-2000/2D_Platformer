using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float horizontal;
    [SerializeField] float speed;
    [SerializeField] float jumpingPower;
    [SerializeField] bool isFacingRight = true;

    [Header("Properties")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [Header("Animation State")]
    [SerializeField] private Animator animator;
    [SerializeField] string currentState;

    [Header("Boolean Behaviour")]
    [SerializeField] bool isMoving;
    [SerializeField] bool isAttack;
    [SerializeField] bool isJumping;

    [Header("Dashing")]
    [SerializeField] bool canDash = true;
    [SerializeField] bool isDashing;
    [SerializeField] float dashingPower;
    [SerializeField] float dashingTime;
    [SerializeField] float dashingCooldown;

    //Animation State
    const string PLAYER_IDLE = "PlayerIdle";
    const string PLAYER_WALK = "PlayerWalk";
    const string PLAYER_JUMP = "PlayerJump";
    //const string PLAYER_FALL = "PlayerFalling";
    const string PLAYER_DASH = "PlayerDashing";
    const string PLAYER_JUMP_ATTACK = "PlayerJumpAttack";
    const string PLAYER_MELEE_ATTACK = "PlayerMeleeAttack";

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        Moving();
        Jumping();
        Flip();
        Dashing();
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    void Moving()
    {
        //Moving
        horizontal = Input.GetAxisRaw("Horizontal");

        if (horizontal == 0 && IsGrounded())
        {
            isMoving = false;
            ChangeAnimationState(PLAYER_IDLE);
        }
        else if (horizontal != 0 && IsGrounded())
        {
            isMoving = true;
            ChangeAnimationState(PLAYER_WALK);
        }
    }

    void Jumping()
    {
        //Jumping
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            isJumping = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        if (!IsGrounded() && isJumping == true)
        {
            ChangeAnimationState(PLAYER_JUMP);
        }
    }

    void Dashing()
    {
        if (isDashing)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);

        //trai
        //tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        //tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    void ChangeAnimationState(string newState)
    {
        //stop the same animation from interrupting itself 
        if (currentState == newState) return;

        //play the animation
        animator.Play(newState);

        //reassign the current state
        currentState = newState;
    }
}
