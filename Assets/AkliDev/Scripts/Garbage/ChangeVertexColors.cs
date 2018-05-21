using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeVertexColors : MonoBehaviour
{
    [SerializeField] Color _Color;
    private Mesh _Mesh;
    private Color[] _VertexColors;

    void Start()
    {
        _Mesh = GetComponent<MeshFilter>().sharedMesh;
        _VertexColors = new Color[_Mesh.vertices.Length];
    }

    void Update()
    {
        for (int i = 0; i < _VertexColors.Length; i++)
        {
            _VertexColors[i] = new Color(_Color.r, _Color.g, _Color.b);
        }
        _Mesh.colors = _VertexColors;
    }
}
