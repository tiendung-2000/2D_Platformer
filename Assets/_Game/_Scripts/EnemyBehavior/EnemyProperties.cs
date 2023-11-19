using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProperties : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] protected int curHealth;
    [SerializeField] protected int maxHealth;
    [SerializeField] protected int speed;

    [SerializeField] float radiusCheck;

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
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected LayerMask connerLayer;

    [SerializeField] protected EnemyType type;

    private void OnEnable()
    {
        OnInit();
    }

    void OnInit()
    {
        curHealth = maxHealth;
        isLive = true;
        player = GetComponent<PlayerController>();
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
        //transform.Translate(Vector2.right * speed * Time.deltaTime);
        //transform.position = Vector2.right * speed * Time.deltaTime;

        //RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, rayCastLength);
        //RaycastHit2D connerInfo = Physics2D.Raycast(groundDetection.position, Vector2.right, rayCastLength);

        //if (groundInfo.collider == false)
        //{
        //    Flip();
        //}

        isGrounded = Physics2D.OverlapCircle(groundDetection.position, radiusCheck, groundLayer);
        isTouchConner = Physics2D.OverlapCircle(groundDetection.position, radiusCheck, connerLayer);

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
        }
    }
    #endregion

    #region Flip
    //public virtual void CheckFlip()
    //{

    //}
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

    //private void OnCollisionEnter2D(Collision2D other)
    //{
    //    if (other.gameObject.layer == connerLayer)
    //    {
    //        isTouchConner = true;
    //    }
    //}

    //private void OnCollisionExit2D(Collision2D other)
    //{
    //    if (other.gameObject.layer == connerLayer)
    //    {
    //        isTouchConner = false;
    //    }
    //}
    #endregion
    #endregion

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

    public virtual void Dead()
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
            //Debug.DrawRay(groundDetection.position, Vector2.down, Color.red);
            //Debug.DrawRay(groundDetection.position, Vector2.right, Color.red);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundDetection.position, radiusCheck);
        }
    }
    #endregion

    public enum EnemyType
    {
        Patrol,
        Flying,
        Guarding,
    }
}