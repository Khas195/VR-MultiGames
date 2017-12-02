using System;
using script.PathFinding;
using UnityEditor;
using UnityEngine;

namespace script.Editor.PathFinding
{
	[CustomEditor(typeof(PathPointInspector))]
	[CanEditMultipleObjects]
	public class PathPointEditor : UnityEditor.Editor
	{
		private PathPoint _self;
		
		private void Awake()
		{
			_self = ((PathPointInspector) target).point;
		}

		public override void OnInspectorGUI()
		{
			if (_self == null) return;

			if (!_self.hasTransform)
			{
				var newPosition = EditorGUILayout.Vector3Field("Position", _self.position);
				var newRotation = EditorGUILayout.Vector3Field("Rotation", _self.rotation.eulerAngles);
				var newScale = EditorGUILayout.Vector3Field("Scale", _self.scale);

				if (newPosition != _self.position)
				{
					_self.position = newPosition;
				}

				if (newRotation != _self.rotation.eulerAngles)
				{
					var newQuaternion = Quaternion.Euler(newRotation);
					_self.rotation = newQuaternion;
				}

				if (newScale != _self.scale)
				{
					_self.scale = newScale;
				}
			}

			var newHandleType = (PathPoint.HandleType) EditorGUILayout.EnumPopup("Handle Type",
				_self.handleType);

			if (newHandleType != _self.handleType)
			{
				_self.handleType = newHandleType;
			}

			if (_self.handleType != PathPoint.HandleType.None)
			{
				var newHandle1 = EditorGUILayout.Vector3Field("Handle 1", _self.handle1);
				var newHandle2 = EditorGUILayout.Vector3Field("Handle 2", _self.handle2);

				switch (_self.handleType)
				{
					case PathPoint.HandleType.Mirror:
					{
						if (newHandle1 != _self.handle1)
						{
							_self.handle1 = newHandle1;
						}
						else if (newHandle2 != _self.handle2)
						{
							_self.handle2 = newHandle2;
						}
					}
						break;
					case PathPoint.HandleType.Independent:
					{
						if (newHandle1 != _self.handle1)
						{
							_self.handle1 = newHandle1;
						}
						
						if (newHandle2 != _self.handle2)
						{
							_self.handle2 = newHandle2;
						}
					}
						break;
				}
			}

			EditorUtility.SetDirty(target);
		}

		private void OnSceneGUI()
		{
			if (_self == null) return;
			
			Handles.color = Color.cyan;
			if (_self.handleType != PathPoint.HandleType.None)
			{
				Vector3 newGlobal1 = Handles.PositionHandle(_self.globalHandle1, _self.rotation);
				Vector3 newGlobal2 = Handles.PositionHandle(_self.globalHandle2, _self.rotation);


				switch (_self.handleType)
				{
					case PathPoint.HandleType.Mirror:
					{
						if (newGlobal1 != _self.globalHandle1)
						{
							_self.globalHandle1 = newGlobal1;
						}
						else if (newGlobal2 != _self.globalHandle2)
						{
							_self.globalHandle2 = newGlobal2;
						}
					}
						break;
					case PathPoint.HandleType.Independent:
					{
						if (newGlobal1 != _self.globalHandle1)
						{
							_self.globalHandle1 = newGlobal1;
						}
						
						if (newGlobal2 != _self.globalHandle2)
						{
							_self.globalHandle2 = newGlobal2;
						}
					}
						break;
				}
				
				Handles.color = Color.yellow;
				Handles.DrawLine(_self.position, _self.globalHandle1);
				Handles.DrawLine(_self.position, _self.globalHandle2);
			}
		}
	}
}
