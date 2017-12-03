using UnityEngine;

namespace script.FSM
{
	[CreateAssetMenu(menuName = "FSM/Action/ShootDecision")]
	public class ShootDecision : Decision
	{
		public override bool Decide(StateController controller)
		{
			var shooter = controller.GetComponent<PaintLaserShooterDrone>();
			return shooter.CanFire();
		}
	}
}
