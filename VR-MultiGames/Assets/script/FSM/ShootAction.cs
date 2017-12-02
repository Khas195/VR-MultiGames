using UnityEngine;

namespace script.FSM
{
	[CreateAssetMenu(menuName = "FSM/Action/Shoot")]
	public class ShootAction : Action
	{
		public override void Act(StateController controller)
		{
			ShootAtTarget(controller);
		}

		private void ShootAtTarget(StateController controller)
		{
			var shooter = controller.GetComponent<PaintLaserShooterDrone>();

			if (shooter.CanFire())
			{
				shooter.Fire();
			}
		}
	}
}
