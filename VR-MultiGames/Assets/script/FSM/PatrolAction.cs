using UnityEngine;

namespace script.FSM
{
	[CreateAssetMenu(menuName = "FSM/Action/Patrol")]
	public class PatrolAction : Action 
	{
		public override void Act(StateController controller)
		{
			Patrol(controller);
		}

		private void Patrol(StateController controller)
		{
			var agent = controller.NavAgent;
			agent.destination = controller.WaypointList[controller.NextWaypoint].position;

			if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
			{
				controller.NextWaypoint = (controller.NextWaypoint + 1) % controller.WaypointList.Count;
			}
		}
	}
}
