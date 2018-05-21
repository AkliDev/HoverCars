using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeParam : MonoBehaviour
{
    [SerializeField] private GetAudioSpectrum _AudioSpectrum;
    [SerializeField] private bool _UseBuffer;
    public int _Band;
    public float _StartScale, _ScaleMultiplier, MaxScale;
   
    

    private Material _Mat;
    void Awake()
    {
        _Mat = GetComponent<Material>();
    }

    void Update()
    {
        if (_UseBuffer)
        {
            transform.localScale = new Vector3(transform.localScale.x, (_AudioSpectrum._AudioBandBuffers[_Band] * _ScaleMultiplier) + _StartScale, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, (_AudioSpectrum._AudioBands[_Band] * _ScaleMultiplier) + _StartScale, transform.localScale.z);
        }

        if (transform.localScale.y > MaxScale)
        {
            transform.localScale = new Vector3(transform.localScale.x, MaxScale, transform.localScale.z);
        }

    }
}
