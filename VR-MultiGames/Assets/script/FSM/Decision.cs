using UnityEngine;

namespace script.FSM
{
	public abstract class Decision : ScriptableObject
	{
		public abstract bool Decide(StateController controller);
	}
}
