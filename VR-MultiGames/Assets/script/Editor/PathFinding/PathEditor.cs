using System;
using script.PathFinding;
using UnityEditor;
using UnityEngine;

namespace script.Editor.PathFinding
{
    [CustomEditor(typeof(PathInspector))]
    [CanEditMultipleObjects]
    public class PathEditor : UnityEditor.Editor
    {
        private enum EditMode
        {
            None,
            Position,
            Rotation,
            Scale
        }
        
        private Path _path;
        private PathInspector _self;
        private bool _showPoints = true;
        private bool _makePointGameObject = false;
        private EditMode _curMode = EditMode.Position;

        private void Awake()
        {
            _self = (PathInspector) target;
            _path = _self.path;
        }

        public override void OnInspectorGUI()
        {
            var newStepNum = EditorGUILayout.IntField("Number of step", _path.bezierCurveStepNum);

            var newPathType = (Path.PathType) EditorGUILayout.EnumPopup("Path Type",
                _path.pathType);

            var newPathStyle = (Path.PathStyle) EditorGUILayout.EnumPopup("Path Style",
                _path.pathStyle);

            var newBezierCurveType = (BezierCurve.BezierCurveType) EditorGUILayout.EnumPopup("Bezier Curve Type",
                _path.bezierCurveType);

            _path.isDrawGizmos = EditorGUILayout.Toggle("Is Draw Gizmos", _path.isDrawGizmos);

            if (newStepNum != _path.bezierCurveStepNum)
            {
                _path.bezierCurveStepNum = newStepNum;
            }

            if (newPathType != _path.pathType)
            {
                _path.pathType = newPathType;
            }

            if (newPathStyle != _path.pathStyle)
            {
                _path.pathStyle = newPathStyle;
            }

            if (newBezierCurveType != _path.bezierCurveType)
            {
                _path.bezierCurveType = newBezierCurveType;
            }
            
            EditorGUILayout.Foldout(_showPoints, "Points");

            if (_showPoints)
            {
                for (int i = 0; i < _path.pointList.Count; ++i)
                {
                    DrawPointInspector(_path.pointList[i], i);
                }
			
                if(GUILayout.Button("Add Point"))
                {
                    _path.Add(new PathPoint());
                }
            }
        }
	
        void OnSceneGUI()
        {
            Event e = Event.current;
            switch (e.type)
            {
                case EventType.KeyUp:
                {
                    switch (e.keyCode)
                    {
                        case KeyCode.W:
                        {
                            _curMode = EditMode.Position;
                        }
                            break;

                        case KeyCode.E:
                        {
                            _curMode = EditMode.Rotation;
                        }
                            break;

                        case KeyCode.R:
                        {
                            _curMode = EditMode.Scale;
                        }
                            break;

                        case KeyCode.Q:
                        {
                                _curMode = EditMode.None;
                        }
                            break;
                    }
                }
                    break;
            }
            
            for(int i = 0; i < _path.pointList.Count; i++)
            {
                DrawPointOnScene(_path.pointList[i]);
            }
        }

        private void DrawPointInspector(PathPoint point, int index)
        {
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                _path.Remove(point);
            }

            if (index != 0 && GUILayout.Button(@"/\", GUILayout.Width(25)))
            {
                var temp = _path.pointList[index - 1];
                _path.pointList[index - 1] = point;
                _path.pointList[index] = temp;
                _path.CalculatePath();
            }

            if (index != _path.pointList.Count - 1 && GUILayout.Button(@"\/", GUILayout.Width(25)))
            {
                var temp = _path.pointList[index + 1];
                _path.pointList[index + 1] = point;
                _path.pointList[index] = temp;
                _path.CalculatePath();
            }
            
            EditorGUILayout.EndHorizontal();
            
            EditorGUI.indentLevel++;
            EditorGUI.indentLevel++;

            if (!point.hasTransform)
            {
                var newPosition = EditorGUILayout.Vector3Field("Position", point.position);
                var newRotation = EditorGUILayout.Vector3Field("Rotation", point.rotation.eulerAngles);
                var newScale = EditorGUILayout.Vector3Field("Scale", point.scale);

                if (newPosition != point.position)
                {
                    point.position = newPosition;
                }

                if (newRotation != point.rotation.eulerAngles)
                {
                    var newQuaternion = Quaternion.Euler(newRotation);
                    point.rotation = newQuaternion;
                }

                if (newScale != point.scale)
                {
                    point.scale = newScale;
                }
            }

            var newHandleType = (PathPoint.HandleType) EditorGUILayout.EnumPopup("Handle Type",
                point.handleType);

            if (newHandleType != point.handleType)
            {
                point.handleType = newHandleType;
            }

            if (point.handleType != PathPoint.HandleType.None)
            {
                var newHandle1 = EditorGUILayout.Vector3Field("Handle 1", point.handle1);
                var newHandle2 = EditorGUILayout.Vector3Field("Handle 2", point.handle2);

                switch (point.handleType)
                {
                    case PathPoint.HandleType.Mirror:
                    {
                        if (newHandle1 != point.handle1)
                        {
                            point.handle1 = newHandle1;
                        }
                        else if (newHandle2 != point.handle2)
                        {
                            point.handle2 = newHandle2;
                        }
                    }
                        break;
                    case PathPoint.HandleType.Independent:
                    {
                        if (newHandle1 != point.handle1)
                        {
                            point.handle1 = newHandle1;
                        }
						
                        if (newHandle2 != point.handle2)
                        {
                            point.handle2 = newHandle2;
                        }
                    }
                        break;
                }
            }
            
            EditorGUI.indentLevel--;
            EditorGUI.indentLevel--;

            if (point.hasChanged)
            {
                point.hasChanged = false;
                _path.CalculatePath();
            }
        }

        private void DrawPointOnScene(PathPoint point)
        {
            Handles.color = Color.green;


            switch (_curMode)
            {
                case EditMode.Position:
                {
                    var newPosition = Handles.PositionHandle(point.position, point.rotation);
                    if (newPosition != point.position)
                    {
                        point.position = newPosition;
                    }
                }
                    break;
                case EditMode.Rotation:
                {
                    var newRotation = Handles.RotationHandle(point.rotation, point.position);
                    if (newRotation != point.rotation)
                    {
                        point.rotation = newRotation;
                    }
                }
                    break;
                case EditMode.Scale:
                {
                    var newScale = Handles.ScaleHandle(point.scale, point.position, point.rotation, 10);
                    if (newScale != point.scale)
                    {
                        point.scale = newScale;
                    }
                }
                    break;
            }

            if (point.handleType != PathPoint.HandleType.None)
            {
                var newGlobal1 = Handles.PositionHandle(point.globalHandle1, point.rotation);
                var newGlobal2 = Handles.PositionHandle(point.globalHandle2, point.rotation);


                switch (point.handleType)
                {
                    case PathPoint.HandleType.Mirror:
                    {
                        if (newGlobal1 != point.globalHandle1)
                        {
                            point.globalHandle1 = newGlobal1;
                        }
                        else if (newGlobal2 != point.globalHandle2)
                        {
                            point.globalHandle2 = newGlobal2;
                        }
                    }
                        break;
                    case PathPoint.HandleType.Independent:
                    {
                        if (newGlobal1 != point.globalHandle1)
                        {
                            point.globalHandle1 = newGlobal1;
                        }
						
                        if (newGlobal2 != point.globalHandle2)
                        {
                            point.globalHandle2 = newGlobal2;
                        }
                    }
                        break;
                }
            
                Handles.color = Color.yellow;
                Handles.DrawLine(point.position, point.globalHandle1);
                Handles.DrawLine(point.position, point.globalHandle2);
            }

            if (point.hasChanged)
            {
                point.hasChanged = false;
                _path.CalculatePath();
            }
        }
    }
}
