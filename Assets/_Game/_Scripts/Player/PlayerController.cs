using System.Collections;
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
    [SerializeField] bool isFalling;

    [Header("Dashing")]
    [SerializeField] bool canDash = true;
    [SerializeField] bool isDashing;
    [SerializeField] float dashingPower;
    [SerializeField] float dashingTime;
    [SerializeField] float dashingCooldown;
    //[SerializeField] ParticleSystem dashFX;

    //Animation State
    const string PLAYER_IDLE = "PlayerIdle";
    const string PLAYER_WALK = "PlayerWalk";
    const string PLAYER_JUMP = "PlayerJump";
    const string PLAYER_FALL = "PlayerFalling";
    //const string PLAYER_DASH = "PlayerDashing";
    const string PLAYER_JUMP_ATTACK = "PlayerJumpAttack";
    const string PLAYER_MELEE_ATTACK = "PlayerMeleeAttack";

    //AttackCombo
    const string PLAYER_ATTACK_1 = "PlayerAttack_1";
    const string PLAYER_ATTACK_2 = "PlayerAttack_2";
    const string PLAYER_ATTACK_3 = "PlayerAttack_3";

    [SerializeField] int attackCount;
    [SerializeField] bool doneAnim;
    public SpriteRenderer spriteRenderer;
    GhostController ghostController;


    public float timeAttack = 2f;
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        ghostController = GetComponent<GhostController>();

        ghostController.enabled = false;
    }

    private void Update()
    {
        Moving();
        Jumping();
        Flip();
        Dashing();
        Attack();
        HandleAnimation();
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    void Attack()
    {
        if (isAttack)
        {
            timeAttack -= Time.deltaTime;
            if (timeAttack < 0)
            {
                ResetCombo();
                isAttack = false;
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            isAttack = true;
            attackCount++;
            timeAttack = 2f;
            if (attackCount == 1)
            {
                ChangeAnimationState(PLAYER_ATTACK_1);
                doneAnim = true;
            }
            else if (attackCount == 2)
            {
                ChangeAnimationState(PLAYER_ATTACK_2);
                doneAnim = true;
            }
            else if (attackCount == 3)
            {
                ChangeAnimationState(PLAYER_ATTACK_3);
                doneAnim = true;
            }
        }
    }

    void ResetAttack()
    {
        if (!isAttack)
        {

        }
    }

    void ResetCombo()
    {
        if (isAttack)
        {
            attackCount = 0;
            ChangeAnimationState(PLAYER_IDLE);
        }
    }

    void CheckEndAnim()
    {


    }

    void Moving()
    {
        //Moving
        horizontal = Input.GetAxisRaw("Horizontal");
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
    }

    void HandleAnimation()
    {
        if (isAttack) return;
        //Fall Anim
        if (Mathf.Abs(rb.velocity.y) > 0.5f && rb.velocity.y < -1f && !IsGrounded())
        {
            if (isFalling == false)
            {
                ChangeAnimationState(PLAYER_FALL);
                isJumping = false;
                isFalling = true;
            }
        }
        else
        {
            isFalling = false;
        }

        //Jump Anim
        if (!IsGrounded() && isJumping == true)
        {
            ChangeAnimationState(PLAYER_JUMP);
        }

        //Move Anim
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

    void Dashing()
    {
        if (isDashing)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
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
        ghostController.enabled = true;
        yield return new WaitForSeconds(dashingTime);
        ghostController.enabled = false;

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
