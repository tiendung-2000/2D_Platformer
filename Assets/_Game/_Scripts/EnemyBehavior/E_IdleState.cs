using UnityEngine;

public class E_IdleState : IStateEnemy
{
    float timer;
    float randomTime;

    public void OnEnter(Enemy enemy)
    {
        enemy.StopMoving();
        timer = 0;
        randomTime = Random.Range(2.5f, 4f);
    }

    public void OnUpdate(Enemy enemy)
    {
        timer += Time.deltaTime;
        if (timer > randomTime)
        {
            enemy.ChangeState(new E_PatrolState());
        }
    }

    public void OnExit(Enemy enemy)
    {
    }
}
