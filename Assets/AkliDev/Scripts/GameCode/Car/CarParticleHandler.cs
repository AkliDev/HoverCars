using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarParticleHandler : MonoBehaviour
{
    private CarManager _CarManager;
    [SerializeField] private Transform[] _Particles;
    [SerializeField] private float _SparkThreshold, _Tilt;
    private bool _IsSparking;

    private void Awake()
    {
        _CarManager = transform.root.GetComponent<CarManager>();
    }

    public bool IsSparking { get { return _IsSparking; } }

    public void SetTilt(float currentTilt)
    {
        _Tilt = currentTilt;
    }

    private void Update()
    {
        if (_Tilt < -_SparkThreshold)
        {
            _Particles[0].gameObject.SetActive(true);
            _IsSparking = true;
            try
            {
                _CarManager.CarAudioHandler.Sounds[0].volume = Mathf.Lerp(_CarManager.CarAudioHandler.Sounds[0].volume, 0.6f, 4 * Time.deltaTime);
            }
            catch
            {

            }
        }
        else if (_Tilt > _SparkThreshold)
        {
            _Particles[1].gameObject.SetActive(true);
            _IsSparking = true;

            try
            {
                _CarManager.CarAudioHandler.Sounds[0].volume = Mathf.Lerp(_CarManager.CarAudioHandler.Sounds[0].volume, 0.6f, 4 * Time.deltaTime);
            }
            catch
            {

            }
        }
        else
        {
            _Particles[0].gameObject.SetActive(false);
            _Particles[1].gameObject.SetActive(false);
            _IsSparking = false;
            try
            {

                _CarManager.CarAudioHandler.Sounds[0].volume = Mathf.Lerp(_CarManager.CarAudioHandler.Sounds[0].volume, 0f, 7 * Time.deltaTime);
            }
            catch
            {

            }
        }



    }
}
