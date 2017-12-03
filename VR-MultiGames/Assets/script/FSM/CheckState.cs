using script.BoidBehavior;
using UnityEngine;

namespace script.FSM
{
	[CreateAssetMenu(menuName = "FSM/State/CheckState")]
	public class CheckState : State
	{
		public override void OnStateExit(StateController controller)
		{
		}

		public override void OnStateEnter(StateController controller)
		{
		}
	}
}
