using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManagerOld : MonoBehaviour
{
    //Car Objects
    private Transform _BoxColliderTransform; // Used to obtain the position of the suspension points.
    public  Transform _ModelTransform; // The visual aspect of the car.
    private Transform _CameraTransform; // The followcamera of the car.
    private Transform _OrientationLookAtTransform; // Acts as the up orientation for the camera and lookat for the camera.

    //Car Components
    private BoxCollider _BoxCollider; // Used to obtain the position of the suspension points.
    private CarPhysicsV3 _Physics; // Script that handles the hover height of the car using suspension points and the calculates the orientation of the car.
    private CarBehaviourOld _Behaviour; // The Cars statemachine to control the cars behaviour.
    private PlayerInputs _Inputs; // Handles the players input to control the car.

    [Header("Suspention Settings")]
    [SerializeField]
    private float _BaseRaycastDistence;
    [SerializeField] private float _BaseRaycastYoffse;
    [SerializeField] private float _BaseSuspensionForceMultipleir;
    [SerializeField] private float _BaseSuspensionFrictionMultiplier;
    [SerializeField] private float _BaseGravityMultiplier;

    private float _RaycastDistence;
    private float _RaycastYoffset;
    private float _SuspensionForceMultipleir;
    private float _SuspensionFrictionMultiplier;
    private float _GravityMultiplier;

    [Header("Behaviour Settings")]
    [SerializeField]
    private float _BaseAcceleration;
    [SerializeField] private float _BaseStraightFriction;
    [SerializeField] private float _BaseSideWaysFriction;
    [SerializeField] private float _BaseMaxStrigthVelocity;
    [SerializeField] private float _BaseYawTurnRate;
    [SerializeField] private float _BaseBankingRate;

    private float _Acceleration;
    private float _StraightFriction;
    private float _SideWaysFriction;
    private float _MaxStrigthVelocity;
    private float _YawTurnRate;
    private float _BankingRate;

    //public methods 

    //Gets
    public Transform GetBoxColliderTransform { get { return _BoxColliderTransform; } }
    public Transform GetModelTransform { get { return _ModelTransform; } }
    public Transform GetCameraTransform { get { return _CameraTransform; } }
    public Transform GetOrientationLookAtTransform { get { return _OrientationLookAtTransform; } }

    public BoxCollider GetBoxCollider { get { return _BoxCollider; } }
    public CarPhysicsV3 GetPhysics { get { return _Physics; } }
    public CarBehaviourOld GetBehaviour { get { return _Behaviour; } }
    public PlayerInputs GetInputs { get { return _Inputs; } }

    public float GetRaycastDistence { get { return _RaycastDistence; } }
    public float GetRaycastYoffset { get { return _RaycastYoffset; } }
    public float GetSuspensionForceMultipleir { get { return _SuspensionForceMultipleir; } }
    public float GetSuspensionFrictionMultiplier { get { return _SuspensionFrictionMultiplier; } }
    public float GetGravityMultiplier { get { return _GravityMultiplier; }}

    public float GetAcceleration { get { return _Acceleration; } }
    public float GetStraightFriction { get { return _StraightFriction; }}
    public float GetSideWaysFriction { get { return _SideWaysFriction; }}
    public float GetMaxStrigthVelocity { get { return _MaxStrigthVelocity; }}
    public float GetYawTurnRate { get { return _YawTurnRate; }}
    public float GetBankingRate { get { return _BankingRate; }}



    //Sets

    public void SetModelPosition(Vector3 newposition)
    {
        _ModelTransform.position = newposition;
    }
    public void SetModelRotation(Quaternion newrotaton)
    {
        _ModelTransform.rotation = newrotaton;
    }
    public void SetLookAtPosition(Vector3 newposition)
    {
        _OrientationLookAtTransform.position = newposition;
    }
    public void SetUpOrientationRotation(Quaternion newrotaton)
    {
        _OrientationLookAtTransform.rotation = newrotaton;
    }

    //suspension
    public void SetRaycastDistence(float newRaycastDistance)
    {
        if (newRaycastDistance >= 0)
            _RaycastDistence = newRaycastDistance;
    }
    public void SetRaycastYoffset(float newRaycastRaycastYoffset)
    {
        if (newRaycastRaycastYoffset >= 0)
            _RaycastYoffset = newRaycastRaycastYoffset;
    }
    public void SetSuspensionForceMultipleir(float newSuspensionForceMultipleir)
    {
        if (newSuspensionForceMultipleir >= 0)
            _SuspensionForceMultipleir = newSuspensionForceMultipleir;
    }
    public void SetSuspensionFrictionMultiplier(float newSuspensionFrictionMultiplier)
    {
        if (newSuspensionFrictionMultiplier >= 0)
            _SuspensionFrictionMultiplier = newSuspensionFrictionMultiplier;
    }
    public void SetGravityMultiplier(float newGravityMultiplier)
    {
        if (newGravityMultiplier >= 0)
            _GravityMultiplier = newGravityMultiplier;
    }

    // behaviour
    public void SetAcceleration(float newAcceleration)
    {
        if (newAcceleration >= 0)
            _Acceleration = newAcceleration;
    }
    public void SetStraightFriction(float newStraightFriction)
    {
        if (newStraightFriction >= 0)
            _StraightFriction = newStraightFriction;
    }
    public void SetSideWaysFriction(float newSideWaysFriction)
    {
        if (newSideWaysFriction >= 0)
            _SideWaysFriction = newSideWaysFriction;
    }
    public void SetMaxStrigthVelocity(float newMaxStrigthVelocity)
    {
        if (newMaxStrigthVelocity >= 0)
            _MaxStrigthVelocity = newMaxStrigthVelocity;
    }
    public void SetYawTurnRate(float newYawTurnRate)
    {
        if (newYawTurnRate >= 0)
            _YawTurnRate = newYawTurnRate;
    }
    public void SettBankingRate(float newtBankingRate)
    {
        if (newtBankingRate >= 0)
            _BaseBankingRate = newtBankingRate;
    }


    private void Awake()
    {
        //GetObjects
        _BoxColliderTransform = transform.GetChild(0);
        _ModelTransform = transform.GetChild(1);
        _CameraTransform = transform.GetChild(2);
        _OrientationLookAtTransform = transform.GetChild(3);

        //GetComponents
        _Physics = GetComponent<CarPhysicsV3>();
        _Behaviour = GetComponent<CarBehaviourOld>();
        _Inputs = GetComponent<PlayerInputs>();
        _BoxCollider = _BoxColliderTransform.GetComponent<BoxCollider>();
    }
    private void Start()
    {
        SetRaycastDistence(_BaseRaycastDistence);
        SetRaycastYoffset(_BaseRaycastYoffse);
        SetSuspensionForceMultipleir(_BaseSuspensionForceMultipleir);
        SetSuspensionFrictionMultiplier(_BaseSuspensionFrictionMultiplier);
        SetGravityMultiplier(_BaseGravityMultiplier);

        SetAcceleration(_BaseAcceleration);
        SetStraightFriction(_BaseStraightFriction);
        SetSideWaysFriction(_BaseSideWaysFriction);
        SetMaxStrigthVelocity(_BaseMaxStrigthVelocity);
        SetYawTurnRate(_BaseYawTurnRate);
    }

    private void FixedUpdate()
    {

    }

    public void TranslateTransform(Vector3 direction, Space space)
    {
        transform.Translate(direction * Time.fixedDeltaTime, space);
    }

    public void RotateTransform(Vector3 rotation, Space space)
    {
        transform.Rotate(rotation * Time.fixedDeltaTime, space);
    }
}

