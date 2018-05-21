using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class Bhero : MonoBehaviour
{
    public XboxController controller;
    public bool _IsUsingController;

    public CarPhysics _Physics;
    [SerializeField]
    public float _TiltMultiplier;
    [SerializeField]
    public float _Acceleration, _MaxStrigthVelocity,  _SideWaysfriction, _Straightfriction, _TurnSensitivity;


    public float _SpeedPerFrame;

    [SerializeField]
    public Vector3  _ProjectedWorldVelocity, _PreProjectedWorldVelocity, 
        _LocalVelocity, _PreLocalVelocity,
        _AngularVelocity,
        _UsedMoveVelocity;
   


    [SerializeField]
    private float _HorizontalAxis, _VerticalAxis, _LTrigger, _RTrigger;
    private bool _AButton, _BButton, _XButton, _YButton, _DPadUp, _DPadDown, _DPadLeft, _DPadRight, _BumperLeft, _BumperRight;

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
            _DPadUp = XCI.GetButton(XboxButton.DPadUp);
            _DPadDown = XCI.GetButton(XboxButton.DPadDown);
            _DPadLeft = XCI.GetButton(XboxButton.DPadLeft);
            _DPadRight = XCI.GetButton(XboxButton.DPadRight);
            _BumperLeft = XCI.GetButton(XboxButton.LeftBumper);
            _BumperRight = XCI.GetButton(XboxButton.RightBumper);         
        }
        if (_IsUsingController == false)
        {
            _RTrigger = Input.GetAxis("Vertical");
            _HorizontalAxis = Input.GetAxis("Horizontal");
        }


    }
    private void Accelerate()
    {
        _SpeedPerFrame = 0;
        _SpeedPerFrame -= _LTrigger * _Acceleration ;
        _SpeedPerFrame += _RTrigger * _Acceleration ;
    }
    private void SetVelocity()
    {
        float projectedVelocityMagnitude = _ProjectedWorldVelocity.magnitude;
        _ProjectedWorldVelocity = Vector3.ProjectOnPlane(_ProjectedWorldVelocity, _Physics._InterpolatedNormal).normalized;
        _ProjectedWorldVelocity = _ProjectedWorldVelocity * projectedVelocityMagnitude;

        Vector3 projectedTransformForward = Vector3.ProjectOnPlane(transform.forward, _Physics._InterpolatedNormal).normalized;

        if(_LocalVelocity.z < _MaxStrigthVelocity && _LocalVelocity.z > -(_MaxStrigthVelocity * 0.25f))
        _ProjectedWorldVelocity += projectedTransformForward * _SpeedPerFrame * Time.fixedDeltaTime;



        _AngularVelocity = (transform.up * _HorizontalAxis * _TurnSensitivity);

         projectedVelocityMagnitude = _ProjectedWorldVelocity.magnitude;
        _ProjectedWorldVelocity = Vector3.ProjectOnPlane(_ProjectedWorldVelocity, _Physics._InterpolatedNormal).normalized;
        _ProjectedWorldVelocity = _ProjectedWorldVelocity * projectedVelocityMagnitude;




        _LocalVelocity.x = Vector3.Dot(_ProjectedWorldVelocity, transform.right);
        _LocalVelocity.y = Vector3.Dot(_ProjectedWorldVelocity, transform.up);
        _LocalVelocity.z = Vector3.Dot(_ProjectedWorldVelocity, transform.forward);

        if (_LocalVelocity.x != 0)
        {
            _ProjectedWorldVelocity -= _SideWaysfriction * transform.right.normalized * Mathf.Sign(_LocalVelocity.x) * Time.fixedDeltaTime;          
        }
        if (_LocalVelocity.z != 0)
        {
            _ProjectedWorldVelocity -= _Straightfriction * transform.forward.normalized *  Mathf.Sign(_LocalVelocity.z) * Time.fixedDeltaTime;         
        }

   
    }

    private void ElaborateMovement()
    {
        transform.Rotate(_AngularVelocity * Time.fixedDeltaTime, Space.World);

        _UsedMoveVelocity = _ProjectedWorldVelocity * Time.fixedDeltaTime;
        transform.Translate(_UsedMoveVelocity, Space.World);
        _Physics._Model.transform.position =  transform.position;

        
       
        

        _Physics._Model.RotateAround(_Physics._Model.position, Vector3.Cross(_Physics._Model.up, _UsedMoveVelocity).normalized, _TiltMultiplier * _UsedMoveVelocity.magnitude * (1f - Mathf.Abs(Vector3.Dot(_Physics._Model.forward.normalized, _UsedMoveVelocity.normalized))));
        _Physics._Model.RotateAround(_Physics._Model.position, Vector3.Cross(_Physics._Model.up, _UsedMoveVelocity).normalized, _TiltMultiplier * _UsedMoveVelocity.magnitude * 0.2f);

        _Physics._UpOrientation.position = transform.position + _ProjectedWorldVelocity.normalized *2;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, _ProjectedWorldVelocity.normalized * 5);
    }

    private void FrictionEnd()
    {


        if (Mathf.Sign(_PreLocalVelocity.z) != Mathf.Sign(_LocalVelocity.z) && _PreLocalVelocity.z != 0 && _LocalVelocity.z != 0)
        {
            _ProjectedWorldVelocity -= _LocalVelocity.z * transform.forward;
        }

        if (Mathf.Sign(_PreLocalVelocity.x) != Mathf.Sign(_LocalVelocity.x) && _PreLocalVelocity.x != 0 && _LocalVelocity.x != 0)
        {
            _ProjectedWorldVelocity -= _LocalVelocity.x * transform.right;
        }


        //if (Mathf.Sign(_PreProjectedVelocity.x) != Mathf.Sign(_ProjectedVelocity.x) && _PreProjectedVelocity.x != 0 && _ProjectedVelocity.x != 0)
        //{
        //    _ProjectedVelocity.x = 0;
        //    _PreProjectedVelocity.x = 0;

        //}
        //if (Mathf.Sign(_PreProjectedVelocity.y) != Mathf.Sign(_ProjectedVelocity.y) && _PreProjectedVelocity.y != 0 && _ProjectedVelocity.y != 0)
        //{
        //    _ProjectedVelocity.y = 0;
        //    _PreProjectedVelocity.y = 0;
        //}
        //if (Mathf.Sign(_PreProjectedVelocity.z) != Mathf.Sign(_ProjectedVelocity.z) && _PreProjectedVelocity.z != 0 && _ProjectedVelocity.z != 0)
        //{
        //    _ProjectedVelocity.z = 0;
        //    _PreProjectedVelocity.z = 0;

        //}
        _PreProjectedWorldVelocity = _ProjectedWorldVelocity;
        _PreLocalVelocity = _LocalVelocity;

    }

}
