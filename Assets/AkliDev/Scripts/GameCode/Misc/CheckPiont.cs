using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPiont : MonoBehaviour
{
    [SerializeField] int _Index;
    private FinishLine _FinishLine;

    private void Start()
    {
        _FinishLine = transform.parent.GetComponent<FinishLine>();
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.CompareTag("PlayerSensor"))
        {
            Vector3 dir = ( other.transform.position -transform.position).normalized ;
            float exitSide = Mathf.Sign( Vector3.Dot(dir, transform.forward));
            if (exitSide > 0)
            {
                if (_Index == 0)
                {
                    _FinishLine._CheckPoints[_Index] = true;
                }
                else
                {
                    if (_FinishLine._CheckPoints[_Index - 1] == true)
                    {
                        _FinishLine._CheckPoints[_Index] = true;
                    }
                }
                
            }
            else
            {
                _FinishLine._CheckPoints[_Index] = false;
                
            }
        }

    }
}
