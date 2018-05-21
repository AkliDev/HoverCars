using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct WindowVertexIndices
{
    public int vertex0, vertex1, vertex2, vertex3;

    public WindowVertexIndices(int Vertex0, int Vertex1, int Vertex2, int Vertex3)
    {
        vertex0 = Vertex0;
        vertex1 = Vertex1;
        vertex2 = Vertex2;
        vertex3 = Vertex3;
    }
}

public class OrderVerticesOfTowerWindows : MonoBehaviour
{
    //[SerializeField] private Vector2Int _Index;

    [SerializeField] private BuildingManager _Manager;
    
    
    private Renderer _Renderer;
    private Mesh _Mesh;
    private int _TotalVertexCount;
    private Color[] _VertexColors;

     private Color _WindowColor;

    private int _VertexCountPerGroup;
   
    private WindowVertexIndices[,] _WindowVertexGroup0;
    private WindowVertexIndices[,] _WindowVertexGroup1;
    private WindowVertexIndices[,] _WindowVertexGroup2;
    private WindowVertexIndices[,] _WindowVertexGroup3;

    [SerializeField] private Frequency _WindowGroupFrequency0;
    [SerializeField] private Frequency _WindowGroupFrequency1;
    [SerializeField] private Frequency _WindowGroupFrequency2;
    [SerializeField] private Frequency _WindowGroupFrequency3;

    [SerializeField] private int _WindowGroupMultiplier0 = 1;
    [SerializeField] private int _WindowGroupMultiplier1 = 1 ;
    [SerializeField] private int _WindowGroupMultiplier2 = 1;
    [SerializeField] private int _WindowGroupMultiplier3 = 1;

    private void Awake()
    {
        _WindowColor = transform.parent.GetComponent<ChangeMaterialColor>().Color;
        _WindowColor = new Color(_WindowColor.r * 0.7f, _WindowColor.g * 0.7f, _WindowColor.b * 0.7f);

        MeshFilter meshFilter  = GetComponent<MeshFilter>();
        _Renderer = GetComponent<Renderer>();
        _Mesh = (Mesh)Instantiate(meshFilter.sharedMesh);
        meshFilter.sharedMesh = _Mesh;

        _TotalVertexCount = _Mesh.vertices.Length;
        _VertexColors = new Color[_TotalVertexCount];
        _VertexCountPerGroup = (int)((float)_TotalVertexCount * 0.25f);

        PutVerticesIntoGroups();

        WindowVertexIndices[][,] windowVertexGroups = new WindowVertexIndices[][,]
        {
            _WindowVertexGroup0,
            _WindowVertexGroup1,
            _WindowVertexGroup2,
            _WindowVertexGroup3,
        };
        Frequency[] windowGroupFrequencys = new Frequency[]
        {
            _WindowGroupFrequency0,
            _WindowGroupFrequency1,
            _WindowGroupFrequency2,
            _WindowGroupFrequency3
        };

        int[] windowGroupMultipliers = new int[]
        {
            _WindowGroupMultiplier0,
            _WindowGroupMultiplier1,
            _WindowGroupMultiplier2,
            _WindowGroupMultiplier3
        };

        _Manager.AddTowerWindowData(windowVertexGroups, windowGroupFrequencys, windowGroupMultipliers, _Renderer, _Mesh, _VertexColors, _WindowColor);
    }
    
    void PutVerticesIntoGroups()
    {
        _WindowVertexGroup0 = new WindowVertexIndices[8, 22];
        _WindowVertexGroup1 = new WindowVertexIndices[8, 22];
        _WindowVertexGroup2 = new WindowVertexIndices[8, 22];
        _WindowVertexGroup3 = new WindowVertexIndices[8, 22];

        int x0 = 0;
        int y0 = 0;

        for (int i = 4; i < _VertexCountPerGroup + 4; i += 4)
        {
            _WindowVertexGroup0[x0, y0].vertex0 = i - 4;
            _WindowVertexGroup0[x0, y0].vertex1 = i - 3;
            _WindowVertexGroup0[x0, y0].vertex2 = i - 2;
            _WindowVertexGroup0[x0, y0].vertex3 = i - 1;

            x0++;
            if (x0 == _WindowVertexGroup0.GetLength(0))
            {
                x0 = 0;
                y0++;
            }
        }

        int x1 = 0;
        int y1 = 0;

        for (int i = _VertexCountPerGroup + 4; i < _VertexCountPerGroup * 2 + 4; i += 4)
        {
            _WindowVertexGroup1[x1, y1].vertex0 = i - 4;
            _WindowVertexGroup1[x1, y1].vertex1 = i - 3;
            _WindowVertexGroup1[x1, y1].vertex2 = i - 2;
            _WindowVertexGroup1[x1, y1].vertex3 = i - 1;

            x1++;
            if (x1 == _WindowVertexGroup1.GetLength(0))
            {
                x1 = 0;
                y1++;
            }
        }

        int x2 = 0;
        int y2 = 0;

        for (int i = _VertexCountPerGroup * 2 + 4; i < _VertexCountPerGroup * 3 + 4; i += 4)
        {
            _WindowVertexGroup2[x2, y2].vertex0 = i - 4;
            _WindowVertexGroup2[x2, y2].vertex1 = i - 3;
            _WindowVertexGroup2[x2, y2].vertex2 = i - 2;
            _WindowVertexGroup2[x2, y2].vertex3 = i - 1;

            x2++;
            if (x2 == _WindowVertexGroup2.GetLength(0))
            {
                x2 = 0;
                y2++;
            }
        }

        int x3 = 0;
        int y3 = 0;

        for (int i = _VertexCountPerGroup * 3 + 4; i < _VertexCountPerGroup * 4 + 4; i += 4)
        {
            _WindowVertexGroup3[x3, y3].vertex0 = i - 4;
            _WindowVertexGroup3[x3, y3].vertex1 = i - 3;
            _WindowVertexGroup3[x3, y3].vertex2 = i - 2;
            _WindowVertexGroup3[x3, y3].vertex3 = i - 1;

            x3++;
            if (x3 == _WindowVertexGroup3.GetLength(0))
            {
                x3 = 0;
                y3++;
            }
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(_Mesh.vertices[_WindowVertexGroup3[_Index.x, _Index.y].vertex0], 2);
    //    Gizmos.DrawSphere(_Mesh.vertices[_WindowVertexGroup3[_Index.x, _Index.y].vertex1], 2);
    //    Gizmos.DrawSphere(_Mesh.vertices[_WindowVertexGroup3[_Index.x, _Index.y].vertex2], 2);
    //    Gizmos.DrawSphere(_Mesh.vertices[_WindowVertexGroup3[_Index.x, _Index.y].vertex3], 2);
    //}

    //private void Update()
    //{
    //    _VertexColors[_WindowVertexGroup3[_Index.x, _Index.y].vertex0] = new Color(1, 0, 1);
    //    if (Input.GetKeyDown(KeyCode.RightArrow))
    //        _Index.x++;
    //    if (Input.GetKeyDown(KeyCode.LeftArrow))
    //        _Index.x--;
    //    if (Input.GetKeyDown(KeyCode.UpArrow))
    //        _Index.y++;
    //    if (Input.GetKeyDown(KeyCode.DownArrow))
    //        _Index.y--;

    //    _Mesh.colors = _VertexColors;
    //}
}
