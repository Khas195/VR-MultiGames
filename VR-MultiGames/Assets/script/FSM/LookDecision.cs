using UnityEngine;

namespace script.FSM
{
	[CreateAssetMenu(menuName = "FSM/Decision/Look")]
	public class LookDecision : Decision
	{
		private StateController _controller;
		
		public override bool Decide(StateController controller)
		{
			_controller = controller;
			return Look(controller);
		}

		private bool Look(StateController controller)
		{
			RaycastHit hit;
			if (Physics.Raycast(controller.transform.position,
				controller.transform.forward, out hit, controller.LookRange) &&
			    hit.collider.CompareTag("Player"))
			{
				controller.ChaseTarget = hit.transform;
				return true;
			}
			return false;
		}

		void OnDrawGizmos()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawLine(_controller.transform.position,
				_controller.transform.position + _controller.transform.forward * _controller.LookRange);
		}
	}
}
