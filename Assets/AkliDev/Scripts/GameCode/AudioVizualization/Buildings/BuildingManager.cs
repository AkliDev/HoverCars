using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum Frequency
{
    SubBass,
    Bass,
    LowMidrange,
    Midrange,
    UpperMidrange,
    Presence,
    Brilliance,
    Above,
    All
}

public enum BuildingColor
{
    Black,
    Cyan,
    Green,
    Orange,
    Pink,
    Red,
    Yellow
}

public class BuildingManager : MonoBehaviour
{   
    [SerializeField] private GetAudioSpectrum _AudioSpectrum;

    private List<WindowVertexIndices[][,]> _AllTowerWindowVertexIndices;
    private List<Frequency[]> _AllTowerWindowGroupFrequencys;
    private List<int[]> _AllTowerWindowGroupMultipliers;
    private List<Renderer> _AllTowerWindowRenderers;
    private List<Mesh> _AllTowerWindowMeshes;
    private List<Color[]> _AllTowerWindowVertexColors;
    private List<Color> _AllTowerWindowColors;

    private WindowVertexIndices[][][,] _AllTowerWindowVertexIndicesA;
    private Frequency[][] _AllTowerWindowGroupFrequencysA;
    private int[][] _AllTowerWindowGroupMultipliersA;
    private Renderer[] _AllTowerWindowRenderersA;
    private Mesh[] _AllTowerWindowMeshesA;
    private Color[][] _AllTowerWindowVertexColorsA;
    private Color[] _AllTowerWindowColorsA;

    private float _TowerWindowDevideAmount;

    void Start()
    {
        ConvertListsToArrays();
        _TowerWindowDevideAmount = (float)1 / _AllTowerWindowVertexIndicesA[0][0].GetLength(1);
    }

    private void ConvertListsToArrays()
    {
        _AllTowerWindowVertexIndicesA = _AllTowerWindowVertexIndices.ToArray();
        _AllTowerWindowGroupFrequencysA = _AllTowerWindowGroupFrequencys.ToArray();
        _AllTowerWindowGroupMultipliersA = _AllTowerWindowGroupMultipliers.ToArray();
        _AllTowerWindowRenderersA = _AllTowerWindowRenderers.ToArray();
        _AllTowerWindowMeshesA = _AllTowerWindowMeshes.ToArray();
        _AllTowerWindowVertexColorsA = _AllTowerWindowVertexColors.ToArray();
        _AllTowerWindowColorsA = _AllTowerWindowColors.ToArray();

        _AllTowerWindowVertexIndices = null;
        _AllTowerWindowGroupFrequencys = null;
        _AllTowerWindowGroupMultipliers = null;
        _AllTowerWindowRenderers = null;
        _AllTowerWindowMeshes = null;
        _AllTowerWindowVertexColors = null;
        _AllTowerWindowColors = null;
    }
    public void AddTowerWindowData(WindowVertexIndices[][,] windowVertexGroups, Frequency[] windowGroupFrequencys,int[] windowGroupMultipliers, Renderer renderer, Mesh mesh, Color[] vertexColors, Color windowColor) 
    {
        if (_AllTowerWindowVertexIndices == null) { _AllTowerWindowVertexIndices = new List<WindowVertexIndices[][,]>(); }
        if (_AllTowerWindowGroupFrequencys == null) { _AllTowerWindowGroupFrequencys = new List<Frequency[]>(); }
        if (_AllTowerWindowGroupMultipliers == null) { _AllTowerWindowGroupMultipliers = new List<int[]>(); }
        if (_AllTowerWindowRenderers == null) { _AllTowerWindowRenderers = new List<Renderer>(); }
        if (_AllTowerWindowMeshes == null) { _AllTowerWindowMeshes = new List<Mesh>(); }
        if (_AllTowerWindowVertexColors == null) { _AllTowerWindowVertexColors = new List<Color[]>(); }
        if (_AllTowerWindowColors == null) { _AllTowerWindowColors = new List<Color>(); }

        _AllTowerWindowVertexIndices.Add(windowVertexGroups);
        _AllTowerWindowGroupFrequencys.Add(windowGroupFrequencys);
        _AllTowerWindowGroupMultipliers.Add(windowGroupMultipliers);
        _AllTowerWindowRenderers.Add(renderer);
        _AllTowerWindowMeshes.Add(mesh);
        _AllTowerWindowVertexColors.Add(vertexColors);
        _AllTowerWindowColors.Add(windowColor);
    }

    //public void AddTowerWindowData(TowerWindowData towerWindowData)
    //{
    //    if (_TowerWindowData == null)
    //    {
    //        _TowerWindowData = new List<TowerWindowData>();
    //    }

    //    _TowerWindowData.Add(towerWindowData);
    //}

    private void Update()
    {
        //ScaleTowerBand();
        TurnTowerWindowsOnOff();
    }
    private void TurnTowerWindowsOnOff()
    {
        float devideAmount = 0;

        for (int i = 0; i < _AllTowerWindowVertexIndicesA.Length; i++)
        {
            if (_AllTowerWindowRenderersA[i].isVisible)
            {
                for (int k = 0; k < _AllTowerWindowVertexIndicesA[i].Length; k++)
                {
                    for (int x = 0; x < _AllTowerWindowVertexIndicesA[i][k].GetLength(0); x++)
                    {
                        if (_AllTowerWindowGroupFrequencysA[i][k] == Frequency.All)
                        {
                            
                            devideAmount = _AllTowerWindowGroupMultipliersA[i][k] * _AudioSpectrum._AudioBandBuffers[x] / _TowerWindowDevideAmount;
                        }
                        else
                        {
                            devideAmount = _AllTowerWindowGroupMultipliersA[i][k] * _AudioSpectrum._AudioBandBuffers64[((int)_AllTowerWindowGroupFrequencysA[i][k] * 8) + x] / _TowerWindowDevideAmount;
                        }

                        if (devideAmount < 0)
                        {
                            devideAmount = 0;
                        }
                        if (devideAmount > _AllTowerWindowVertexIndicesA[i][k].GetLength(1))
                        {
                            devideAmount = _AllTowerWindowVertexIndicesA[i][k].GetLength(1);
                        }

                        for (int j = 0; j < devideAmount; j++)// turn on
                        {
                            _AllTowerWindowVertexColorsA[i][_AllTowerWindowVertexIndicesA[i][k][x, j].vertex0] = _AllTowerWindowColorsA[i];
                            _AllTowerWindowVertexColorsA[i][_AllTowerWindowVertexIndicesA[i][k][x, j].vertex1] = _AllTowerWindowColorsA[i];
                            _AllTowerWindowVertexColorsA[i][_AllTowerWindowVertexIndicesA[i][k][x, j].vertex2] = _AllTowerWindowColorsA[i];
                            _AllTowerWindowVertexColorsA[i][_AllTowerWindowVertexIndicesA[i][k][x, j].vertex3] = _AllTowerWindowColorsA[i];
                        }

                        for (int j = _AllTowerWindowVertexIndicesA[i][k].GetLength(1) - 1; j > devideAmount - 1; j--)
                        {
                            _AllTowerWindowVertexColorsA[i][_AllTowerWindowVertexIndicesA[i][k][x, j].vertex0] = new Color();
                            _AllTowerWindowVertexColorsA[i][_AllTowerWindowVertexIndicesA[i][k][x, j].vertex1] = new Color();
                            _AllTowerWindowVertexColorsA[i][_AllTowerWindowVertexIndicesA[i][k][x, j].vertex2] = new Color();
                            _AllTowerWindowVertexColorsA[i][_AllTowerWindowVertexIndicesA[i][k][x, j].vertex3] = new Color();
                        }
                    }
                }
                _AllTowerWindowMeshesA[i].colors = _AllTowerWindowVertexColorsA[i];
            }
        }
    }

 

    //private void ScaleTowerBand()
    //{

    //    for (int i = 0; i < _AllTowerBands.Count; i++)
    //    {
    //        for (int j = 0; j < _AllTowerBands[i].Count; j++)
    //        {
    //            if (_AllTowerBands[i][j].isVisible)
    //            {
    //                if (_AllTowerBandFrequencys[i] == Frequency.All)
    //                {
    //                    _AllTowerBands[i][j].transform.localScale = new Vector3(_AllTowerBands[i][j].transform.localScale.x, 1 + _AudioSpectrum._AudioBandBuffers[j] * 20, _AllTowerBands[i][j].transform.localScale.z);
    //                }
    //                else
    //                {
    //                    _AllTowerBands[i][j].transform.localScale = new Vector3(_AllTowerBands[i][j].transform.localScale.x, 1 + _AudioSpectrum._AudioBandBuffers64[((int)_AllTowerBandFrequencys[i] * 8) + j] * 50, _AllTowerBands[i][j].transform.localScale.z);
    //                }
    //                if (_AllTowerBands[i][j].transform.localScale.y > 25.5f)
    //                {
    //                    _AllTowerBands[i][j].transform.localScale = new Vector3(_AllTowerBands[i][j].transform.localScale.x, 25.5f, _AllTowerBands[i][j].transform.localScale.z);
    //                }
    //            }
    //        }
    //    }
    //}
}

