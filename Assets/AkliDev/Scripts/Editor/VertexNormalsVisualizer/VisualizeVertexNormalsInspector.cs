using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VisualizeVertexNormals))]
public class VisualizeVertexNormalsInspector : Editor
{
    private VisualizeVertexNormals _Visualizer;

    private void OnSceneGUI()
    {
        _Visualizer = target as VisualizeVertexNormals;

        if (_Visualizer.ShowVertexNormals)
        {
            DrawNormals();
        }
        
    }
    private void DrawNormals()
    {
        Handles.color = Color.magenta;
        for (int i = 0; i < _Visualizer.Vertices.Length; i++)
        {
            Handles.DrawLine(_Visualizer.transform.TransformPoint(_Visualizer.Vertices[i]), _Visualizer.transform.TransformPoint(_Visualizer.Vertices[i] + (_Visualizer.Normals[i]) * _Visualizer.NormalLength));
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        _Visualizer = target as VisualizeVertexNormals;

        EditorGUI.BeginChangeCheck();
        bool showVertexNormals = EditorGUILayout.Toggle("Show Vertex Normals", _Visualizer.ShowVertexNormals);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_Visualizer, "Show Vertex Normals Toggle Altered");
            EditorUtility.SetDirty(_Visualizer);
            _Visualizer.SetShowVertexNormals(showVertexNormals);
        }

        if (_Visualizer.ShowVertexNormals)
        {
            EditorGUI.BeginChangeCheck();
            float NormalLength = EditorGUILayout.FloatField("Length Of Normals", _Visualizer.NormalLength);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(_Visualizer, "Length Of Normals Altered");
                EditorUtility.SetDirty(_Visualizer);
                _Visualizer.SetNormalLength(NormalLength);
            }
        }
    }   
}
