using UnityEngine;

public class EnemyParent : MonoBehaviour
{
    [SerializeField] protected Animator anim;
    [SerializeField] protected float speed = 1;
    [SerializeField] protected SpriteRenderer sprite;
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected float hp;
    //[SerializeField] protected HealthBar healthBar;
    //[SerializeField] protected CombatText combatText;
    [SerializeField] protected GameObject AttackZone;
    public bool isDeath => hp <= 0;
    protected bool isRight = true;
    string animName = "idle";

    #region AnimationName
    protected const string IDLE = "Idle";
    protected const string WALK = "Walk";
    protected const string ATTACK = "Attack";
    protected const string DIE = "Die";
    #endregion

    protected void ChangeAnimation(string animName)
    {
        if (this.animName != animName)
        {
            anim.ResetTrigger(this.animName);
            this.animName = animName;
            anim.SetTrigger(animName);
        }
    }
    public void ChangeDicrect(bool isRight)
    {
        this.isRight = isRight;
        if (isRight)
        {
            transform.rotation = Quaternion.Euler(Vector2.up * 180);
            //sprite.flipX = true;
        }
        else
        {
            transform.rotation = Quaternion.identity;
            //sprite.flipX = false;
        }
    }
    public virtual void OnInit()
    {
        hp = 100;
        //healthBar.OnInit(hp);
        AttackZone.SetActive(false);
    }
    public virtual void OnDeSpawn()
    {

    }
    public virtual void Hit(float damage)
    {
        if (hp >= 0)
        {
            hp -= damage;
            //healthBar.CurrHealth = hp;
            //Instantiate(combatText, transform.position + Vector3.up, Quaternion.identity).OnInit(damage);
            if (isDeath)
            {
                OnDeath();
            }

        }
    }
    protected virtual void ResetAttack()
    {
        ChangeAnimation(IDLE);

    }
    protected virtual void ActiveAttack()
    {
        AttackZone.SetActive(true);
    }
    protected virtual void DeActiveAttack()
    {
        AttackZone.SetActive(false);
    }
    protected virtual void OnDeath()
    {

    }
}
