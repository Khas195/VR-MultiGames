using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace script.FSM
{
	public class DroneUrgeState : StateMachineBehaviour
	{
		[Serializable]
		public enum DroneAction
		{
			Patrol,
			Wandering,
			CheckKey
		}
		
		[Serializable]
		public struct ActionProbality
		{
			public DroneAction Action;
			public float Probality;
		}

		[SerializeField] 
		private List<ActionProbality> _actionProbalityList = new List<ActionProbality>();

		private float _start = 0, _end = 0;
		private bool _hasUrge;
		
		public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
		{
			var urge = UrgeToDo(_actionProbalityList);

			switch (urge)
			{
				case DroneAction.Patrol:
				{
					animator.SetBool("IsPatrol", true);
					_hasUrge = true;
				}
					break;
				case DroneAction.Wandering:
				{
					animator.SetBool("IsWandering", true);
					_hasUrge = true;
				}
					break;
				case DroneAction.CheckKey:
				{
					animator.SetBool("IsCheckKey", true);
					_hasUrge = true;
				}
					break;
				default:
				{
					_hasUrge = false;
				}
					break;
			}
		}

		public DroneAction UrgeToDo(List<ActionProbality> actionProbalityList)
		{
			float prob = 0;
			
			foreach (var probality in actionProbalityList)
			{
				prob += probality.Probality;
			}
			
			Random.InitState((int) DateTime.Now.Ticks);

			var random = Random.Range(0, prob);

			prob = 0;
			foreach (var probality in actionProbalityList)
			{
				prob += probality.Probality;

				if (random <= prob)
				{
					return probality.Action;
				}
			}

			return DroneAction.Patrol;
		}

		public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
		{
			if (!_hasUrge)
			{
				var urge = UrgeToDo(_actionProbalityList);

				switch (urge)
				{
					case DroneAction.Patrol:
					{
						animator.SetBool("IsPatrol", true);
						_hasUrge = true;
					}
						break;
					case DroneAction.Wandering:
					{
						animator.SetBool("IsWandering", true);
						_hasUrge = true;
					}
						break;
					case DroneAction.CheckKey:
					{
						animator.SetBool("IsCheckKey", true);
						_hasUrge = true;
					}
						break;
					default:
					{
						_hasUrge = false;
					}
						break;
				}
			}
		}
	}
}
