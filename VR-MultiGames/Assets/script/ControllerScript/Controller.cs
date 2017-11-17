using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using script.MovementScript;

namespace script.ControllerScript
{
	public class Controller : MonoBehaviour
	{
		[SerializeField] private Movement _movement;
		[SerializeField] protected bool IsDrawGizmos = true;

		public Rigidbody _Rigidbody { get; private set; }

		public Movement Movement
		{
			get { return _movement; }
		}

		private void Awake()
		{
			// Get current movement list
			_movement = GetComponent<Movement>();
			_Rigidbody = GetComponent<Rigidbody>();
		}
	}
}
