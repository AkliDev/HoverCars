using System.Collections.Generic;
using UnityEngine;



public class TowerWindowGenerator : MonoBehaviour
{
    [SerializeField] int _Index;
    
    private Transform[,] _WindowGroup0Transform;
    private Transform[,] _WindowGroup1Transform;
    private Transform[,] _WindowGroup2Transform;
    private Transform[,] _WindowGroup3Transform;
    private List<Transform> _AllWindows;

    [SerializeField] Vector3 VertexOffster;
    private Mesh _NewWindowsMesh;
    private Vector3[] _Vertices;
    private int[] _Triangles;
    private Color[] _VertexColors;

    void Awake()
    {
        _NewWindowsMesh = new Mesh();
        GetWindows();
        FillAllWindowsArray();
        CreateNewMesh();
    }

    private void Start()
    {
        
    }

    void GetWindows()
    {
        GameObject windowGroupContainer = transform.GetChild(1).gameObject;
        List<GameObject> groups = new List<GameObject>();

        foreach (Transform child in windowGroupContainer.transform)
        {
            groups.Add(child.gameObject);
        }

        List<GameObject> windowsRows0 = new List<GameObject>();
        List<GameObject> windowsRows1 = new List<GameObject>();
        List<GameObject> windowsRows2 = new List<GameObject>();
        List<GameObject> windowsRows3 = new List<GameObject>();
        for (int i = 0; i < groups.Count; i++)
        {
            switch (i)
            {
                case 0:
                    foreach (Transform child in groups[i].transform)
                    {
                        windowsRows0.Add(child.gameObject);
                    }
                    break;
                case 1:
                    foreach (Transform child in groups[i].transform)
                    {
                        windowsRows1.Add(child.gameObject);
                    }
                    break;
                case 2:
                    foreach (Transform child in groups[i].transform)
                    {
                        windowsRows2.Add(child.gameObject);
                    }
                    break;
                case 3:
                    foreach (Transform child in groups[i].transform)
                    {
                        windowsRows3.Add(child.gameObject);
                    }
                    break;
            }
        }

        List<GameObject> Rows0Windows0 = new List<GameObject>();
        List<GameObject> Rows0Windows1 = new List<GameObject>();
        List<GameObject> Rows0Windows2 = new List<GameObject>();
        List<GameObject> Rows0Windows3 = new List<GameObject>();
        List<GameObject> Rows0Windows4 = new List<GameObject>();
        List<GameObject> Rows0Windows5 = new List<GameObject>();
        List<GameObject> Rows0Windows6 = new List<GameObject>();
        List<GameObject> Rows0Windows7 = new List<GameObject>();
        for (int i = 0; i < windowsRows0.Count; i++)
        {
            switch (i)
            {
                case 0:
                    foreach (Transform child in windowsRows0[i].transform)
                    {
                        Rows0Windows0.Add(child.gameObject);
                    }
                    break;
                case 1:
                    foreach (Transform child in windowsRows0[i].transform)
                    {
                        Rows0Windows1.Add(child.gameObject);
                    }
                    break;
                case 2:
                    foreach (Transform child in windowsRows0[i].transform)
                    {
                        Rows0Windows2.Add(child.gameObject);
                    }
                    break;
                case 3:
                    foreach (Transform child in windowsRows0[i].transform)
                    {
                        Rows0Windows3.Add(child.gameObject);
                    }
                    break;
                case 4:
                    foreach (Transform child in windowsRows0[i].transform)
                    {
                        Rows0Windows4.Add(child.gameObject);
                    }
                    break;
                case 5:
                    foreach (Transform child in windowsRows0[i].transform)
                    {
                        Rows0Windows5.Add(child.gameObject);
                    }
                    break;
                case 6:
                    foreach (Transform child in windowsRows0[i].transform)
                    {
                        Rows0Windows6.Add(child.gameObject);
                    }
                    break;
                case 7:
                    foreach (Transform child in windowsRows0[i].transform)
                    {
                        Rows0Windows7.Add(child.gameObject);
                    }
                    break;
            }
        }
        _WindowGroup0Transform = new Transform[windowsRows0.Count, Rows0Windows0.Count];
        for (int x = 0; x < _WindowGroup0Transform.GetLength(0); x++)
        {
            for (int y = 0; y < _WindowGroup0Transform.GetLength(1); y++)
            {
                switch (x)
                {
                    case 0:
                        _WindowGroup0Transform[x, y] = Rows0Windows0[y].GetComponent<Transform>();
                        break;
                    case 1:
                        _WindowGroup0Transform[x, y] = Rows0Windows1[y].GetComponent<Transform>();
                        break;
                    case 2:
                        _WindowGroup0Transform[x, y] = Rows0Windows2[y].GetComponent<Transform>();
                        break;
                    case 3:
                        _WindowGroup0Transform[x, y] = Rows0Windows3[y].GetComponent<Transform>();
                        break;
                    case 4:
                        _WindowGroup0Transform[x, y] = Rows0Windows4[y].GetComponent<Transform>();
                        break;
                    case 5:
                        _WindowGroup0Transform[x, y] = Rows0Windows5[y].GetComponent<Transform>();
                        break;
                    case 6:
                        _WindowGroup0Transform[x, y] = Rows0Windows6[y].GetComponent<Transform>();
                        break;
                    case 7:
                        _WindowGroup0Transform[x, y] = Rows0Windows7[y].GetComponent<Transform>();
                        break;
                }
            }
        }

        List<GameObject> Rows1Windows0 = new List<GameObject>();
        List<GameObject> Rows1Windows1 = new List<GameObject>();
        List<GameObject> Rows1Windows2 = new List<GameObject>();
        List<GameObject> Rows1Windows3 = new List<GameObject>();
        List<GameObject> Rows1Windows4 = new List<GameObject>();
        List<GameObject> Rows1Windows5 = new List<GameObject>();
        List<GameObject> Rows1Windows6 = new List<GameObject>();
        List<GameObject> Rows1Windows7 = new List<GameObject>();
        for (int i = 0; i < windowsRows1.Count; i++)
        {
            switch (i)
            {
                case 0:
                    foreach (Transform child in windowsRows1[i].transform)
                    {
                        Rows1Windows0.Add(child.gameObject);
                    }
                    break;
                case 1:
                    foreach (Transform child in windowsRows1[i].transform)
                    {
                        Rows1Windows1.Add(child.gameObject);
                    }
                    break;
                case 2:
                    foreach (Transform child in windowsRows1[i].transform)
                    {
                        Rows1Windows2.Add(child.gameObject);
                    }
                    break;
                case 3:
                    foreach (Transform child in windowsRows1[i].transform)
                    {
                        Rows1Windows3.Add(child.gameObject);
                    }
                    break;
                case 4:
                    foreach (Transform child in windowsRows1[i].transform)
                    {
                        Rows1Windows4.Add(child.gameObject);
                    }
                    break;
                case 5:
                    foreach (Transform child in windowsRows1[i].transform)
                    {
                        Rows1Windows5.Add(child.gameObject);
                    }
                    break;
                case 6:
                    foreach (Transform child in windowsRows1[i].transform)
                    {
                        Rows1Windows6.Add(child.gameObject);
                    }
                    break;
                case 7:
                    foreach (Transform child in windowsRows1[i].transform)
                    {
                        Rows1Windows7.Add(child.gameObject);
                    }
                    break;
            }
        }
        _WindowGroup1Transform = new Transform[windowsRows1.Count, Rows1Windows0.Count];
        for (int x = 0; x < _WindowGroup1Transform.GetLength(0); x++)
        {
            for (int y = 0; y < _WindowGroup1Transform.GetLength(1); y++)
            {
                switch (x)
                {
                    case 0:
                        _WindowGroup1Transform[x, y] = Rows1Windows0[y].GetComponent<Transform>();
                        break;
                    case 1:
                        _WindowGroup1Transform[x, y] = Rows1Windows1[y].GetComponent<Transform>();
                        break;
                    case 2:
                        _WindowGroup1Transform[x, y] = Rows1Windows2[y].GetComponent<Transform>();
                        break;
                    case 3:
                        _WindowGroup1Transform[x, y] = Rows1Windows3[y].GetComponent<Transform>();
                        break;
                    case 4:
                        _WindowGroup1Transform[x, y] = Rows1Windows4[y].GetComponent<Transform>();
                        break;
                    case 5:
                        _WindowGroup1Transform[x, y] = Rows1Windows5[y].GetComponent<Transform>();
                        break;
                    case 6:
                        _WindowGroup1Transform[x, y] = Rows1Windows6[y].GetComponent<Transform>();
                        break;
                    case 7:
                        _WindowGroup1Transform[x, y] = Rows1Windows7[y].GetComponent<Transform>();
                        break;
                }
            }
        }

        List<GameObject> Rows2Windows0 = new List<GameObject>();
        List<GameObject> Rows2Windows1 = new List<GameObject>();
        List<GameObject> Rows2Windows2 = new List<GameObject>();
        List<GameObject> Rows2Windows3 = new List<GameObject>();
        List<GameObject> Rows2Windows4 = new List<GameObject>();
        List<GameObject> Rows2Windows5 = new List<GameObject>();
        List<GameObject> Rows2Windows6 = new List<GameObject>();
        List<GameObject> Rows2Windows7 = new List<GameObject>();
        for (int i = 0; i < windowsRows2.Count; i++)
        {
            switch (i)
            {
                case 0:
                    foreach (Transform child in windowsRows2[i].transform)
                    {
                        Rows2Windows0.Add(child.gameObject);
                    }
                    break;
                case 1:
                    foreach (Transform child in windowsRows2[i].transform)
                    {
                        Rows2Windows1.Add(child.gameObject);
                    }
                    break;
                case 2:
                    foreach (Transform child in windowsRows2[i].transform)
                    {
                        Rows2Windows2.Add(child.gameObject);
                    }
                    break;
                case 3:
                    foreach (Transform child in windowsRows2[i].transform)
                    {
                        Rows2Windows3.Add(child.gameObject);
                    }
                    break;
                case 4:
                    foreach (Transform child in windowsRows2[i].transform)
                    {
                        Rows2Windows4.Add(child.gameObject);
                    }
                    break;
                case 5:
                    foreach (Transform child in windowsRows2[i].transform)
                    {
                        Rows2Windows5.Add(child.gameObject);
                    }
                    break;
                case 6:
                    foreach (Transform child in windowsRows2[i].transform)
                    {
                        Rows2Windows6.Add(child.gameObject);
                    }
                    break;
                case 7:
                    foreach (Transform child in windowsRows2[i].transform)
                    {
                        Rows2Windows7.Add(child.gameObject);
                    }
                    break;
            }
        }
        _WindowGroup2Transform = new Transform[windowsRows2.Count, Rows2Windows0.Count];
        for (int x = 0; x < _WindowGroup2Transform.GetLength(0); x++)
        {
            for (int y = 0; y < _WindowGroup2Transform.GetLength(1); y++)
            {
                switch (x)
                {
                    case 0:
                        _WindowGroup2Transform[x, y] = Rows2Windows0[y].GetComponent<Transform>();
                        break;
                    case 1:
                        _WindowGroup2Transform[x, y] = Rows2Windows1[y].GetComponent<Transform>();
                        break;
                    case 2:
                        _WindowGroup2Transform[x, y] = Rows2Windows2[y].GetComponent<Transform>();
                        break;
                    case 3:
                        _WindowGroup2Transform[x, y] = Rows2Windows3[y].GetComponent<Transform>();
                        break;
                    case 4:
                        _WindowGroup2Transform[x, y] = Rows2Windows4[y].GetComponent<Transform>();
                        break;
                    case 5:
                        _WindowGroup2Transform[x, y] = Rows2Windows5[y].GetComponent<Transform>();
                        break;
                    case 6:
                        _WindowGroup2Transform[x, y] = Rows2Windows6[y].GetComponent<Transform>();
                        break;
                    case 7:
                        _WindowGroup2Transform[x, y] = Rows2Windows7[y].GetComponent<Transform>();
                        break;
                }
            }
        }

        List<GameObject> Rows3Windows0 = new List<GameObject>();
        List<GameObject> Rows3Windows1 = new List<GameObject>();
        List<GameObject> Rows3Windows2 = new List<GameObject>();
        List<GameObject> Rows3Windows3 = new List<GameObject>();
        List<GameObject> Rows3Windows4 = new List<GameObject>();
        List<GameObject> Rows3Windows5 = new List<GameObject>();
        List<GameObject> Rows3Windows6 = new List<GameObject>();
        List<GameObject> Rows3Windows7 = new List<GameObject>();
        for (int i = 0; i < windowsRows3.Count; i++)
        {
            switch (i)
            {
                case 0:
                    foreach (Transform child in windowsRows3[i].transform)
                    {
                        Rows3Windows0.Add(child.gameObject);
                    }
                    break;
                case 1:
                    foreach (Transform child in windowsRows3[i].transform)
                    {
                        Rows3Windows1.Add(child.gameObject);
                    }
                    break;
                case 2:
                    foreach (Transform child in windowsRows3[i].transform)
                    {
                        Rows3Windows2.Add(child.gameObject);
                    }
                    break;
                case 3:
                    foreach (Transform child in windowsRows3[i].transform)
                    {
                        Rows3Windows3.Add(child.gameObject);
                    }
                    break;
                case 4:
                    foreach (Transform child in windowsRows3[i].transform)
                    {
                        Rows3Windows4.Add(child.gameObject);
                    }
                    break;
                case 5:
                    foreach (Transform child in windowsRows3[i].transform)
                    {
                        Rows3Windows5.Add(child.gameObject);
                    }
                    break;
                case 6:
                    foreach (Transform child in windowsRows3[i].transform)
                    {
                        Rows3Windows6.Add(child.gameObject);
                    }
                    break;
                case 7:
                    foreach (Transform child in windowsRows3[i].transform)
                    {
                        Rows3Windows7.Add(child.gameObject);
                    }
                    break;
            }
        }
        _WindowGroup3Transform = new Transform[windowsRows3.Count, Rows3Windows0.Count];
        for (int x = 0; x < _WindowGroup3Transform.GetLength(0); x++)
        {
            for (int y = 0; y < _WindowGroup3Transform.GetLength(1); y++)
            {
                switch (x)
                {
                    case 0:
                        _WindowGroup3Transform[x, y] = Rows3Windows0[y].GetComponent<Transform>();
                        break;
                    case 1:
                        _WindowGroup3Transform[x, y] = Rows3Windows1[y].GetComponent<Transform>();
                        break;
                    case 2:
                        _WindowGroup3Transform[x, y] = Rows3Windows2[y].GetComponent<Transform>();
                        break;
                    case 3:
                        _WindowGroup3Transform[x, y] = Rows3Windows3[y].GetComponent<Transform>();
                        break;
                    case 4:
                        _WindowGroup3Transform[x, y] = Rows3Windows4[y].GetComponent<Transform>();
                        break;
                    case 5:
                        _WindowGroup3Transform[x, y] = Rows3Windows5[y].GetComponent<Transform>();
                        break;
                    case 6:
                        _WindowGroup3Transform[x, y] = Rows3Windows6[y].GetComponent<Transform>();
                        break;
                    case 7:
                        _WindowGroup3Transform[x, y] = Rows3Windows7[y].GetComponent<Transform>();
                        break;
                }
            }
        }
    }

    void CreateNewMesh()
    {
        _Vertices = new Vector3[_AllWindows.Count * 4];
        _Triangles = new int[_AllWindows.Count * 6];
        _VertexColors = new Color[_Vertices.Length]; 

        int v = 0;
        int t = 0;

        for (int i = 0; i < _AllWindows.Count; i++)
        {
            //populate the vertices and triangles arrays
            _Vertices[v] =     _AllWindows[i].position + _AllWindows[i].right * -VertexOffster.x + _AllWindows[i].up * -VertexOffster.y;
            _Vertices[v + 1] = _AllWindows[i].position + _AllWindows[i].right * -VertexOffster.x + _AllWindows[i].up *  VertexOffster.y;
            _Vertices[v + 2] = _AllWindows[i].position + _AllWindows[i].right *  VertexOffster.x + _AllWindows[i].up * -VertexOffster.y;
            _Vertices[v + 3] = _AllWindows[i].position + _AllWindows[i].right *  VertexOffster.x + _AllWindows[i].up *  VertexOffster.y;


            _Triangles[t] = v;
            _Triangles[t + 1] = _Triangles[t + 4] = v + 1;
            _Triangles[t + 2] = _Triangles[t + 3] = v + 2;
            _Triangles[t + 5] = v + 3;

            v += 4;
            t += 6;
        }

        _NewWindowsMesh.Clear();
        _NewWindowsMesh.vertices = _Vertices;
        _NewWindowsMesh.triangles = _Triangles;
        _NewWindowsMesh.colors = _VertexColors;
        _NewWindowsMesh.RecalculateNormals();

        GameObject Windows = new GameObject();
        Windows.AddComponent<MeshFilter>();
        Windows.GetComponent<MeshFilter>().mesh = _NewWindowsMesh;
        Windows.AddComponent<MeshRenderer>();
    }

    void FillAllWindowsArray()
    {
        _AllWindows = new List<Transform>();

        for (int y = 0; y < _WindowGroup0Transform.GetLength(1); y++)
        {
            for (int x = 0; x < _WindowGroup0Transform.GetLength(0); x++)
            {
                _AllWindows.Add(_WindowGroup0Transform[x, y]);
            }
        }

        for (int y = 0; y < _WindowGroup1Transform.GetLength(1); y++)
        {
            for (int x = 0; x < _WindowGroup1Transform.GetLength(0); x++)
            {
                _AllWindows.Add(_WindowGroup1Transform[x, y]);
            }
        }

        for (int y = 0; y < _WindowGroup2Transform.GetLength(1); y++)
        {
            for (int x = 0; x < _WindowGroup2Transform.GetLength(0); x++)
            {
                _AllWindows.Add(_WindowGroup2Transform[x, y]);
            }
        }

        for (int y = 0; y < _WindowGroup3Transform.GetLength(1); y++)
        {
            for (int x = 0; x < _WindowGroup3Transform.GetLength(0); x++)
            {
                _AllWindows.Add(_WindowGroup3Transform[x, y]);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawSphere(_Vertices[_Index],2);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            _Index++;
        if (Input.GetKeyDown(KeyCode.DownArrow))
            _Index--;
    }
}
