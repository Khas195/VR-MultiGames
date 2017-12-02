using UnityEngine;

namespace script.FSM
{
	[CreateAssetMenu(menuName = "FSM/Action/Aim")]
	public class AimAction : Action
	{
		public override void Act(StateController controller)
		{
			AimAtTarget(controller);
		}

		private void AimAtTarget(StateController controller)
		{
			var target = controller._boidController.Target;

			controller.transform.LookAt(target.transform);
		}
	}
}
