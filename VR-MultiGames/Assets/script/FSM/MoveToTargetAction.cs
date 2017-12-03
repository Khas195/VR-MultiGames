using script.BoidBehavior;
using UnityEngine;

namespace script.FSM
{
	[CreateAssetMenu(menuName = "FSM/Action/MoveToTargetAction")]
	public class MoveToTargetAction : Action
	{
		[SerializeField]
		private float _minDistance = 10;
		
		public override void Act(StateController controller)
		{
			MoveToTarget(controller);
		}

		private void MoveToTarget(StateController controller)
		{
			controller.GetComponent<ArrivalBehavior>().IsEnable = Vector3.Distance(controller.transform.position, controller._boidController.transform.position) > _minDistance;
		}
	}
}
