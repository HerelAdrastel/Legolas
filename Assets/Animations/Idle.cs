using UnityEngine;

namespace Animations {
	public class Idle : StateMachineBehaviour {

		// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			animator.SetBool("Bounce", false);
		}
	}
}
