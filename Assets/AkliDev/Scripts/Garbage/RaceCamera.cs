using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceCamera : MonoBehaviour
{
    public Transform _Logic;
    public Transform _Destination;


    [SerializeField]
    private float _Interpolation, _RotatonInterpolation;


    private void Start()
    {
        
    }
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, _Destination.position, _Interpolation * Time.fixedDeltaTime);

        transform.rotation = Quaternion.Lerp(transform.rotation,_Destination.rotation, _RotatonInterpolation * Time.fixedDeltaTime);
        
       
    }
}
