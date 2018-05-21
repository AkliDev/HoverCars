using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BuildingManagerOld : MonoBehaviour
{
    [Range(0, 1)] [SerializeField] float t;
    [SerializeField] private GetAudioSpectrum _AudioSpectrum;

    [SerializeField] private GameObject _TowerWindow;
    [SerializeField] private GameObject _TowerBackBand;

    [SerializeField] Material[] _TowerColors;

    private List<List<Renderer>> _AllTowerBands;
    private List<Renderer[,]> _TowerWindows;
    private bool[][,] _TowerWindowsStatus;
    private List<BuildingColor> _WindowGroupColors;

    private List<Frequency> _AllTowerBandFrequencys;
    private List<Frequency> _AllTowerWindowFrequencys;

    private float _TowerWindowDevideAmount;

    public GameObject TowerWindow { get { return _TowerWindow; } }
    public GameObject TowerBackBand { get { return _TowerBackBand; } }
    public Material[] TowerColors { get { return _TowerColors; } }

    void Start()
    {
        _TowerWindowDevideAmount = (float)1 / _TowerWindows[0].GetLength(1);
        _TowerWindowsStatus = new bool[_TowerWindows.Count][,];
        for (int i = 0; i < _TowerWindowsStatus.Length; i++)
        {
            _TowerWindowsStatus[i] = new bool[_TowerWindows[i].GetLength(0), _TowerWindows[i].GetLength(1)];
        }     
    }

    public void AddTowerBandsToList(List<Renderer> towerBands)
    {
        if (_AllTowerBands == null)
        {
            _AllTowerBands = new List<List<Renderer>>();
        }
        _AllTowerBands.Add(towerBands);
    }
    public void AddTowerWindowsToList(Renderer[,] towerWindows)
    {
        if (_TowerWindows == null)
        {
            _TowerWindows = new List<Renderer[,]>();
        }
        _TowerWindows.Add(towerWindows);
    }
    public void AddTowerColorToList(BuildingColor windowGroupColor)
    {
        if (_WindowGroupColors == null)
        {
            _WindowGroupColors = new List<BuildingColor>();
        }
        _WindowGroupColors.Add(windowGroupColor);
    }
    public void AddTowerBandFrequencyToList(Frequency frequency)
    {

        if (_WindowGroupColors == null)
        {
            _AllTowerBandFrequencys = new List<Frequency>();
        }
        _AllTowerBandFrequencys.Add(frequency);
    }
    public void AddTowerWindowFrequencyToList(Frequency frequency)
    {
        if (_WindowGroupColors == null)
        {
            _AllTowerWindowFrequencys = new List<Frequency>();
        }
        _AllTowerWindowFrequencys.Add(frequency);
    }

    private void Update()
    {
        ScaleTowerBand();
        TurnTowerWindowsOnOff();
    }

    private void TurnTowerWindowsOnOff()
    {
        float devideAmount = 0;
       
        for (int i = 0; i < _TowerWindows.Count; i++)
        {
            for (int x = 0; x < _TowerWindows[i].GetLength(0); x++)
            {
                if (_AllTowerWindowFrequencys[i] == Frequency.All)
                {
                   devideAmount = _AudioSpectrum._AudioBandBuffers[x] / _TowerWindowDevideAmount;
                }
                else
                {
                    devideAmount = 2 * _AudioSpectrum._AudioBandBuffers64[((int)_AllTowerWindowFrequencys[i] * 8) + x] / _TowerWindowDevideAmount;
                }
                if (devideAmount > _TowerWindows[i].GetLength(1))
                {
                    devideAmount = _TowerWindows[i].GetLength(1);
                }
                for (int j = 0; j < devideAmount; j++)// turn on
                {
                    if (_TowerWindows[i][x, j].isVisible)
                    {
                        if (_TowerWindowsStatus[i][x, j] == false)
                        {
                            //_TowerWindows[i][x, j].enabled = true;
                            _TowerWindows[i][x, j].material = TowerColors[(int)_WindowGroupColors[i]];
                            _TowerWindowsStatus[i][x, j] = true;
                        }
                    }
                }
                for (int j = _TowerWindows[i].GetLength(1) - 1; j > devideAmount - 1; j--)// turn off
                {
                    if (_TowerWindows[i][x, j].isVisible)
                    {
                        if (_TowerWindowsStatus[i][x, j] == true)
                        {
                            //_TowerWindows[i][x, j].enabled = false;
                            _TowerWindows[i][x, j].material = TowerColors[0];
                            _TowerWindowsStatus[i][x, j] = false;
                        }
                    }
                }
            }
           
        }
    }

    private void ScaleTowerBand()
    {

        for (int i = 0; i < _AllTowerBands.Count; i++)
        {
            for (int j = 0; j < _AllTowerBands[i].Count; j++)
            {
                if (_AllTowerBands[i][j].isVisible)
                {
                    if (_AllTowerBandFrequencys[i] == Frequency.All)
                    {
                        _AllTowerBands[i][j].transform.localScale = new Vector3(_AllTowerBands[i][j].transform.localScale.x, 1 + _AudioSpectrum._AudioBandBuffers[j] * 20, _AllTowerBands[i][j].transform.localScale.z);
                    }
                    else
                    {
                        _AllTowerBands[i][j].transform.localScale = new Vector3(_AllTowerBands[i][j].transform.localScale.x, 1 + _AudioSpectrum._AudioBandBuffers64[((int)_AllTowerBandFrequencys[i] * 8) + j] * 50, _AllTowerBands[i][j].transform.localScale.z);
                    }
                    if (_AllTowerBands[i][j].transform.localScale.y > 25.5f)
                    {
                        _AllTowerBands[i][j].transform.localScale = new Vector3(_AllTowerBands[i][j].transform.localScale.x, 25.5f, _AllTowerBands[i][j].transform.localScale.z);
                    }
                }
            }
        }
    }
}
