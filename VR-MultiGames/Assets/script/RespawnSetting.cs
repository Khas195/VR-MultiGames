using UnityEngine;

namespace script
{
	public class RespawnSetting : MonoBehaviour
	{
		[SerializeField]
		private Transform _startingPoint = null;
		
		private static Transform _respawnPoint;
		
		public static Transform respawnPoint
		{
			get { return _respawnPoint; }
			set { _respawnPoint = value; }
		}

		private void Awake()
		{
			if (!_startingPoint)
			{
				Debug.LogWarning("Starting point is not set");
				Debug.Break();
			}
		}
	}
}
