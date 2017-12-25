using UnityEngine;

namespace script
{
	public class RespawnArea : MonoBehaviour
	{
		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.CompareTag("Player"))
			{
				var player = other.gameObject.transform;
				player.position = RespawnSetting.respawnPoint.position;
				player.forward =  RespawnSetting.respawnPoint.forward;
				player.right =  RespawnSetting.respawnPoint.right;
			}
		}
	}
}
