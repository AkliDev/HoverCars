using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamBuildBar : MonoBehaviour
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
    [SerializeField] private float _MaxScale;
    private float _AudioBand;
    private Vector3 _StartScale;

    void Start()
    {
        _StartScale = transform.lossyScale;
        GetComponent<Renderer>().material = _Mat;
    }

    void Update()
    {
        SetAudioBand();
        SetScale();
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

    private void SetScale()
    {
        transform.localScale = new Vector3(transform.localScale.x, (_AudioBand * _Multiplier) + _StartScale.y, transform.localScale.z);


        if (transform.localScale.y > _MaxScale)
        {
            transform.localScale = new Vector3(transform.localScale.x, _MaxScale, transform.localScale.z);
        }
    }
}
