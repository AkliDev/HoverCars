using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class KANSAIDORIFTO : MonoBehaviour
{
    public XboxController controller;
    public bool _IsUsingController;

    public CarPhysics _Physics;
    [SerializeField]
    public float _TiltMultiplier,_TiltAmount;
    [SerializeField]
    public float _Acceleration, _FrictionMultiplier, _MaxVelocityMagnitude, _SpeedPerFrame;
   
   
    [SerializeField]
    public Vector3 _Velocity, _PreVelocity,  _ProjectedVelocity, _PreProjectedVelocity, _UsedVelocity;
    [SerializeField]
    public float _VelocityMagnitude;


    [SerializeField]
    private float _HorizontalAxis, _TurnSensitivity, _VerticalAxis, _LTrigger, _RTrigger;
    private bool _AButton, _BButton, _XButton, _YButton,_DPadUp, _DPadDown, _DPadLeft, _DPadRight,_BumperLeft,_BumperRight;
    
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
            _AButton = XCI.GetButton(XboxButton.A);
            _BButton = XCI.GetButton(XboxButton.B);
            _XButton = XCI.GetButton(XboxButton.X);
            _YButton = XCI.GetButton(XboxButton.Y);
            _DPadUp  = XCI.GetButton(XboxButton.DPadUp);
            _DPadDown = XCI.GetButton(XboxButton.DPadDown);
            _DPadLeft = XCI.GetButton(XboxButton.DPadLeft);
            _DPadRight = XCI.GetButton(XboxButton.DPadRight);
            _BumperLeft = XCI.GetButton(XboxButton.LeftBumper);
            _BumperRight = XCI.GetButton(XboxButton.RightBumper);

            if (XCI.GetButtonDown(XboxButton.X))
            {
                _FrictionMultiplier = _FrictionMultiplier * 1.5f;
               
            }
            if (XCI.GetButtonUp(XboxButton.X))
            {
                _FrictionMultiplier = _FrictionMultiplier / 1.5f;
            }

        }
        if (_IsUsingController == false)
        {
            _RTrigger = Input.GetAxis("Vertical");
            _HorizontalAxis = Input.GetAxis("Horizontal");
            if (Input.GetKeyDown(KeyCode.E))
            {
                _FrictionMultiplier = _FrictionMultiplier * 1.5f;
            }
            if (Input.GetKeyUp(KeyCode.E))
            {
                _FrictionMultiplier = _FrictionMultiplier / 1.5f;
            }
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
     
        _Velocity += transform.forward * _SpeedPerFrame;
       

       
        Vector3 projectedTransformForward = Vector3.ProjectOnPlane(transform.forward, _Physics._InterpolatedNormal).normalized;
        
        if(_VelocityMagnitude < _MaxVelocityMagnitude && _VelocityMagnitude > -_MaxVelocityMagnitude* 0.25f)
        _ProjectedVelocity += projectedTransformForward * _SpeedPerFrame;

        float projectedVelocityMagnitude = _ProjectedVelocity.magnitude;
        _ProjectedVelocity = Vector3.ProjectOnPlane(_ProjectedVelocity, _Physics._InterpolatedNormal).normalized;
        _ProjectedVelocity = _ProjectedVelocity * projectedVelocityMagnitude;




        //Debug.Log(_ProjectedVelocity.magnitude);



        if (_Velocity != Vector3.zero)
        {
            _Velocity -= _FrictionMultiplier * _Velocity.normalized * Time.fixedDeltaTime;
            if (_AButton)
            {
                _Velocity -= _FrictionMultiplier * _Velocity.normalized * 0.7f * Time.fixedDeltaTime;
            }
        }

        if (_ProjectedVelocity != Vector3.zero)
        {
            _ProjectedVelocity -= _FrictionMultiplier * _ProjectedVelocity.normalized * Time.fixedDeltaTime;
            if (_AButton)
            {
                _ProjectedVelocity -= _FrictionMultiplier * _ProjectedVelocity.normalized * Time.fixedDeltaTime;
            }
        }

        _VelocityMagnitude = _ProjectedVelocity.magnitude;
    }

    private void ElaborateMovement()
    {

        _UsedVelocity = _ProjectedVelocity * Time.fixedDeltaTime;
        transform.Translate(_UsedVelocity, Space.World);
        _Physics._Model.transform.position = Vector3.Slerp(_Physics._Model.transform.position, transform.position,Time.fixedTime);

        float tilt = Mathf.Lerp(_TiltAmount, _HorizontalAxis * _TiltMultiplier * _UsedVelocity.magnitude, 5 * Time.deltaTime);
        transform.Rotate(transform.up * _HorizontalAxis * _TurnSensitivity * Time.fixedDeltaTime, Space.World);

        _Physics._Model.RotateAround(_Physics._Model.position, Vector3.Cross(_Physics._Model.up, _UsedVelocity).normalized, _TiltMultiplier * _UsedVelocity.magnitude *( 1f- Mathf.Abs( Vector3.Dot(_Physics._Model.forward.normalized,_UsedVelocity.normalized))));
        _Physics._Model.RotateAround(_Physics._Model.position, Vector3.Cross(_Physics._Model.up, _UsedVelocity).normalized, _TiltMultiplier * _UsedVelocity.magnitude * 0.2f);

        _Physics._UpOrientation.position = transform.position + _ProjectedVelocity.normalized;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position , _ProjectedVelocity.normalized * 5);
    }

    private void FrictionEnd()
    {

        if (Mathf.Sign(_PreVelocity.x) != Mathf.Sign(_Velocity.x) && _PreVelocity.x != 0 && _Velocity.x != 0)
        {
            _Velocity.x = 0;
            _PreVelocity.x = 0;

        }
        if (Mathf.Sign(_PreVelocity.y) != Mathf.Sign(_Velocity.y) && _PreVelocity.y != 0 && _Velocity.y != 0)
        {
            _Velocity.y = 0;
            _PreVelocity.y = 0;
        }
        if (Mathf.Sign(_PreVelocity.z) != Mathf.Sign(_Velocity.z) && _PreVelocity.z != 0 && _Velocity.z != 0)
        {
            _Velocity.z = 0;
            _PreVelocity.z = 0;

        }

        _PreVelocity = _Velocity;




        if (Mathf.Sign(_PreProjectedVelocity.x) != Mathf.Sign(_ProjectedVelocity.x) && _PreProjectedVelocity.x != 0 && _ProjectedVelocity.x != 0)
        {
            _ProjectedVelocity.x = 0;
            _PreProjectedVelocity.x = 0;

        }
        if (Mathf.Sign(_PreProjectedVelocity.y) != Mathf.Sign(_ProjectedVelocity.y) && _PreProjectedVelocity.y != 0 && _ProjectedVelocity.y != 0)
        {
            _ProjectedVelocity.y = 0;
            _PreProjectedVelocity.y = 0;
        }
        if (Mathf.Sign(_PreProjectedVelocity.z) != Mathf.Sign(_ProjectedVelocity.z) && _PreProjectedVelocity.z != 0 && _ProjectedVelocity.z != 0)
        {
            _ProjectedVelocity.z = 0;
            _PreProjectedVelocity.z = 0;

        }
        _PreProjectedVelocity = _ProjectedVelocity;

    }
}
