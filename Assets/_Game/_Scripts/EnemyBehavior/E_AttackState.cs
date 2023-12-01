using UnityEngine;

public class E_AttackState : IStateEnemy
{
    float timer;
    public void OnEnter(Enemy enemy)
    {
        if (enemy.Target != null)
        {
            enemy.ChangeDicrect(enemy.Target.transform.position.x < enemy.transform.position.x);
            enemy.StopMoving();
            enemy.Attack();
        }
        timer = 0;
    }

    public void OnUpdate(Enemy enemy)
    {
        timer += Time.deltaTime;
        if (timer >= 3.5f)
        {
            enemy.ChangeState(new E_PatrolState());
        }
    }

    public void OnExit(Enemy enemy)
    {
    }
}
