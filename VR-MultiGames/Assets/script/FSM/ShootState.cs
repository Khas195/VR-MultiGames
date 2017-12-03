using script.BoidBehavior;
using UnityEngine;

namespace script.FSM
{
	[CreateAssetMenu(menuName = "FSM/State/ShootState")]
	public class ShootState : State
	{
		public override void OnStateExit(StateController controller)
		{
		}

		public override void OnStateEnter(StateController controller)
		{
		}
	}
}
