using UnityEngine;

public class CrystallisedHusk : EnemyProperties
{
    public float timer = 2f;
    public float duration = 2f;
    private enum Behavior { Idle, Move, Attack }
    private Behavior behavior;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GuardingMovement();

        CaculateBehavior();
    }

    void CaculateBehavior()
    {
        timer -= Time.deltaTime;

        switch (behavior)
        {
            case Behavior.Idle:
                if (timer <= 0f)
                {
                    Debug.Log("Move");
                    TransitionBehavior(Behavior.Move);
                }
                break;

            case Behavior.Move:
                if (IsCatchPlayer())
                {
                    Debug.Log("CatchPlayer");

                    TransitionBehavior(Behavior.Attack);
                }
                else if (timer <= 0f)
                {
                    Debug.Log("Idle");

                    TransitionBehavior(Behavior.Idle);
                }
                break;

            case Behavior.Attack:
                // Perform attack behavior here
                if (/*timer <= 0f*/ !IsCatchPlayer())
                {
                    Debug.Log("Idle");

                    TransitionBehavior(Behavior.Idle);
                }
                break;
        }
    }

    private void TransitionBehavior(Behavior nextBehavior)
    {
        behavior = nextBehavior;

        switch (behavior)
        {
            case Behavior.Idle:
                timer = duration;
                if (!IsCanAttack())
                {
                    isMoving = false;
                    ChangeAnimationState(IDLE);
                }
                break;

            case Behavior.Move:
                timer = duration;
                Flip();
                isMoving = true;
                break;

            case Behavior.Attack:
                timer = duration;
                isMoving = false;
                Attack();
                ChangeAnimationState("Attack");
                break;
        }
    }

    void Attack()
    {
        FlipTowardsPlayer();
        Debug.Log("Attack");
    }
}
