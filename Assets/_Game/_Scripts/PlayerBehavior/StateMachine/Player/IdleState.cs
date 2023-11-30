using UnityEngine;

public class IdleState : IStatePlayer
{
    public void OnEnter(PlayerController player)
    {
        player.ChangeAnimation(PlayerAnim.IDLE);
    }

    public void OnUpdate(PlayerController player)
    {
        player.Moving();
        if (Input.GetKeyDown(KeyCode.Space))
        {

        }
    }

    public void OnExit(PlayerController player)
    {

    }
}
