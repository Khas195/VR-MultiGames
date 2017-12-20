using script.BoidBehavior;
using UnityEngine;

namespace script.FSM
{
	public class WanderingState : StateMachineBehaviour
	{
		[SerializeField] 
		private float _radius = 2;

		[SerializeField]
		private LayerMask _layerMask;

		[SerializeField] 
		private float _checkTime = 0.2f;

		private BoidController _boidController;
		private float _elapse;

		public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
		{
			_elapse = 0;
			_boidController = animator.gameObject.GetComponent<BoidController>();
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
		{
			if (!_boidController) return;
		
			_elapse += Time.deltaTime;

			if (!(_elapse > _checkTime)) return;
		
			var collider = Physics.OverlapSphere(_boidController.transform.position, _radius, _layerMask);
			if (collider.Length > 0)
			{
				animator.SetFloat("x", collider[0].transform.position.x);
				animator.SetFloat("y", collider[0].transform.position.y);
				animator.SetFloat("z", collider[0].transform.position.z);
				animator.SetBool("Is Fleeing", true);
				return;
			}
			
			_elapse = 0;
		}
	}
}
