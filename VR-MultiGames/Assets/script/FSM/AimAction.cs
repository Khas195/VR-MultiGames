using UnityEngine;

namespace script.FSM
{
	[CreateAssetMenu(menuName = "FSM/Action/Aim")]
	public class AimAction : Action
	{
		public override void Act(StateController controller)
		{
			throw new System.NotImplementedException();
		}

		private void AimAtTarget(StateController controller)
		{
			var target = controller._controller.Target;

			controller.transform.LookAt(target.transform);
		}
	}
}
