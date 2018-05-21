using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private BuildingManagerOld _Manager;
    [SerializeField] private BuildingColor _TowerColor;
    [SerializeField] private Frequency _BandsFrequency;
    [SerializeField] private Frequency _WindowGroup0Frequency;
    [SerializeField] private Frequency _WindowGroup1Frequency;
    [SerializeField] private Frequency _WindowGroup2Frequency;
    [SerializeField] private Frequency _WindowGroup3Frequency;

    private List<Renderer> _BandRenderers;
    private Renderer[,] _WindowGroup0Renderer;
    private Renderer[,] _WindowGroup1Renderer;
    private Renderer[,] _WindowGroup2Renderer;
    private Renderer[,] _WindowGroup3Renderer;

    void Awake()
    {
        GetBackBands();
        GetWindows();

        _Manager.AddTowerBandsToList(_BandRenderers);
        _Manager.AddTowerBandFrequencyToList(_BandsFrequency);

        _Manager.AddTowerWindowsToList(_WindowGroup0Renderer);
        _Manager.AddTowerWindowFrequencyToList(_WindowGroup0Frequency);
        _Manager.AddTowerColorToList(_TowerColor);

        _Manager.AddTowerWindowsToList(_WindowGroup1Renderer);
        _Manager.AddTowerWindowFrequencyToList(_WindowGroup1Frequency);
        _Manager.AddTowerColorToList(_TowerColor);

        _Manager.AddTowerWindowsToList(_WindowGroup2Renderer);
        _Manager.AddTowerWindowFrequencyToList(_WindowGroup2Frequency);
        _Manager.AddTowerColorToList(_TowerColor);

        _Manager.AddTowerWindowsToList(_WindowGroup3Renderer);
        _Manager.AddTowerWindowFrequencyToList(_WindowGroup3Frequency);
        _Manager.AddTowerColorToList(_TowerColor);

        print(_WindowGroup0Renderer.GetLength(0));  print(_WindowGroup0Renderer.GetLength(1));

    }

    void GetBackBands()
    {
        _BandRenderers = new List<Renderer>();
        GameObject backRow = transform.GetChild(0).gameObject;

        foreach (Transform child in backRow.transform)
        {
            _BandRenderers.Add(child.GetComponent<Renderer>());
            child.GetComponent<Renderer>().material = _Manager.TowerColors[(int)_TowerColor];
        }
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
        _WindowGroup0Renderer = new Renderer[windowsRows0.Count, Rows0Windows0.Count];
        for (int x = 0; x < _WindowGroup0Renderer.GetLength(0); x++)
        {
            for (int y = 0; y < _WindowGroup0Renderer.GetLength(1); y++)
            {
                switch (x)
                {
                    case 0:
                        _WindowGroup0Renderer[x, y] = Rows0Windows0[y].GetComponent<Renderer>();
                        break;
                    case 1:
                        _WindowGroup0Renderer[x, y] = Rows0Windows1[y].GetComponent<Renderer>();
                        break;
                    case 2:
                        _WindowGroup0Renderer[x, y] = Rows0Windows2[y].GetComponent<Renderer>();
                        break;
                    case 3:
                        _WindowGroup0Renderer[x, y] = Rows0Windows3[y].GetComponent<Renderer>();
                        break;
                    case 4:
                        _WindowGroup0Renderer[x, y] = Rows0Windows4[y].GetComponent<Renderer>();
                        break;
                    case 5:
                        _WindowGroup0Renderer[x, y] = Rows0Windows5[y].GetComponent<Renderer>();
                        break;
                    case 6:
                        _WindowGroup0Renderer[x, y] = Rows0Windows6[y].GetComponent<Renderer>();
                        break;
                    case 7:
                        _WindowGroup0Renderer[x, y] = Rows0Windows7[y].GetComponent<Renderer>();
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
        _WindowGroup1Renderer = new Renderer[windowsRows1.Count, Rows1Windows0.Count];
        for (int x = 0; x < _WindowGroup1Renderer.GetLength(0); x++)
        {
            for (int y = 0; y < _WindowGroup1Renderer.GetLength(1); y++)
            {
                switch (x)
                {
                    case 0:
                        _WindowGroup1Renderer[x, y] = Rows1Windows0[y].GetComponent<Renderer>();
                        break;
                    case 1:
                        _WindowGroup1Renderer[x, y] = Rows1Windows1[y].GetComponent<Renderer>();
                        break;
                    case 2:
                        _WindowGroup1Renderer[x, y] = Rows1Windows2[y].GetComponent<Renderer>();
                        break;
                    case 3:
                        _WindowGroup1Renderer[x, y] = Rows1Windows3[y].GetComponent<Renderer>();
                        break;
                    case 4:
                        _WindowGroup1Renderer[x, y] = Rows1Windows4[y].GetComponent<Renderer>();
                        break;
                    case 5:
                        _WindowGroup1Renderer[x, y] = Rows1Windows5[y].GetComponent<Renderer>();
                        break;
                    case 6:
                        _WindowGroup1Renderer[x, y] = Rows1Windows6[y].GetComponent<Renderer>();
                        break;
                    case 7:
                        _WindowGroup1Renderer[x, y] = Rows1Windows7[y].GetComponent<Renderer>();
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
        _WindowGroup2Renderer = new Renderer[windowsRows2.Count, Rows2Windows0.Count];
        for (int x = 0; x < _WindowGroup2Renderer.GetLength(0); x++)
        {
            for (int y = 0; y < _WindowGroup2Renderer.GetLength(1); y++)
            {
                switch (x)
                {
                    case 0:
                        _WindowGroup2Renderer[x, y] = Rows2Windows0[y].GetComponent<Renderer>();
                        break;
                    case 1:
                        _WindowGroup2Renderer[x, y] = Rows2Windows1[y].GetComponent<Renderer>();
                        break;
                    case 2:
                        _WindowGroup2Renderer[x, y] = Rows2Windows2[y].GetComponent<Renderer>();
                        break;
                    case 3:
                        _WindowGroup2Renderer[x, y] = Rows2Windows3[y].GetComponent<Renderer>();
                        break;
                    case 4:
                        _WindowGroup2Renderer[x, y] = Rows2Windows4[y].GetComponent<Renderer>();
                        break;
                    case 5:
                        _WindowGroup2Renderer[x, y] = Rows2Windows5[y].GetComponent<Renderer>();
                        break;
                    case 6:
                        _WindowGroup2Renderer[x, y] = Rows2Windows6[y].GetComponent<Renderer>();
                        break;
                    case 7:
                        _WindowGroup2Renderer[x, y] = Rows2Windows7[y].GetComponent<Renderer>();
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
        _WindowGroup3Renderer = new Renderer[windowsRows3.Count, Rows3Windows0.Count];
        for (int x = 0; x < _WindowGroup3Renderer.GetLength(0); x++)
        {
            for (int y = 0; y < _WindowGroup3Renderer.GetLength(1); y++)
            {
                switch (x)
                {
                    case 0:
                        _WindowGroup3Renderer[x, y] = Rows3Windows0[y].GetComponent<Renderer>();
                        break;
                    case 1:
                        _WindowGroup3Renderer[x, y] = Rows3Windows1[y].GetComponent<Renderer>();
                        break;
                    case 2:
                        _WindowGroup3Renderer[x, y] = Rows3Windows2[y].GetComponent<Renderer>();
                        break;
                    case 3:
                        _WindowGroup3Renderer[x, y] = Rows3Windows3[y].GetComponent<Renderer>();
                        break;
                    case 4:
                        _WindowGroup3Renderer[x, y] = Rows3Windows4[y].GetComponent<Renderer>();
                        break;
                    case 5:
                        _WindowGroup3Renderer[x, y] = Rows3Windows5[y].GetComponent<Renderer>();
                        break;
                    case 6:
                        _WindowGroup3Renderer[x, y] = Rows3Windows6[y].GetComponent<Renderer>();
                        break;
                    case 7:
                        _WindowGroup3Renderer[x, y] = Rows3Windows7[y].GetComponent<Renderer>();
                        break;
                }
            }
        }
    }
}
