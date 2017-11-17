using script.MovementScript;
using UnityEngine;
using Random = UnityEngine.Random;

namespace script.BoidBehavior
{
	public class BoidMovement : Movement
	{
		[SerializeField] private float _jumpForce = 5f;

		// Use this for initialization
		public override void Move(Vector3 direction)
		{
			Controller._Rigidbody.velocity += direction;
		}

		public override void Jump(float scale)
		{
			if (Controller._Rigidbody.useGravity)
			{
				
			}
		}
	}
}
