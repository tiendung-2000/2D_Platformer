using UnityEngine;

public class EnemyProperties : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] protected int curHealth;
    [SerializeField] protected int maxHealth;
    [SerializeField] protected int speed;

    [SerializeField] protected float radiusCheck;

    [SerializeField] protected float distanceToChase;
    [SerializeField] protected float distanceToAttack;

    [Header("Bool")]
    [SerializeField] protected bool isFacingRight;
    [SerializeField] protected bool isGrounded;
    [SerializeField] protected bool isMoving;
    [SerializeField] protected bool isAttack;
    [SerializeField] protected bool isLive;

    [SerializeField] protected bool isTouchConner;

    [Header("Animation State")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected string currentState;

    #region AnimationName
    //Waiting
    protected const string FLY = "Fly";
    protected const string IDLE = "Idle";
    //Moving
    protected const string WALK = "Walk";
    //End
    protected const string DIE = "Die";
    #endregion

    [SerializeField] protected PlayerController player;
    [SerializeField] protected Transform groundDetection;
    [SerializeField] protected Transform connerDetection;
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] protected EnemyType type;

    Vector2 direction = Vector2.right;

    private void OnEnable()
    {
        OnInit();
    }
    public virtual void OnInit()
    {
        curHealth = maxHealth;
        isLive = true;
        //player = GetComponent<PlayerController>();
        //player = LevelManager.Ins.player;
        rb = GetComponent<Rigidbody2D>();
        CheckEnemyType();
    }
    void CheckEnemyType()
    {
        switch (type)
        {
            case EnemyType.Patrol:
                ChangeAnimationState(WALK);
                isMoving = true;
                break;
            case EnemyType.Flying:
                ChangeAnimationState(FLY);
                isMoving = true;
                break;
            case EnemyType.Guarding:
                ChangeAnimationState(IDLE);
                isMoving = false;
                break;
        }
    }
    #region Moving Behavior Enemy Type
    #region Patrol Enemy
    public virtual void PatrolMovement()
    {
        CheckFlip();

        if (isMoving)
        {
            ChangeAnimationState(WALK);
            rb.velocity = transform.right * speed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }
    #endregion
    #region Fly Enemy
    public virtual void FlyMovement()
    {
        if (isMoving)
        {
            ChangeAnimationState(FLY);
        }
    }
    #endregion
    #region Guarding Enemy
    public virtual void GuardingMovement()
    {
        CheckFlip();

        if (isMoving)
        {
            Debug.Log("Moving");

            ChangeAnimationState(WALK);
            rb.velocity = transform.right * speed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }
    #endregion
    #region Flip

    public void FlipTowardsPlayer()
    {
        if (isAttack)
        {
            Vector3 directionToPlayer = player.gameObject.transform.position - transform.position;

            if (directionToPlayer.x > 0)
            {
                direction = Vector2.right;
                transform.eulerAngles = new Vector3(0, 0, 0);
                isFacingRight = true;
            }
            else
            {
                direction = Vector2.left;
                transform.eulerAngles = new Vector3(0, -180, 0);
                isFacingRight = false;
            }
        }
    }

    public void Flip()
    {
        if (isFacingRight == true)
        {
            direction = Vector2.left;
            transform.eulerAngles = new Vector3(0, -180, 0);
            isFacingRight = false;
        }
        else
        {
            direction = Vector2.right;
            transform.eulerAngles = new Vector3(0, 0, 0);
            isFacingRight = true;
        }
    }

    public void CheckFlip()
    {
        isGrounded = Physics2D.OverlapCircle(groundDetection.position, radiusCheck, groundLayer);
        isTouchConner = Physics2D.OverlapCircle(connerDetection.position, radiusCheck, groundLayer);
        if (!isGrounded || isTouchConner)
        {
            Flip();
        }
    }
    #endregion
    #endregion

    public bool IsCatchPlayer()
    {
        return Physics2D.Raycast(transform.position, direction, distanceToChase, playerLayer);
    }

    public bool IsCanAttack()
    {
        return Physics2D.Raycast(transform.position, direction, distanceToAttack, playerLayer);
    }
    public virtual void GiveDamage(int damage)
    {
        if (isLive && isAttack)
        {
            Debug.Log("Damage Player");
        }
    }
    public virtual void TakeDamage(int damage)
    {
        if (isLive)
        {
            curHealth -= damage;
            if (curHealth <= 0)
            {
                isLive = false;
                OnDespawn();
            }
        }
    }
    public virtual void Die()
    {
    }
    public virtual void OnDespawn()
    {
        Destroy(gameObject);
    }
    public void ChangeAnimationState(string newState)
    {
        //stop the same animation from interrupting itself 
        if (currentState == newState) return;
        //play the animation
        animator.Play(newState);
        //reassign the current state
        currentState = newState;
    }
    #region DebugGizmos
    public virtual void OnDrawGizmos()
    {
        if (groundDetection != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundDetection.position, radiusCheck);
            Gizmos.DrawWireSphere(connerDetection.position, radiusCheck);
        }
        Gizmos.DrawRay(transform.position, direction * distanceToChase);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, direction * distanceToAttack);
    }
    #endregion
    public enum EnemyType
    {
        Patrol,
        Flying,
        Guarding,
    }
}