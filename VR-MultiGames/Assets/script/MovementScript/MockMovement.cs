using UnityEngine;

namespace script.MovementScript
{
	public class MockMovement : MovementScript.Movement 
	{
		public bool IsGrounded { get; private set; }
		public bool IsCrouch { get; private set;  }
		public bool IsSprint { get; private set; }

		private Rigidbody _rigidbody;

		public MockMovement()
		{
			IsCrouch = IsGrounded = IsSprint = false;
		}

		public override void Move(Vector3 direction)
		{
			Debug.Log( string.Format("Mock object {0} direction is {1}", this, direction));
		}
	
		public override void Jump(float scale)
		{
			Debug.Log( string.Format("Mock object {0} is trying to jump {1}", this, scale));
		}
	}
}
