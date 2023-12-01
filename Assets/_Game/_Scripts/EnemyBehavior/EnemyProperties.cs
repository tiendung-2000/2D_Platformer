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
    [SerializeField] protected bool isLive;
    [SerializeField] protected bool isFacingRight;
    [SerializeField] protected bool isTouchConner;
    [SerializeField] protected bool isGrounded;
    [SerializeField] protected bool isMoving;
    [SerializeField] protected bool isAttack;

    [Header("Animation State")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected string currentState;

    #region AnimationName
    protected const string IDLE = "Idle";
    protected const string MOVE = "Walk";
    protected const string ATTACK = "Attack";
    protected const string DIE = "Die";
    #endregion

    [SerializeField] protected PlayerController player;
    [SerializeField] protected Transform groundDetection;
    [SerializeField] protected Transform connerDetection;
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] protected EnemyType type;

    protected enum EnemyType { Patrol, Flying, Guarding, }
    protected enum State { Idle, Patrol, /*Chase,*/ Attack }
    Vector2 direction = Vector2.right;
    protected float timer = 2f;
    protected float duration = 2f;
    protected State state;

    public Vector3 distanceChase;
    protected virtual void Update()
    {
        Movement();
        CaculateBehavior();
        Debug.Log("Hehehhe");
    }

    private void OnEnable()
    {
        OnInit();
    }

    protected virtual void OnInit()
    {
        curHealth = maxHealth;
        isLive = true;
        //player = GetComponent<PlayerController>();
        //player = LevelManager.Ins.player;
        rb = GetComponent<Rigidbody2D>();
        CheckEnemyType();
        distanceChase = new Vector3(-(distanceToChase * 0.5f), 0f);
    }

    protected void CheckEnemyType()
    {
        switch (type)
        {
            case EnemyType.Patrol:
                ChangeAnimationState(MOVE);
                isMoving = true;
                break;
            case EnemyType.Flying:
                ChangeAnimationState(IDLE);
                isMoving = false;
                break;
            case EnemyType.Guarding:
                ChangeAnimationState(IDLE);
                isMoving = false;
                break;
        }
    }

    protected virtual void Movement()
    {
        CheckFlip();

        if (isMoving)
        {
            ChangeAnimationState(MOVE);
            rb.velocity = transform.right * speed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    protected void FlipTowardsPlayer()
    {
        Vector3 directionToPlayer = player.gameObject.transform.position - transform.position;

        if (directionToPlayer.x > 0)
        {
            direction = Vector2.right;
            distanceChase = new Vector3(-(distanceToChase * 0.5f), 0f);
            transform.eulerAngles = new Vector3(0, 0, 0);
            isFacingRight = true;
        }
        else
        {
            direction = Vector2.left;
            distanceChase = new Vector3((distanceToChase * 0.5f), 0f);
            transform.eulerAngles = new Vector3(0, -180, 0);
            isFacingRight = false;
        }
    }

    protected void Flip()
    {
        if (isFacingRight == true)
        {
            direction = Vector2.left;
            distanceChase = new Vector3((distanceToChase * 0.5f), 0f);
            transform.eulerAngles = new Vector3(0, -180, 0);
            isFacingRight = false;
        }
        else
        {
            direction = Vector2.right;
            distanceChase = new Vector3(-(distanceToChase * 0.5f), 0f);
            transform.eulerAngles = new Vector3(0, 0, 0);
            isFacingRight = true;
        }
    }

    protected void CheckFlip()
    {
        isGrounded = Physics2D.OverlapCircle(groundDetection.position, radiusCheck, groundLayer);
        isTouchConner = Physics2D.OverlapCircle(connerDetection.position, radiusCheck, groundLayer);
        if (!isGrounded || isTouchConner)
        {
            Flip();
        }
    }

    protected bool IsCatchPlayer()
    {
        if (type != EnemyType.Flying)
        {
            return Physics2D.Raycast(transform.position + distanceChase, direction, distanceToChase, playerLayer);
        }

        return Physics2D.OverlapCircle(transform.position, distanceToChase, playerLayer);
    }

    protected bool IsCanAttack()
    {
        return Physics2D.Raycast(transform.position, direction, distanceToAttack, playerLayer);
    }

    protected virtual void CaculateBehavior()
    {
        timer -= Time.deltaTime;

        switch (state)
        {
            case State.Idle:
                if (timer <= 0f)
                {
                    isMoving = true;
                    ChangeState(State.Patrol);
                    Debug.Log("Patroling");
                }
                else if (IsCatchPlayer() && !IsCanAttack())
                {
                    isMoving = true;
                    FlipTowardsPlayer();
                    ChangeState(State.Patrol);
                    Debug.Log("Chasing");
                }
                break;

            case State.Patrol:
                if (timer <= 0f)
                {
                    isMoving = false;
                    ChangeState(State.Idle);
                    Debug.Log("Idling");
                }
                else if (IsCatchPlayer() /*&& !IsCanAttack()*/)
                {
                    //isMoving = true;
                    FlipTowardsPlayer();
                    //ChangeState(State.Chase);
                    if (IsCanAttack())
                    {
                        isMoving = false;
                        Debug.Log("Can Attack");
                        ChangeState(State.Attack);
                    }
                    Debug.Log("Chasing");
                }
                break;

            //case State.Chase:
            //    if (IsCanAttack())
            //    {
            //        isMoving = false;
            //        Debug.Log("Can Attack");
            //        ChangeState(State.Attack);
            //    }
            //    else if (!IsCanAttack())
            //    {
            //        if (IsCatchPlayer() && !IsCanAttack())
            //        {
            //            isMoving = true;
            //            ChangeState(State.Chase);
            //        }
            //        else if (!IsCatchPlayer())
            //        {
            //            isMoving = false;
            //            ChangeState(State.Idle);
            //        }
            //    }
            //    break;

            case State.Attack:
                if (IsCanAttack())
                {
                    isMoving = false;
                    isAttack = true;
                    MeleeAttack();
                }
                else if (!IsCanAttack())
                {
                    isAttack = false;
                    isMoving = false;
                    ChangeState(State.Idle);
                    Debug.Log("Idling");
                }
                break;
        }
    }

    protected virtual void ChangeState(State nextState)
    {
        state = nextState;

        switch (state)
        {
            case State.Idle:
                timer = duration;
                ChangeAnimationState(IDLE);
                Debug.Log("IdleState");
                break;

            case State.Patrol:
                timer = duration;
                ChangeAnimationState(MOVE);
                Debug.Log("PatrolState");
                break;

            //case State.Chase:
            //    ChangeAnimationState(MOVE);
            //    Debug.Log("ChaseState");
            //    break;

            case State.Attack:
                ChangeAnimationState(ATTACK);
                Debug.Log("AttackState");
                break;
        }
    }

    protected virtual void MeleeAttack()
    {
        if (isAttack == true)
        {
            Debug.Log("Attacking");
        }
    }

    protected virtual void RangerAttack()
    {

    }

    protected virtual void GiveDamage(int damage)
    {
        if (isLive && isAttack)
        {
            Debug.Log("Damage Player");
        }
    }

    protected virtual void TakeDamage(int damage)
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

    protected virtual void Die()
    {
        ChangeAnimationState(DIE);
    }

    protected virtual void OnDespawn()
    {
        Destroy(gameObject);
    }

    protected void ChangeAnimationState(string newState)
    {
        //stop the same animation from interrupting itself 
        if (currentState == newState) return;
        //play the animation
        animator.Play(newState);
        //reassign the current state
        currentState = newState;
    }

    #region DebugGizmos
    protected virtual void OnDrawGizmos()
    {
        if (groundDetection != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundDetection.position, radiusCheck);
            Gizmos.DrawWireSphere(connerDetection.position, radiusCheck);
        }

        if (type != EnemyType.Flying)
        {
            Gizmos.DrawRay(transform.position + distanceChase, direction * distanceToChase);
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, direction * distanceToAttack);
        }
        else
        {
            Gizmos.DrawWireSphere(transform.position, distanceToChase);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, distanceToAttack);
        }
    }
    #endregion
}