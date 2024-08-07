using UnityEngine;

public class RestartRunning : StateMachineBehaviour
{
	static int s_DeadHash = Animator.StringToHash("Dead");

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    //    Debug.Log("tyt? OnStateExit i animator yaris");
        // We don't restart if we go toward the death state
        if (animator.GetBool(s_DeadHash))
            return; 
        
        TrackManager.instance.StartMove(false);
    }

}
