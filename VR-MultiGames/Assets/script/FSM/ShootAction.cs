using UnityEngine;

namespace script.FSM
{
	[CreateAssetMenu(menuName = "FSM/Action/Shoot")]
	public class ShootAction : Action
	{
		public override void Act(StateController controller)
		{
			throw new System.NotImplementedException();
		}

		private void ShootAtTarget(StateController controller)
		{
			var target = controller._controller.Target;

			controller.transform.LookAt(target.transform);
		}
	}
}
