using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAudioHandler : MonoBehaviour
{
    [SerializeField] private GameObject _CarSounds;
    private AudioSource[] _Sounds;

    [SerializeField] private float _EngineMinVol = 0.3f;         //The minimum volume of the engine
    [SerializeField] private float _EngineMaxVol = 0.6f;        //The maximum volume of the engine
    [SerializeField] private float _EngineMinPitch = 0.2f;      //The minimum pitch of the engine
    [SerializeField] private float _EngineMaxPitch = 1.2f;		//The maximum pitch of the engine

    public AudioSource[] Sounds { get { return _Sounds; } }
    public float EngineMinVol { get { return _EngineMinVol; } }
    public float EngineMaxVol { get { return _EngineMaxVol; } }
    public float EngineMinPitch { get { return _EngineMinPitch; } }
    public float EngineMaxPitch { get { return _EngineMaxPitch; } }

    public void SetEngineMinVol(float newEngineMinVol)
    {
        _EngineMinVol = newEngineMinVol;
    }
    public void SetEngineMaxVol(float newEngineMaxVol)
    {
        _EngineMaxVol = newEngineMaxVol;
    }
    public void SetEngineMinPitch(float newEngineMinPitch)
    {
        _EngineMinPitch = newEngineMinPitch;
    }
    public void SetEngineMaxPitch(float newEngineMaxPitch)
    {
        _EngineMaxPitch = newEngineMaxPitch;
    }

    void Start()
    {
        _Sounds = _CarSounds.GetComponents<AudioSource>();
    }

}
