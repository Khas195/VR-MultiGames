using script.BoidBehavior;
using UnityEngine;

namespace script.FSM
{
	public class DroneWanderingState : StateMachineBehaviour
	{
		[SerializeField] 
		private float _wanderingTimer = 5;
		
		private WanderBehavior _wanderBehavior;
		private float _timer;

		public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
		{
			_wanderBehavior = animator.gameObject.GetComponent<WanderBehavior>();

			_wanderBehavior.IsEnable = true;
			
			_timer = _wanderingTimer;
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
		{
			_wanderBehavior.IsEnable = false;
		}

		public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
		{
			_timer -= Time.deltaTime;

			if (_timer <= 0)
			{
				animator.SetBool("IsWandering", false);
			}
		}
	}
}
