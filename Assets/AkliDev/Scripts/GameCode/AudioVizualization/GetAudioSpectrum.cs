using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GetAudioSpectrum : MonoBehaviour
{
    public enum Channel
    {
        Stereo,
        Left,
        Right
    }

    [SerializeField] private Channel _Channel;
    AudioSource _AudioSource;

    private float[] _SamplesLeft;
    private float[] _SamplesRight;

    private float[] _FrequencyBands;
    private float[] _FrequencyBandBuffers;
    private float[] _BufferDecreses;
    private float[] _HighestFrequencyBands;

    private float[] _FrequencyBands64;
    private float[] _FrequencyBandBuffers64;
    private float[] _BufferDecreses64;
    private float[] _HighestFrequencyBands64;

    public float[] _AudioBands;
    public float[] _AudioBandBuffers;

    public float[] _AudioBands64;
    public float[] _AudioBandBuffers64;

    public float _Amplitude;
    public float _AmplitudeBuffer;

    private float _HighestAmplitude;

    [SerializeField] private float _AudioProfileFloat;
    [SerializeField] private float _AudioProfileFloat64;

    private void Awake()
    {
        _AudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _SamplesLeft = new float[512];
        _SamplesRight = new float[512];

        _FrequencyBands = new float[8];
        _FrequencyBandBuffers = new float[_FrequencyBands.Length];
        _BufferDecreses = new float[_FrequencyBandBuffers.Length];
        _HighestFrequencyBands = new float[_FrequencyBandBuffers.Length];
        _AudioBands = new float[_FrequencyBandBuffers.Length];
        _AudioBandBuffers = new float[_FrequencyBandBuffers.Length];

        _FrequencyBands64 = new float[64];
        _FrequencyBandBuffers64 = new float[_FrequencyBands64.Length];
        _BufferDecreses64 = new float[_FrequencyBandBuffers64.Length];
        _HighestFrequencyBands64 = new float[_FrequencyBandBuffers64.Length];
        _AudioBands64 = new float[_FrequencyBandBuffers64.Length];
        _AudioBandBuffers64 = new float[_FrequencyBandBuffers64.Length];

        _Amplitude = 0;
        _AmplitudeBuffer = 0;
        _HighestAmplitude = 0;

        AudioProfile(_AudioProfileFloat);
        AudioProfile64(_AudioProfileFloat64);
    }
    void AudioProfile(float audioProfile)
    {
        for (int i = 0; i < _FrequencyBands.Length; i++)
        {
            _HighestFrequencyBands[i] = audioProfile;
        }
    }
    void AudioProfile64(float audioProfile64)
    {
        for (int i = 0; i < _FrequencyBands64.Length; i++)
        {
            _HighestFrequencyBands64[i] = audioProfile64;
        }
    }
    private void Update()
    {
        SetSpectrumAudioSource();
        MakeFrequencyBands();
        MakeFrequencyBands64();
        MakeBandBuffer();
        MakeBandBuffer64();
        CreateAudioBands();
        CreateAudioBands64();
        GetAmplitude();
    }

    private void SetSpectrumAudioSource()
    {
        _AudioSource.GetSpectrumData(_SamplesLeft, 0, FFTWindow.Rectangular);
        _AudioSource.GetSpectrumData(_SamplesRight, 1, FFTWindow.Rectangular);
    }
    private void MakeFrequencyBands()
    {
        int count = 0;

        for (int i = 0; i < _FrequencyBands.Length; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;

            if (i == 7)
            {
                sampleCount += 2;
            }
            for (int u = 0; u < sampleCount; u++)
            {
                switch (_Channel)
                {
                    case Channel.Stereo:
                        average += (_SamplesLeft[count] + _SamplesRight[count]) * (count + 1);
                        break;
                    case Channel.Left:
                        average += _SamplesLeft[count] * (count + 1);
                        break;
                    case Channel.Right:
                        average += _SamplesRight[count] * (count + 1);
                        break;
                }

                count++;
            }
            average /= count;
            _FrequencyBands[i] = average * 10;
        }
    }
    private void MakeFrequencyBands64()
    {
        int count = 0;
        int sampleCount = 1;
        int power = 0;

        for (int i = 0; i < _FrequencyBands64.Length; i++)
        {
            float average = 0;

            if (i == 16 || i == 32 || i == 40 || i == 48 || i == 56)
            {
                power++;
                sampleCount = (int)Mathf.Pow(2, power);
                if (power == 3)
                {
                    sampleCount -= 2;
                }
            }

            for (int u = 0; u < sampleCount; u++)
            {
                switch (_Channel)
                {
                    case Channel.Stereo:
                        average += (_SamplesLeft[count] + _SamplesRight[count]) * (count + 1);
                        break;
                    case Channel.Left:
                        average += _SamplesLeft[count] * (count + 1);
                        break;
                    case Channel.Right:
                        average += _SamplesRight[count] * (count + 1);
                        break;
                }

                count++;
            }
            average /= count;
            _FrequencyBands64[i] = average * 80;
        }
    }
    private void MakeBandBuffer()
    {
        for (int i = 0; i < _FrequencyBands.Length; i++)
        {
            if (_FrequencyBands[i] > _FrequencyBandBuffers[i])
            {
                _FrequencyBandBuffers[i] = _FrequencyBands[i];
                _BufferDecreses[i] = 0.005f;
            }
            if (_FrequencyBands[i] < _FrequencyBandBuffers[i])
            {
                _BufferDecreses[i] = (_FrequencyBandBuffers[i] - _FrequencyBands[i]) / 8;
                _FrequencyBandBuffers[i] -= _BufferDecreses[i];
            }
        }
    }
    private void MakeBandBuffer64()
    {
        for (int i = 0; i < _FrequencyBands64.Length; i++)
        {
            if (_FrequencyBands64[i] > _FrequencyBandBuffers64[i])
            {
                _FrequencyBandBuffers64[i] = _FrequencyBands64[i];
                _BufferDecreses64[i] = 0.005f;
            }
            if (_FrequencyBands64[i] < _FrequencyBandBuffers64[i])
            {
                _BufferDecreses64[i] = (_FrequencyBandBuffers64[i] - _FrequencyBands64[i]) / 8;
                _FrequencyBandBuffers64[i] -= _BufferDecreses64[i];
            }
        }
    }
    private void CreateAudioBands()
    {
        for (int i = 0; i < _FrequencyBands.Length; i++)
        {
            if (_FrequencyBands[i] > _HighestFrequencyBands[i])
            {
                _HighestFrequencyBands[i] = _FrequencyBands[i];
            }

            if (_FrequencyBands[i] != 0 || _HighestFrequencyBands[i] != 0)
            {
                _AudioBands[i] = _FrequencyBands[i] / _HighestFrequencyBands[i];
            }
            if (_FrequencyBandBuffers[i] != 0 || _HighestFrequencyBands[i] != 0)
            {
                _AudioBandBuffers[i] = _FrequencyBandBuffers[i] / _HighestFrequencyBands[i];
            }
        }
    }
    private void CreateAudioBands64()
    {
        for (int i = 0; i < _FrequencyBands64.Length; i++)
        {
            if (_FrequencyBands64[i] > _HighestFrequencyBands64[i])
            {
                _HighestFrequencyBands64[i] = _FrequencyBands64[i];
            }

            if (_FrequencyBands64[i] != 0 || _HighestFrequencyBands64[i] != 0)
            {
                _AudioBands64[i] = _FrequencyBands64[i] / _HighestFrequencyBands64[i];
            }
            if (_FrequencyBandBuffers64[i] != 0 || _HighestFrequencyBands64[i] != 0)
            {
                _AudioBandBuffers64[i] = _FrequencyBandBuffers64[i] / _HighestFrequencyBands64[i];
            }
        }
    }
    private void GetAmplitude()
    {
        float currentAplitude = 0;
        float currentAplitudeBuffer = 0;
        for (int i = 0; i < _FrequencyBands.Length; i++)
        {
            currentAplitude += _AudioBands[i];
            currentAplitudeBuffer += _AudioBandBuffers[i];
        }
        if (currentAplitude > _HighestAmplitude)
        {
            _HighestAmplitude = currentAplitude;
        }

        if (currentAplitude != 0 || _HighestAmplitude != 0)
        {
            _Amplitude = currentAplitude / _HighestAmplitude;
        }
        if (currentAplitudeBuffer != 0 || _HighestAmplitude != 0)
        {
            _AmplitudeBuffer = currentAplitudeBuffer / _HighestAmplitude;
        }


    }
}
