using System;
using UnityEngine;

namespace script.FSM
{
	[Serializable]
	public class Transition
	{
		public Decision decision;
		public State TrueState;
		public State FalseState;
	}
}
