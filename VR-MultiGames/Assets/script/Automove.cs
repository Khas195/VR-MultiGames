using UnityEngine;

namespace script
{
	public class Automove : BoidBehavior.BoidBehavior 
	{
		public override void PerformBehavior()
		{
			if(!IsEnable || BoidController == null) return;

			SteeringForce = transform.forward * BoidController.Movement.MaxSpeed - BoidController.Velocity;
		}
	}
}
