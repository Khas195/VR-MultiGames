using script.BoidBehavior;
using UnityEngine;

namespace script.FSM
{
	public class DronePatrolState : StateMachineBehaviour
	{
		[SerializeField] 
		private float _patrolTimer = 5;
		
		private PathFollowBehavior _pathFollowBehavior;
		private WanderBehavior _wanderBehavior;
		
		private float _timer;
		
		public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
		{
			_pathFollowBehavior = animator.gameObject.GetComponent<PathFollowBehavior>();
			_wanderBehavior = animator.gameObject.GetComponent<WanderBehavior>();

			_pathFollowBehavior.IsEnable = true;
			_wanderBehavior.IsEnable = true;

			_timer = _patrolTimer;
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
		{
			_pathFollowBehavior.IsEnable = false;
			_wanderBehavior.IsEnable = false;
		}

		public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
		{
			_timer -= Time.deltaTime;

			if (_timer <= 0)
			{
				animator.SetBool("IsPatrol", false);
			}
		}
	}
}
