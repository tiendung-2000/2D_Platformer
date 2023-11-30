//using UnityEngine;

//public class IdleState : IStatePlayer
//{
//    public void OnEnter(PlayerController player)
//    {
//        player.ChangeAnimationState(Constants.PLAYER_IDLE);
//    }

//    public void OnUpdate(PlayerController player)
//    {
//        player.Moving();
//        if (Input.GetButtonDown("Jump"))
//        {
//            player.ChangeState(new JumpState());
//            Debug.Log("Change state to jump");
//        }
//        else if (Input.GetMouseButtonDown(0))
//        {
//            player.ChangeState(new AttackState());
//            Debug.Log("Change state to attack");
//        }
//        else if (Input.GetKeyDown(KeyCode.LeftShift) && player.CanDash)
//        {
//            player.ChangeState(new DashingState());
//            Debug.Log("Change state to dash");
//        }
//        else if (player.IsMoving())
//        {
//            player.ChangeState(new WalkState());
//        }
//        else if (player.Rb.velocity.y < 0f && (!player.IsGrounded() || !player.IsPlatform()))
//        {
//            player.ChangeState(new FallingState());
//        }
//    }

//    public void OnExit(PlayerController player)
//    {

//    }
//}
