using UnityEngine;

namespace script.FSM
{
	public abstract class Action : ScriptableObject
	{
		public abstract void Act(StateController controller);
	}
}
