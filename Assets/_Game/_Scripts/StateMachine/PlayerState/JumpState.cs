//using UnityEngine;

//public class JumpState : IStatePlayer
//{
//    public void OnEnter(PlayerController player)
//    {
//        player.ChangeAnimationState(Constants.PLAYER_JUMP);
//    }

//    public void OnUpdate(PlayerController player)
//    {
//        // ======= Function để nó hoạt động
//        if (player.IsJumping) return;
//        if (player.IsGrounded() || player.IsPlatform())
//        {
//            player.IsJumping = true;
//            player.Rb.velocity = new Vector2(player.Rb.velocity.x, player.JumpingPower);
//        }


//        // =========== Viết function để state thay đổi sang state mới (phải có điều kiện để thay đổi)
//        if (player.Rb.velocity.y < 0f)
//        {
//            player.ChangeState(new FallingState());
//        }
//    }

//    public void OnExit(PlayerController player)
//    {

//    }
//}
