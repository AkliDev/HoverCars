using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{

    [HideInInspector] public bool[] _CheckPoints;
    private bool _IsReady;

    public bool _DebugMode;

    private void Start()
    {
        _CheckPoints = new bool[transform.childCount];
    }

    private void Update()
    {
        int checks = 0;
        for (int i = 0; i < _CheckPoints.Length; i++)
        {
            if (_CheckPoints[i] == true)
            {
                checks++;
            }
        }

        if (checks == _CheckPoints.Length)
        {
           _IsReady = true;
        }
        else
        {
            _IsReady = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if ((_IsReady || _DebugMode) && other.gameObject.CompareTag("PlayerSensor"))
        {
            GameManager.instance.PlayerCompletedLap();
            _IsReady = false;

            for (int i = 0; i < _CheckPoints.Length; i++)
            {
                _CheckPoints[i] = false;
            }
        }
    }
}
