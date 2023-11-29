using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;
    public int damage;

    [Header("Movement")]
    [SerializeField] float horizontal;
    [SerializeField] float speed;
    [SerializeField] float jumpingPower;
    [SerializeField] bool isFacingRight = true;

    [Header("Properties")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask platformLayer;

    [Header("Animation State")]
    [SerializeField] private Animator animator;
    [SerializeField] string currentState;

    [Header("Boolean Behaviour")]
    [SerializeField] bool isMoving;
    [SerializeField] bool isAttack;
    [SerializeField] bool isJumping;
    [SerializeField] bool isFalling;
    //public bool isGround;
    //public bool isPlatform;

    [Header("Dashing")]
    [SerializeField] bool canDash = true;
    [SerializeField] bool isDashing;
    [SerializeField] float dashingPower;
    [SerializeField] float dashingTime;
    [SerializeField] float dashingCooldown;

    //Animation State
    const string PLAYER_IDLE = "Idle";
    const string PLAYER_WALK = "Walk";
    const string PLAYER_JUMP = "Jump";
    const string PLAYER_FALL = "Falling";
    const string PLAYER_DASH = "Dashing";

    //AttackCombo
    const string PLAYER_ATTACK_1 = "Attack_1";
    const string PLAYER_ATTACK_2 = "Attack_2";
    const string PLAYER_ATTACK_3 = "Attack_3";

    [Header("Attack")]
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
        Attack();
        if (isAttack)
        {
            horizontal = 0;
            return;
        }
        Moving();
        Jumping();
        Flip();
        Dashing();
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

    #region Attack Combo
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
            if (IsGrounded() || IsPlatform())
            {
                isAttack = true;
                attackCount++;
                timeAttack = 2f;
                if (attackCount == 1)
                {
                    ChangeAnimationState(PLAYER_ATTACK_1);
                }

                if (attackCount > 1) doneAnim = true;
            }
        }
    }
    void ResetCombo()
    {
        if (isAttack)
        {
            attackCount = 0;
            isAttack = false;
            ChangeAnimationState(PLAYER_IDLE);
        }
    }
    void CheckEndAnim2()
    {
        if (attackCount > 1 && doneAnim)
        {
            doneAnim = false;
            attackCount++;
            ChangeAnimationState(PLAYER_ATTACK_2);
        }
        else
        {
            ResetCombo();
        }
    }
    void CheckEndAnim3()
    {
        if (attackCount > 2 && doneAnim)
        {
            doneAnim = false;
            attackCount++;
            ChangeAnimationState(PLAYER_ATTACK_3);
        }
        else
        {
            ResetCombo();
        }
    }
    #endregion
    void Moving()
    {
        //Moving
        horizontal = Input.GetAxisRaw("Horizontal");
    }

    void Jumping()
    {
        //Jumping
        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded() || IsPlatform())
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            }
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    void HandleAnimation()
    {
        if (Mathf.Abs(rb.velocity.y) > 0.05f && rb.velocity.y > 0)
        {
            ChangeAnimationState(PLAYER_JUMP);

        }
        else if (Mathf.Abs(rb.velocity.y) > 0.05f && rb.velocity.y < -2f)
        {
            ChangeAnimationState(PLAYER_FALL);
        }
        else
        {
            if (horizontal == 0)
            {
                if ((IsGrounded() || IsPlatform()) && !isDashing)
                {
                    ChangeAnimationState(PLAYER_IDLE);
                }
                else if ((IsGrounded() || IsPlatform()) && isDashing)
                {
                    ChangeAnimationState(PLAYER_DASH);
                }
            }
            else if (horizontal != 0)
            {
                if (IsGrounded() || IsPlatform())
                {
                    ChangeAnimationState(PLAYER_WALK);
                }
            }
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
        isDashing = true;
        ChangeAnimationState(PLAYER_DASH);
        canDash = false;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        ghostController.enabled = true;

        yield return new WaitForSeconds(dashingTime);

        ChangeAnimationState(PLAYER_IDLE);
        ghostController.enabled = false;
        rb.gravityScale = originalGravity;
        isDashing = false;

        yield return new WaitForSeconds(dashingCooldown);

        canDash = true;
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

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.5f, groundLayer);
    }

    private bool IsPlatform()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.5f, platformLayer);
    }

    void ChangeAnimationState(string newState)
    {
        //stop the same animation from interrupting itself 
        if (currentState == newState) return;

        //play the animation

        if (currentState != string.Empty)
        {
            animator.SetBool(currentState, false);
        }

        animator.SetBool(newState, true);

        //reassign the current state
        currentState = newState;
    }

    public void TakeDamage()
    {

    }

    public void KnockBack()
    {

    }

    public void GiveDamage()
    {

    }
}
