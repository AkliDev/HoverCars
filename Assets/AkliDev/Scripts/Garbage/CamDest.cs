using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamDest : MonoBehaviour
{
    [SerializeField] private Transform _Logic;
    private KANSAIDORIFTO _CarController;

    [SerializeField] private Vector3 _Offset;

    private Quaternion _Defaultrotation;
    void Start()
    {
        _CarController = _Logic.gameObject.GetComponent<KANSAIDORIFTO>();
    }


    void FixedUpdate()
    {
        Vector3 lookDirection = (_Logic.position + _CarController._ProjectedVelocity * 15) - transform.position;

        Quaternion rotationDest = Quaternion.FromToRotation(transform.up, (_Logic.up)) * transform.rotation;
         rotationDest = Quaternion.FromToRotation(transform.forward, (lookDirection.normalized)) * transform.rotation;
       transform.forward = lookDirection;
        transform.forward = _Logic.up;

        if (_CarController._ProjectedVelocity == Vector3.zero)
        {
            transform.rotation = _Defaultrotation;
        }
        else
        {

            transform.rotation = rotationDest;
        }

        Vector3 position = _Logic.position + (-_CarController._ProjectedVelocity.normalized + (-transform.forward.normalized *4)).normalized * _Offset.z;
        position = position + _Logic.transform.up* _Offset.y;

  
        position = position + _Logic.right * (_Offset.x* ((Marth.ZeroSign(Vector3.SignedAngle(_Logic.forward.normalized, _CarController._UsedVelocity.normalized, _Logic.up.normalized)) - Vector3.Dot(_Logic.forward.normalized, _CarController._UsedVelocity.normalized))));
         
        transform.position = position;

        Debug.Log(Vector3.SignedAngle(_Logic.forward.normalized, _CarController._UsedVelocity.normalized, _Logic.up.normalized) / Mathf.Abs(Vector3.SignedAngle(_Logic.forward.normalized, _CarController._UsedVelocity.normalized, _Logic.up.normalized)));
    }
}
