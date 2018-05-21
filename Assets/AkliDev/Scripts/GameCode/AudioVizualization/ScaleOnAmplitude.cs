using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleOnAmplitude : MonoBehaviour
{
    [SerializeField] private GetAudioSpectrum _AudioSpectrum;
    [SerializeField] private Vector3 _StartScale;
    [SerializeField] private float _MaxScale;
    [SerializeField] bool _UseBuffer;

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_UseBuffer)
        {
            transform.localScale = new Vector3((_AudioSpectrum._AudioBandBuffers[0] * _MaxScale) + _StartScale.x,
                                               (_AudioSpectrum._AudioBandBuffers[0] * _MaxScale) + _StartScale.y,
                                               (_AudioSpectrum._AudioBandBuffers[0] * _MaxScale) + _StartScale.z);
        }
        else
        {
            transform.localScale = new Vector3((_AudioSpectrum._Amplitude * _MaxScale) + _StartScale.x,
                                               (_AudioSpectrum._Amplitude * _MaxScale) + _StartScale.y,
                                               (_AudioSpectrum._Amplitude * _MaxScale) + _StartScale.z);
        }
    }
}
