using script.MovementScript;
using UnityEngine;
using Random = UnityEngine.Random;

namespace script.BoidBehavior
{
	public class BoidMovement : Movement
	{
		[Header("Setting")]
		[SerializeField]
		private float _jumpForce = 5f;

		[SerializeField]
		private bool _isRotateToVelocity = true;

		[Tooltip("Scale the rotation speed to look at velocity direction")] 
		[SerializeField]
		private float _rotationSyncScale = 5;

		public float RotationSyncScale
		{
			get { return _rotationSyncScale; }
		}

		// Use this for initialization
		public override void Move(Vector3 direction)
		{
			Controller._Rigidbody.velocity += direction;

			if (_isRotateToVelocity && Controller._Rigidbody.velocity.sqrMagnitude > 1)
			{
				transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Controller._Rigidbody.velocity), 
					Time.deltaTime * _rotationSyncScale);
			}
		}

		public override void Jump(float scale)
		{
			if (Controller._Rigidbody.useGravity)
			{
				
			}
		}
	}
}
