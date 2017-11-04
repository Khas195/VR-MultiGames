using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using script.MovementScript;

namespace script.ControllerScript
{
	public class Controller : MonoBehaviour
	{
		[SerializeField] protected List<Movement> MovementList;

		[SerializeField]
		public bool AllowMultipleMovements;

		protected Controller(bool allowMultipleMovements=false)
		{
			AllowMultipleMovements = allowMultipleMovements;
		}
		
		private void Awake()
		{ 
			// Get current movement list
			if (MovementList != null)
			{
				MovementList = MovementList.Concat(GetComponents<Movement>().ToList()).ToList();
			}
			else
			{
				MovementList = GetComponents<Movement>().ToList();
			}

			foreach (var movement in MovementList)
			{
				movement.SetController(this);
			}
		}

		private void OnEnable()
		{
			//update current MovementList;
			MovementList = GetComponents<Movement>().ToList();
		}
	}
}
