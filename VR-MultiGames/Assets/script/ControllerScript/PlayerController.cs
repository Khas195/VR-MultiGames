﻿using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using script.MovementScript;
using UnityEngine;

namespace script.ControllerScript
{
	public class PlayerController : Controller 
	{
		private void FixedUpdate()
		{
			Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, 
				Input.GetAxis("Vertical"));

			float jump = Input.GetAxis("Jump");
			
			Movement.Move(direction);
			Movement.Jump(jump);
		}
	}
}
