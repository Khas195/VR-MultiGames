using script.ControllerScript;
using UnityEngine;

namespace script.MovementScript
{
	public class SimpleMovement : Movement
	{
		[SerializeField] public float MaxSpeed = 5f;
		[SerializeField] public float Speed = 8f;
		[SerializeField] public float SprintMultiplier = 1.5f;
		[SerializeField] public float JumpForce = 30f;

		private Rigidbody _rigidbody;
		private CapsuleCollider _capsule;
		private Vector3 _groundNormal;
	
		private void Start()
		{
			GroundCheck();
			_rigidbody = Controller.GetComponent<Rigidbody>();
			_capsule = Controller.GetComponent<CapsuleCollider>();
		}

		public override void Move(Vector3 direction)
		{
			if (direction.magnitude > 1f)
			{
				direction.Normalize();
			}

			GroundCheck();

			if (!IsGrounded) return;

			var desiredDirection = Controller.transform.forward * direction.z + 
			                       Controller.transform.right * direction.x;
			
			_rigidbody.AddForce(desiredDirection * Speed * Time.deltaTime, ForceMode.Impulse);

			if(_rigidbody.velocity.magnitude > MaxSpeed)
				_rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, MaxSpeed);
		}

		public void Crouch()
		{
			Debug.Log(string.Format("{0} cannot crouch", this));
		}

		public override void Jump(float scale)
		{
			GroundCheck();
		
			if (IsGrounded)
			{
				_rigidbody.AddForce(0, scale * JumpForce * Time.deltaTime, 0, ForceMode.Impulse);
				IsGrounded = false;
			}
		}
	}
}
