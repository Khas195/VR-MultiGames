using UnityEngine;

namespace script.FSM
{
	[CreateAssetMenu(menuName = "FSM/State")]
	public class State : ScriptableObject
	{
		public Action[] ActionList;
		public Transition[] TransitionList;

		public void UpdateState(StateController controller)
		{
			DoActions(controller);
			CheckTransitions(controller);
		}

		public void DoActions(StateController controller)
		{
			foreach (var action in ActionList)
			{
				action.Act(controller);
			}
		}

		public void CheckTransitions(StateController controller)
		{
			foreach (var transition in TransitionList)
			{
				if (transition.decision.Decide(controller))
				{
					Debug.Log("True");
					controller.TransitionToState(transition.TrueState);
				}
				else
				{
					controller.TransitionToState(transition.FalseState);
				}
			}
		}
	}
}
