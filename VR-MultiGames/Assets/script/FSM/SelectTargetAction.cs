using UnityEngine;

namespace script.FSM
{
	[CreateAssetMenu(menuName = "FSM/Action/SelectTargetAction")]
	public class SelectTargetAction : Action
	{
		public override void Act(StateController controller)
		{
			CheckTarget(controller);
		}

		private void CheckTarget(StateController controller)
		{
			if (controller.CheckTargets == null || controller.CheckTargets.Length < 1) return;
			var nearestTarget = controller._boidController.Target ? 
				controller._boidController.Target :
				controller.CheckTargets[0];
			
			var sqrNearestDistance = (nearestTarget.transform.position - controller.transform.position).sqrMagnitude;
			
			foreach (var target in controller.CheckTargets)
			{
				var sqrDistance = (target.transform.position - controller.transform.position).sqrMagnitude;

				if (sqrDistance >= sqrNearestDistance) continue;
				
				nearestTarget = target;
				sqrNearestDistance = sqrDistance;
			}

			controller._boidController.Target = nearestTarget;
		}
	}
}
