using System;
using UnityEngine;

namespace script.PathFinding
{
	[Serializable]
	public class PathPoint
	{
		public enum HandleType
		{
			None,
			Mirror,
			Independent
		}

		[SerializeField]
		private bool _hasChanged;
		[SerializeField]
		private Transform _transform = null;
		[SerializeField]
		private HandleType _handleType = HandleType.None;
		[SerializeField]
		private Vector3 _position = Vector3.zero;
		[SerializeField]
		private Quaternion _rotation = Quaternion.identity;
		[SerializeField]
		private Vector3 _scale = Vector3.one;
		[SerializeField]
		private Vector3 _handle1 = Vector3.zero;
		[SerializeField]
		private Vector3 _handle2 = Vector3.zero;
		private Matrix4x4 _localToWorldMatrix = Matrix4x4.identity;
		

		public HandleType handleType
		{
			get { return _handleType; }
			set
			{
				if(_handleType == value) return;
				
				_hasChanged = true;
				_handleType = value;

				switch (_handleType)
				{
					case HandleType.None:
					{
						_handle1 = _handle2 = Vector3.zero;
					}
						break;
					case HandleType.Mirror:
					{
						if (_handle1 != Vector3.zero)
						{
							_handle2 = -_handle1;
						}
						else if (_handle2 != Vector3.zero)
						{
							_handle1 = -_handle2;
						}
						else
						{
							_handle2 = new Vector3(-1, 0, 0);
							_handle1 = -_handle2;
						}
					}
						break;
					case HandleType.Independent:
					{
						if (_handle1 == Vector3.zero && _handle2 == Vector3.zero)
						{
							_handle2 = new Vector3(-1, 0, 0);
							_handle1 = -_handle2;
						}
					}
						break;
				}
			}
		}

		public Vector3 position
		{
			get { return _transform ? _transform.position : _position; }
			set
			{
				if(_position == value) return;
				
				_hasChanged = true;

				if (_transform)
				{
					_transform.position = value;
				}
				else
				{
					_position = value;
					_localToWorldMatrix = Matrix4x4.TRS(_position, _rotation, _scale);
				}
			}
		}

		public Quaternion rotation
		{
			get { return _transform ? _transform.rotation : _rotation; }
			set
			{
				if (_rotation == value) return;

				_hasChanged = true;

				if (_transform)
				{
					_transform.rotation = value;
				}
				else
				{
					_rotation = value;
					_localToWorldMatrix = Matrix4x4.TRS(_position, _rotation, _scale);
				}
			}
		}
		
		public Vector3 scale
		{
			get { return _transform ? _transform.localScale : _scale; }
			set
			{
				if(_scale == value) return;
				
				_hasChanged = true;

				if (_transform)
				{
					_transform.localScale = value;
				}
				else
				{
					_scale = value;
					_localToWorldMatrix = Matrix4x4.TRS(_position, _rotation, _scale);
				}
			}
		}

		public Matrix4x4 localToWorldMatrix
		{
			get { return _transform ? _transform.localToWorldMatrix : _localToWorldMatrix; }
		}

		public Vector3 handle1
		{
			get { return _handle1; }
			set
			{
				if (_handle1 == value) return;
				
				_hasChanged = true;
				_handle1 = value;

				switch (_handleType)
				{
					case HandleType.None:
					{
						if (_handle1 != Vector3.zero)
						{
							handleType = HandleType.Independent;
						}
					}
						break;
					case HandleType.Mirror:
					{
						if (_handle1 == Vector3.zero)
						{
							handleType = HandleType.None;
						}
						else
						{
							_handle2 = -_handle1;
						}
					}
						break;
					case HandleType.Independent:
					{
						if (_handle1 == Vector3.zero && _handle2 == Vector3.zero)
						{
							handleType = HandleType.None;
						}
					}
						break;
				}
			}
		}

		public Vector3 handle2
		{
			get { return _handle2; }
			set
			{
				if(_handle2 == value) return;
				
				_hasChanged = true;
				_handle2 = value;

				switch (_handleType)
				{
					case HandleType.None:
					{
						if (_handle2 != Vector3.zero)
						{
							handleType = HandleType.Independent;
						}
					}
						break;
					case HandleType.Mirror:
					{
						if (_handle2 == Vector3.zero)
						{
							handleType = HandleType.None;
						}
						else
						{
							_handle1 = -_handle2;
						}
					}
						break;
					case HandleType.Independent:
					{
						if (_handle1 == Vector3.zero && _handle2 == Vector3.zero)
						{
							handleType = HandleType.None;
						}
					}
						break;
				}
			}
		}

		public Vector3 globalHandle1
		{
			get { return _transform ? _transform.TransformPoint(_handle1) : _localToWorldMatrix.MultiplyPoint3x4(_handle1); }
			set
			{
				_hasChanged = true;

				handle1 = _transform ? _transform.InverseTransformPoint(value) :
					_localToWorldMatrix.inverse.MultiplyPoint3x4(value);
			}
		}

		public Vector3 globalHandle2
		{
			get { return _transform ? _transform.TransformPoint(_handle2) : _localToWorldMatrix.MultiplyPoint3x4(_handle2); }
			set 
			{
				_hasChanged = true;

				handle2 = _transform ? _transform.InverseTransformPoint(value) : 
					_localToWorldMatrix.inverse.MultiplyPoint3x4(value);
			}
		}

		public bool hasChanged
		{
			get { return _hasChanged; }
			set { _hasChanged = value; }
		}

		public bool hasTransform
		{
			get { return _transform; }
		}

		public Transform transform
		{
			get { return _transform; }
			set { _transform = value; }
		}
	}
}
