using UnityEngine;
using UnityEngine.Events;

namespace script
{
	public class CheckPointArea : MonoBehaviour
	{
		[SerializeField] private Transform _point = null;
		[SerializeField] private UnityEvent _onCheckPointEnter;
		[SerializeField] private UnityEvent _onCheckPointStay;
		[SerializeField] private UnityEvent _onCheckPointExit;

		public Transform point
		{
			get { return _point; }
			set { _point = value; }
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Player"))
			{
				if (!_point)
				{
					RespawnSetting.respawnPoint = this.transform;
				}
				else
				{
					RespawnSetting.respawnPoint = _point;
				}
				
				_onCheckPointEnter.Invoke();
			}
		}

		private void OnTriggerStay(Collider other)
		{
			if (other.CompareTag("Player"))
			{
				_onCheckPointStay.Invoke();
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.CompareTag("Player"))
			{
				_onCheckPointExit.Invoke();
			}
		}
	}
}
