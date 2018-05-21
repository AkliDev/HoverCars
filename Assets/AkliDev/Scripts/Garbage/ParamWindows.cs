using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamWindows : MonoBehaviour
{
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
    public enum BandIndex
    {
        One = 1,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Aight,
    }
    [SerializeField] private Material _Mat;
    [SerializeField] private GetAudioSpectrum _AudioSpectrum;
    [SerializeField] private bool _UseBuffer;
    [SerializeField] private Frequency _Frequency;
    [SerializeField] private BandIndex _BandIndex;
    [SerializeField] private float _Multiplier;
    private float _AudioBand;
    private float _DevideAmount;
    private List<GameObject> _Windows;

    void Start()
    {
        _Windows = new List<GameObject>();
        GetChildren();

        _DevideAmount = (float)1 / _Windows.Count;

    }
    void GetChildren()
    {
        int children = transform.childCount;
        for (int i = 0; i < children; ++i)
        {
            _Windows.Add(transform.GetChild(i).gameObject);
            _Windows[i].GetComponent<Renderer>().material = _Mat;
        }


    }
    void Update()
    {

        SetAudioBand();
        SetWindows();
    }

    private void SetAudioBand()
    {

        if (_UseBuffer)
        {
            if (_Frequency == Frequency.All)
            {
                _AudioBand = _AudioSpectrum._AudioBandBuffers[(int)_BandIndex - 1] * _Multiplier;
            }
            else
            {
                _AudioBand = _AudioSpectrum._AudioBandBuffers64[(int)_BandIndex - 1 + (int)_Frequency * 8] * _Multiplier;
            }
        }
        else
        {
            if (_Frequency == Frequency.All)
            {
                _AudioBand = _AudioSpectrum._AudioBands[(int)_BandIndex - 1] * _Multiplier;
            }
            else
            {
                _AudioBand = _AudioSpectrum._AudioBands64[(int)_BandIndex - 1 + (int)_Frequency * 8] * _Multiplier;
            }
        }
    }
    private void SetWindows()
    {
        int litAmount = 0;

        for (int i = 0; i < _Windows.Count; i++)
        {
            if (_AudioBand > _DevideAmount * i)
            {
                litAmount++;
            }
        }

        for (int i = 0; i < _Windows.Count; i++)
        {
            _Windows[i].SetActive(false);
        }

        for (int u = 0; u < litAmount; u++)
        {
            _Windows[u].SetActive(true);
        }
    }
}
