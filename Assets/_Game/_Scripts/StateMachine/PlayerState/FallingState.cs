//public class FallingState : IStatePlayer
//{
//    public void OnEnter(PlayerController player)
//    {
//        player.ChangeAnimationState(Constants.PLAYER_FALL);
//    }

//    public void OnUpdate(PlayerController player)
//    {
//        if (player.IsGrounded() || player.IsPlatform())
//        {
//            player.IsJumping = false;
//            player.Moving();
//            if (player.Rb.velocity.y >= 0)
//            {
//                if (player.IsMoving())
//                {
//                    player.ChangeState(new WalkState());
//                }
//                else if (!player.IsMoving())
//                {
//                    player.ChangeState(new IdleState());
//                }
//            }

//        }
//        else
//        {
//            player.Rb.velocity.y
//        }


//    }

//    public void OnExit(PlayerController player)
//    {

//    }
//}
