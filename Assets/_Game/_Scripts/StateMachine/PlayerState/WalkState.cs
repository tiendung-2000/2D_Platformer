//using UnityEngine;

//public class WalkState : IStatePlayer
//{
//    public void OnEnter(PlayerController player)
//    {
//        player.ChangeAnimationState(Constants.PLAYER_WALK);
//    }

//    public void OnUpdate(PlayerController player)
//    {
//        // =========== Viết function để state hoạt động
//        player.Moving();
//        player.Rb.velocity = new Vector2(player.Horizontal * player.Speed, 0);
//        // =========== Flip
//        if (player.IsFacingRight && player.Horizontal < 0f || !player.IsFacingRight && player.Horizontal > 0f)
//        {
//            player.IsFacingRight = !player.IsFacingRight;
//            Vector3 localScale = player.transform.localScale;
//            localScale.x *= -1f;
//            player.transform.localScale = localScale;
//        }

//        // =========== Viết function để state thay đổi sang state mới (phải có điều kiện để thay đổi)

//        // =========== Jumping
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
//        else if (!player.IsMoving())
//        {
//            player.ChangeState(new IdleState());
//            Debug.Log("Change state to idle");
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
