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
		public List<Transform> WaypointList;
		public NavMeshAgent NavAgent;
		public Transform ChaseTarget;
		public int NextWaypoint;
		public float LookRange;
		public BoidController _controller;

		private void Awake()
		{
			NavAgent = GetComponent<NavMeshAgent>();
			_controller = GetComponent<BoidController>();
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
