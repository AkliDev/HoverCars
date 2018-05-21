using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class PlayerInputs : MonoBehaviour
{
    //Inputs
    [SerializeField] private XboxController controller;
    public bool _AccelButtonUp, _AccelButtonDown, _AccelButtonHeld, _DriftButtonUp, _DriftButtonDown, _DriftButtonHeld;
    public float _AccelAxis, _BrakeAxis, _SteerAxis;

    void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        if (XCI.IsPluggedIn(controller))
        {
            _AccelAxis = XCI.GetAxis(XboxAxis.RightTrigger, controller);
            _BrakeAxis = XCI.GetAxis(XboxAxis.LeftTrigger, controller);
            _SteerAxis = XCI.GetAxis(XboxAxis.LeftStickX, controller);

            _DriftButtonUp = XCI.GetButtonUp(XboxButton.X, controller);
            _DriftButtonDown = XCI.GetButtonDown(XboxButton.X, controller);
            _DriftButtonHeld = XCI.GetButton(XboxButton.X, controller);
        }
        else
        {
            _AccelAxis = Input.GetAxis("Vertical");
            _SteerAxis = Input.GetAxis("Horizontal");

            _DriftButtonUp = Input.GetKeyUp(KeyCode.Space);
            _DriftButtonDown = Input.GetKeyDown(KeyCode.Space);
            _DriftButtonHeld = Input.GetKey(KeyCode.Space);
        }
    }
}
