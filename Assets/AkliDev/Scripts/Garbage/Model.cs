using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class Model : MonoBehaviour
{
    private CarPhysics _Physics;
    private Transform _LogicTransform;
    
    // Use this for initialization
    void Start()
    {
        _Physics = GetComponentInParent<CarPhysics>();
        _LogicTransform = GetComponentInParent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 normal = _Physics._CombinedSurviceNormal;
        float triggers = 0;
        triggers += XCI.GetAxisRaw(XboxAxis.RightTrigger);
        triggers -= XCI.GetAxisRaw(XboxAxis.LeftTrigger);

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.up, (normal)) * transform.rotation, 5 * Time.deltaTime);
        //transform.Rotate(Vector3.forward * -triggers * 100f * Time.deltaTime, Space.Self);
        
        
        
    }
}
