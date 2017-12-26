using System.Collections.Generic;
using Assets.script;
using UnityEngine;

namespace script.Portal
{
	public class PortalController : MonoBehaviour
	{
		[SerializeField]
		private Paintable _paintReceiver;

		[SerializeField] 
		private List<GameObject> _portalFrame;
		
		private void Start()
		{
			foreach (var frame in _portalFrame)
			{
				var mat = Ultil.GetMaterialWithShader(frame.GetComponent<Renderer>().materials, PaintableDefinition.PaintableShaderName, name);

				var initScale = GameSettings.GetInstance().standardScale / this.transform.localScale.x;
				int initSize = (int) (GameSettings.GetInstance().standardPaintSize / initScale);

				var drawTexture = new Texture2D(initSize, initSize);

				ResetTextureToColor(drawTexture, new Color(0, 0, 0, 0));
				mat.SetTexture(PaintableDefinition.DrawOnTextureName, drawTexture);
			}	
			
			_paintReceiver.newPaintEvent.AddListener(TriggerBox);
		}

		private void ResetTextureToColor(Texture2D texture, Color color)
		{
			Color[] colors = texture.GetPixels();
			for (int i = 0; i < colors.Length; i++)
			{
				colors[i] = color;
			}
			texture.SetPixels(colors);
			texture.Apply();
		}

		public void TriggerBox(Color color)
		{
			var glowable = _paintReceiver.GetComponent<Glowable>();
			glowable.Glow();
		}
	}
}
