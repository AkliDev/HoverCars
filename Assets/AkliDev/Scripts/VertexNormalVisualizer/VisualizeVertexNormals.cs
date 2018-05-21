using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VisualizeVertexNormals : MonoBehaviour
{
    private Mesh _Mesh;

    [HideInInspector] [SerializeField] public bool _ShowVertexNormals;
    [HideInInspector] [SerializeField] public float _NormalLength;
    [HideInInspector] [SerializeField] public Vector3[] _Vertices;
    [HideInInspector] [SerializeField] public Vector3[] _Normals;

    //Getters
    public bool ShowVertexNormals { get { return _ShowVertexNormals; } }
    public float NormalLength { get { return _NormalLength; } }
    public Vector3[] Vertices { get { return _Vertices; } }
    public Vector3[] Normals { get { return _Normals; } }

    //Setters
    public void SetShowVertexNormals(bool Newbool)
    {
        _ShowVertexNormals = Newbool;
    }
    public void SetNormalLength(float length)
    {
        if (length > 0)
        {
            _NormalLength = length;
        }
    }
    //
    private void OnDrawGizmosSelected()
    {
        GetMeshData();
    }

    private void GetMeshData()
    {
        if (_Mesh != gameObject.GetComponent<MeshFilter>().sharedMesh)
        {
            _Mesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
            _Vertices = _Mesh.vertices;
            _Normals = _Mesh.normals;
        }
    }

    private void Reset()
    {
        _NormalLength = 0.2f;
    }



}
