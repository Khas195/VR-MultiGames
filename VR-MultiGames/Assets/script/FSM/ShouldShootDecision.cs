using script.BoidBehavior;
using UnityEditor;
using UnityEngine;

namespace script.FSM
{
	[CreateAssetMenu(menuName = "FSM/Decision/ShouldShootDecision")]
	public class ShouldShootDecision : Decision
	{
		[SerializeField] private float _minDistance = 10;

		private bool CheckTarget(StateController controller)
		{
				RaycastHit raycastHit;
				if (Physics.Raycast(controller.transform.position, controller.transform.forward, out raycastHit, _minDistance * 10,
					LayerMask.GetMask("Obstacle")))
				{
					var renderer = raycastHit.transform.GetComponent<Renderer>();
					var texture = renderer.material.mainTexture as Texture2D;
					var color = texture.GetPixel((int) raycastHit.textureCoord.x, (int) raycastHit.textureCoord.y);

					if (Ultil.CalColorDifference(color, controller._color) > 1)
					{
						return true;
					}
			}
			return false;
		}

		public override bool Decide(StateController controller)
		{
			return CheckTarget(controller);
		}
	}
}