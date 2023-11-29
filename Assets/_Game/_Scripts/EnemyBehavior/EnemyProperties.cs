using UnityEngine;

public class EnemyProperties : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] protected int curHealth;
    [SerializeField] protected int maxHealth;
    [SerializeField] protected int speed;
    [SerializeField] protected int damage;

    [SerializeField] float radiusCheck;
    [SerializeField] float distanceCheck;

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
    const string FLY = "Fly";
    const string IDLE = "Idle";
    //Moving
    const string WALK = "Walk";
    //End
    const string DIE = "Die";
    #endregion

    [SerializeField] protected PlayerController player;
    [SerializeField] protected Transform groundDetection;
    [SerializeField] protected Transform connerDetection;
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected LayerMask playerLayer;

    [SerializeField] protected EnemyType type;

    public virtual void OnInit()
    {
        curHealth = maxHealth;
        isLive = true;
        player = LevelManager.Ins.player;
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
        isGrounded = Physics2D.OverlapCircle(groundDetection.position, radiusCheck, groundLayer);
        isTouchConner = Physics2D.OverlapCircle(connerDetection.position, radiusCheck, groundLayer);

        if (!isGrounded || isTouchConner)
        {
            Flip();
        }

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
        if (isMoving)
        {
            ChangeAnimationState(WALK);
            rb.velocity = transform.forward * speed;
        }
    }
    #endregion

    #region Flip
    void Flip()
    {
        if (isFacingRight == true)
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
            isFacingRight = false;
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            isFacingRight = true;
        }
    }
    #endregion
    #endregion

    public bool IsCatchPlayer()
    {
        return Physics2D.OverlapCircle(transform.position, distanceCheck, playerLayer);
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

    void ChangeAnimationState(string newState)
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
        Gizmos.DrawWireSphere(transform.position, distanceCheck);

    }
    #endregion

    public enum EnemyType
    {
        Patrol,
        Flying,
        Guarding,
    }
}