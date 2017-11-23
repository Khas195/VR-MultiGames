using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using script.MovementScript;

namespace script.ControllerScript
{
	public class Controller : MonoBehaviour
	{
		[SerializeField] 
		private Movement _movement;
		
		[SerializeField] 
		protected bool IsDrawGizmos;

		public Rigidbody Rigidbody { get; private set; }

		public Vector3 Velocity
		{
			get { return Rigidbody.velocity; }
		}

		public Movement Movement
		{
			get { return _movement; }
		}

		private void Awake()
		{
			// Get current movement list
			if (!_movement)
			{
				_movement = GetComponent<Movement>();
			}
			
			Rigidbody = GetComponent<Rigidbody>();
		}
	}
}
