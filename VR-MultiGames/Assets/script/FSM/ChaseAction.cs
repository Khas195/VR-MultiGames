using UnityEngine;

namespace script.FSM
{
	[CreateAssetMenu(menuName = "FSM/Action/Chase")]
	public class ChaseAction : Action
	{
		public override void Act(StateController controller)
		{
			Chase(controller);
		}

		private void Chase(StateController controller)
		{
			controller.NavAgent.destination = controller.ChaseTarget.position;
		}
	}
}
