using UnityEngine;

namespace script.FSM
{
	[CreateAssetMenu(menuName = "FSM/Decision/CheckDecision")]
	public class CheckDecision : Decision
	{
		[SerializeField]
		private float _checkProbality = 0.5f;

		public override bool Decide(StateController controller)
		{
			return ShouldMoveCloser(controller);
		}

		private bool ShouldMoveCloser(StateController controller)
		{
			Random.InitState((int) controller._boidController.Velocity.sqrMagnitude);
			return Random.Range(0, 1) <= _checkProbality;
		}
	}
}
