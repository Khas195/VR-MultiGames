using UnityEngine;

namespace script.FSM
{
	[CreateAssetMenu(menuName = "FSM/State")]
	public abstract class State : ScriptableObject
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
					controller.TransitionToState(transition.TrueState);
				}
				else
				{
					controller.TransitionToState(transition.FalseState);
				}
			}
		}

		public abstract void OnStateExit(StateController controller);
		public abstract void OnStateEnter(StateController controller);
	}
}
