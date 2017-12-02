using System.Collections.Generic;
using JetBrains.Annotations;
using script.BoidBehavior;
using script.ControllerScript;
using UnityEngine;
using UnityEngine.AI;

namespace script.FSM
{
	public class StateController : Controller
	{
		public State CurState;
		public BoidController _boidController;

		private void Awake()
		{
			_boidController = GetComponent<BoidController>();
		}

		private void FixedUpdate()
		{
			CurState.UpdateState(this);
		}

		public void TransitionToState(State nextState)
		{
			if (nextState != null && CurState != nextState)
			{
				CurState = nextState;
			}
		}
		
	}
}
