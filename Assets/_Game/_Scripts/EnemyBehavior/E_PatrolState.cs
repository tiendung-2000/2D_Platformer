using UnityEngine;

public class E_PatrolState : IStateEnemy
{
    float timer;
    float randomTime;

    public void OnEnter(Enemy enemy)
    {
        timer = 0;
        randomTime = Random.Range(3f, 6f);
    }

    public void OnUpdate(Enemy enemy)
    {
        timer += Time.deltaTime;
        if (enemy.Target != null)
        {
            enemy.ChangeDicrect(enemy.Target.transform.position.x < enemy.transform.position.x);
            if (enemy.IsTargetInRange())
            {
                enemy.ChangeState(new E_AttackState());
            }
            else
            {
                enemy.Moving();
            }

            //float distance = Vector2.Distance(enemy.Target.transform.position, enemy.transform.position);
            //if (distance <= 2)
            //{
            //    enemy.StopMoving();
            //}
        }
        else
        {
            if (timer < randomTime)
            {
                enemy.Moving();
            }
            else
            {
                enemy.ChangeState(new E_IdleState());
            }
        }
    }

    public void OnExit(Enemy enemy)
    {

    }
}
