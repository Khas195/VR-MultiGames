using System;
using System.Collections.Generic;
using System.Linq;
using Assets.script;
using UnityEngine;

namespace script.Portal
{
	[Serializable]
	public struct PortalPaint
	{
		public Color PortalColor;
		public PortalController Portal;
	}
	
	public class PortalController : MonoBehaviour
	{
		[Serializable]
		public struct PortalData
		{
			public Transform CornerTL;
			public Transform CornerTR;
			public Transform CornerBL;
			public Transform CornerBR;

			public Transform LookTarget;
		}

		[SerializeField] 
		private bool _isStationary = false;

		[SerializeField] 
		private PortalData _data;
		
		[SerializeField] 
		private PortalCamera _portalSetting;

		[SerializeField] 
		private Sender _teleporter;

		[SerializeField]
		private Paintable _paintReceiver;

		[SerializeField] 
		private List<Glowable> _portalFrame;

		[SerializeField] 
		private GameObject _portalControl;

		[SerializeField]
		private Color _color;

		public PortalCamera portalSetting
		{
			get { return _portalSetting; }
		}

		public PortalData data
		{
			get { return _data; }
		}

		public Color color
		{
			get { return _color; }
			set { _color = value; }
		}

		private void Start()
		{
			_paintReceiver.newPaintEvent.AddListener(TriggerBox);
			
			_portalFrame = GetComponentsInChildren<Glowable>().ToList();
			_portalSetting = GetComponentsInChildren<PortalCamera>(true)[0];
			_teleporter = GetComponentsInChildren<Sender>(true)[0];
			
			foreach (var frame in _portalFrame)
			{
				var mat = Ultil.GetMaterialWithShader(frame.GetComponent<Renderer>().materials, PaintableDefinition.PaintableShaderName, name);

				var initScale = GameSettings.GetInstance().standardScale / this.transform.localScale.x;
				int initSize = (int) (GameSettings.GetInstance().standardPaintSize / initScale);

				var drawTexture = new Texture2D(initSize, initSize);

				ResetTextureToColor(drawTexture, new Color(0, 0, 0, 0));
				mat.SetTexture(PaintableDefinition.DrawOnTextureName, drawTexture);
			}

			if (_isStationary)
			{
				if (_portalSetting.portal && _portalSetting.otherPortal && _portalSetting.portal != _portalSetting.otherPortal)
				{
					SetActive(true);
					SetGlow(true);
					
					_portalSetting.portal.GetComponent<PortalController>().SetActive(true);
					_portalSetting.portal.GetComponent<PortalController>().SetGlow(true);
				}
			}
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

		private void TriggerBox(Color color)
		{
			if (!_isStationary)
				PortalSetting.OnPaintPortal(this, color);
		}

		public void Conntect(PortalController portal)
		{
			if(this == portal) return;
			
			_portalSetting.portal = portal.gameObject;
			_portalSetting.corner_TL = portal._data.CornerTL;
			_portalSetting.corner_TR = portal._data.CornerTR;
			_portalSetting.corner_BL = portal._data.CornerBL;
			_portalSetting.corner_BR = portal._data.CornerBR;
			_portalSetting.lookTarget = portal._data.LookTarget;

			_teleporter.receiver = portal._teleporter.gameObject;
		}

		public void SetActive(bool active)
		{
			_portalControl.SetActive(active);
		}

		public void SetGlow(bool isGlow)
		{
			foreach (var frame in _portalFrame)
			{
				if (isGlow)
				{
					frame.GlowColor = _color;
					frame.Glow();
				}
				else
				{
					frame.StopGlow();
				}
			}
		}
	}
}
