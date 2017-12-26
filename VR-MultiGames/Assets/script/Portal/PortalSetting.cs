using System;
using UnityEngine;

namespace script.Portal
{
	public class PortalSetting : MonoBehaviour 
	{
		[Serializable]
		public struct ColorPortal
		{
			public Color PortalColor;
			public PortalController Portal;
		}

		[SerializeField] private ColorPortal _startPortalA;
		[SerializeField] private ColorPortal _startPortalB;

		private static ColorPortal _portalA;
		private static ColorPortal _portalB;

		private void Start()
		{
			_portalA = _startPortalA;
			_portalB = _startPortalB;
		
			if (_portalA.Portal && _portalB.Portal)
			{
				if (_portalA.Portal == _portalB.Portal)
				{
					_portalB.Portal = null;
					return;
				}
				
				Conntect(_portalA.Portal, _portalB.Portal);
			
				_portalA.Portal.color = _portalA.PortalColor;
				_portalA.Portal.SetActive(true);
				_portalA.Portal.SetGlow(true);

				_portalB.Portal.color = _portalB.PortalColor;
				_portalB.Portal.SetActive(true);
				_portalB.Portal.SetGlow(true);
			}
		}

		private static void Conntect(PortalController A, PortalController B)
		{
			Debug.Log("A: " + A + " connect B : " + B );
			A.Conntect(B);
			Debug.Log("B: " + B + " connect A : " + A );
			B.Conntect(A);
		}

		public static void OnPaintPortal(PortalController portal, Color color)
		{
			if (Ultil.CalColorDifference(_portalA.PortalColor, color) < 0.5f)
			{
				if(_portalA.Portal == portal) return;

				if (_portalB.Portal == portal)
				{
					_portalB.Portal.SetActive(false);
					_portalB.Portal.SetGlow(false);
					_portalB.Portal = null;
				}
			
				if (_portalA.Portal)
				{
					_portalA.Portal.SetActive(false);
					_portalA.Portal.SetGlow(false);
				}
			
				_portalA.Portal = portal;

				if (_portalB.Portal != null && _portalB.Portal != _portalA.Portal)
				{
					Conntect(_portalA.Portal, _portalB.Portal);
			
					_portalA.Portal.color = _portalA.PortalColor;
					_portalA.Portal.SetActive(true);
					_portalA.Portal.SetGlow(true);

					_portalB.Portal.color = _portalB.PortalColor;
					_portalB.Portal.SetActive(true);
					_portalB.Portal.SetGlow(true);
				}
			}
			else if (Ultil.CalColorDifference(_portalB.PortalColor, color) < 0.5f)
			{
				if(_portalB.Portal == portal) return;

				if (_portalA.Portal == portal)
				{
					_portalA.Portal.SetActive(false);
					_portalA.Portal.SetGlow(false);
					_portalA.Portal = null;
				}
			
				if (_portalB.Portal)
				{
					_portalB.Portal.SetActive(false);
					_portalB.Portal.SetGlow(false);
				}
				
				_portalB.Portal = portal;

				if (_portalA.Portal != null && _portalA.Portal != _portalB.Portal)
				{
					Conntect(_portalA.Portal, _portalB.Portal);
			
					_portalA.Portal.color = _portalA.PortalColor;
					_portalA.Portal.SetActive(true);
					_portalA.Portal.SetGlow(true);

					_portalB.Portal.color = _portalB.PortalColor;
					_portalB.Portal.SetActive(true);
					_portalB.Portal.SetGlow(true);
				}
			}
		}
	}
}
