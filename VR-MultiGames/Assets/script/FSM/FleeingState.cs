using script.BoidBehavior;
using UnityEngine;

namespace script.FSM
{
	public class FleeingState : StateMachineBehaviour
	{
		[SerializeField]
		private float _duration = 3;
	
		private BoidController _boidController;
		private GameObject _gameObject;
		private FleeBehavior _fleeBehavior;
		private float _elapse;

		public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
		{
			_boidController = animator.gameObject.GetComponent<BoidController>();
			_fleeBehavior = animator.gameObject.GetComponent<FleeBehavior>();

			if (!_fleeBehavior)
			{
				_fleeBehavior = animator.gameObject.AddComponent<FleeBehavior>();
			}
		
			var position = new Vector3(animator.GetFloat("x"), animator.GetFloat("y"), animator.GetFloat("z"));

			if (!_boidController || !_fleeBehavior) return;

			if (!_gameObject)
			{
				_gameObject = new GameObject();
			}
	
			_gameObject.transform.position = position;
			_boidController.Target = _gameObject;

			_fleeBehavior.IsEnable = true;

			_elapse = 0;
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
		{
			_fleeBehavior = animator.gameObject.GetComponent<FleeBehavior>();
			_fleeBehavior.IsEnable = false;

			_boidController.Target = null;
		}

		public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
		{
			_elapse += Time.deltaTime;

			if (_elapse > _duration)
			{
				animator.SetBool("Is Fleeing", false);
			}
		}
	}
}
