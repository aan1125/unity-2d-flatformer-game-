using UnityEngine;

public class JumpStateBehaviour : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        exam08_player player = animator.GetComponentInParent<exam08_player>();
        if (player != null)
        {
            player.OnJumpAnimationEnd();
        }
    }
}
