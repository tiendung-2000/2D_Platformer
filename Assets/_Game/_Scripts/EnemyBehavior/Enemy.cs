using UnityEngine;

public class Enemy : EnemyParent
{
    [SerializeField] private float attackRange;
    private IStateEnemy currentState;
    private PlayerController target;
    public PlayerController Target => target;

    private void Start()
    {
        OnInit();
    }
    private void Update()
    {
        if (currentState != null)
        {
            currentState.OnUpdate(this);
        }
    }
    public override void OnInit()
    {
        base.OnInit();
        ChangeState(new E_IdleState());
    }
    public override void OnDeSpawn()
    {
        base.OnDeSpawn();
    }
    public override void Hit(float damage)
    {
        base.Hit(damage);
    }
    protected override void OnDeath()
    {
        base.OnDeath();
        ChangeAnimation(DIE);
        Invoke(nameof(LoadDeath), 1f);

    }
    public void ChangeState(IStateEnemy state)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }

        currentState = state;

        if (currentState != null)
        {
            currentState.OnEnter(this);
        }
    }
    public void Moving()
    {
        ChangeAnimation(WALK);
        rb.velocity = transform.right * speed;

    }
    public void StopMoving()
    {
        ChangeAnimation(IDLE);
        rb.velocity = Vector2.zero;
    }
    public void Attack()
    {
        ChangeAnimation(ATTACK);
        Invoke(nameof(ResetAttack), 0.5f);
        ActiveAttack();
        Invoke(nameof(DeActiveAttack), 0.5f);
        return;
    }
    public bool IsTargetInRange()
    {
        if (target != null && Vector2.Distance(this.target.transform.position, transform.position) <= attackRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyWall"))
        {
            ChangeDicrect(!isRight);
            target = null;
        }
    }

    public void SetTarget(PlayerController character)
    {
        this.target = character;
        if (IsTargetInRange())
        {
            ChangeState(new E_AttackState());
        }
        else
        if (Target != null)
        {
            ChangeState(new E_PatrolState());
        }
        else
        {
            ChangeState(new E_IdleState());
        }
    }

    private void LoadDeath()
    {
        gameObject.SetActive(false);
    }
}
