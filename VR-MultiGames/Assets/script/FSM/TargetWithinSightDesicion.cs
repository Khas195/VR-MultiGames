using System;
using UnityEngine;

namespace script.FSM
{
	[CreateAssetMenu(menuName = "FSM/Decision/TargetWithinWightDecision")]
	[Serializable]
	public class TargetWithinWightDecision
		: Decision
	{
		[SerializeField]
		private float _minDistance = 5;
		
		public override bool Decide(StateController controller)
		{
			return CheckTargetWithinSight(controller);
		}

		private bool CheckTargetWithinSight(StateController controller)
		{
			var target = controller._boidController.Target;

			return (target.transform.position - controller.transform.position).magnitude <= _minDistance;
		}
	}
}
