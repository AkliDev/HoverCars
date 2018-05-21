using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class CarController : MonoBehaviour
{
    public XboxController controller;
    public bool _IsUsingController;

    private CarPhysics _Physics;

    [SerializeField]
    public float _Acceleration, _FrictionMultiplier, _MaxVelocity, _SpeedPerFrame;
    [SerializeField]
    private float _HorizontalAxis, _TurnSensitivity, _VerticalAxis, _LTrigger, _RTrigger;
    
   
    [SerializeField]
    public float _Velocity, _PreVelocity;

    



    private static bool didQueryNumOfCtrlrs = false;

    void Start()
    {
        _Physics = GetComponent<CarPhysics>();

     

        if (!didQueryNumOfCtrlrs)
        {
            didQueryNumOfCtrlrs = true;

            int queriedNumberOfCtrlrs = XCI.GetNumPluggedCtrlrs();

            if (queriedNumberOfCtrlrs == 1)
            {
                Debug.Log("Only " + queriedNumberOfCtrlrs + " Xbox controller plugged in.");
            }
            else if (queriedNumberOfCtrlrs == 0)
            {
                Debug.Log("No Xbox controllers plugged in!");
            }
            else
            {
                Debug.Log(queriedNumberOfCtrlrs + " Xbox controllers plugged in.");
            }

            XCI.DEBUG_LogControllerNames();

            // This code only works on Windows
            if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
            {
                Debug.Log("Windows Only:: Any Controller Plugged in: " + XCI.IsPluggedIn(XboxController.Any).ToString());

                Debug.Log("Windows Only:: Controller 1 Plugged in: " + XCI.IsPluggedIn(XboxController.First).ToString());
                Debug.Log("Windows Only:: Controller 2 Plugged in: " + XCI.IsPluggedIn(XboxController.Second).ToString());
                Debug.Log("Windows Only:: Controller 3 Plugged in: " + XCI.IsPluggedIn(XboxController.Third).ToString());
                Debug.Log("Windows Only:: Controller 4 Plugged in: " + XCI.IsPluggedIn(XboxController.Fourth).ToString());
            }
        }
    }
    private void FixedUpdate()
    {
        Accelerate();
        SetVelocity();

        ElaborateMovement();
        FrictionEnd();
    }
    void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        if (_IsUsingController)
        {
            _HorizontalAxis = XCI.GetAxis(XboxAxis.LeftStickX, controller);
            _VerticalAxis = XCI.GetAxis(XboxAxis.LeftStickY, controller);
            _LTrigger = 1 * XCI.GetAxis(XboxAxis.LeftTrigger, controller);
            _RTrigger = 1 * XCI.GetAxis(XboxAxis.RightTrigger, controller);

            
        }
        if (_IsUsingController == false)
        {
            _RTrigger = Input.GetAxisRaw("Vertical");
            _HorizontalAxis = Input.GetAxisRaw("Horizontal");
        }
    }
    private void Accelerate()
    {
        _SpeedPerFrame = 0;
        _SpeedPerFrame -= _LTrigger * _Acceleration * Time.fixedDeltaTime;
        _SpeedPerFrame += _RTrigger * _Acceleration * Time.fixedDeltaTime;
    }
    private void SetVelocity()
    {
        if(_Velocity > -200 & _Velocity < 200)
        _Velocity += _SpeedPerFrame;

        if (_Velocity > 0)
        {
            _Velocity -= _FrictionMultiplier * Time.fixedDeltaTime;
        }
        else if (_Velocity < 0)

        {
            _Velocity += _FrictionMultiplier * Time.fixedDeltaTime;
        }
    }

    private void ElaborateMovement()
    {
        

        transform.Translate(transform.forward * _Velocity * Time.fixedDeltaTime, Space.World);


        _Physics._Model.transform.position = transform.position;
        transform.Rotate(transform.up * _HorizontalAxis * _TurnSensitivity * Time.fixedDeltaTime, Space.World);
        _Physics._Model.RotateAround(_Physics._Model.position, transform.forward, _HorizontalAxis * _Velocity * -0.05f);

    }
  
    private void FrictionEnd()
    {
        if (Mathf.Sign(_PreVelocity) != Mathf.Sign(_Velocity) && _PreVelocity != 0 && _Velocity != 0)
        {
            _Velocity = 0;
            _PreVelocity = 0;
        }
        _PreVelocity = _Velocity;
    }
}
