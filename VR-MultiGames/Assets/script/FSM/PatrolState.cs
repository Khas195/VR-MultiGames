using script.BoidBehavior;
using UnityEngine;

namespace script.FSM
{
	[CreateAssetMenu(menuName = "FSM/State/PatrolState")]
	public class PatrolState : State
	{
		public override void OnStateExit(StateController controller)
		{
			controller.GetComponent<PathFollowBehavior>().IsEnable = false;
		}

		public override void OnStateEnter(StateController controller)
		{
			controller.GetComponent<PathFollowBehavior>().IsEnable = true;
		}
	}
}
