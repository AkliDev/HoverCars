//This script acts as a hub for all car scripts if they want to acces other variables or methods of other scripts
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CarHoverHandler))]
[RequireComponent(typeof(CarBehaviour))]
[RequireComponent(typeof(PlayerInputs))]
public class CarManager : MonoBehaviour
{
    [SerializeField] private bool _IsAI;

    [SerializeField] bool _SetStats;

    [Header("Car Objects")]
    private Transform _ModelTransform; // The visual aspect of the car.
    private Transform _CameraTransform; // The followcamera of the car.
    private Transform _OrientationLookAtTransform; // Acts as the up orientation for the camera and lookat for the camera.
    private Transform _SplineProjection; // Transform of the spline projection;

    [Header("Car Components")]
    private CarHoverHandler _HoverHandler; // Script that handles the hover height of the car using the PID controller and the calculates the orientation of the car.
    private CarBehaviour _Behaviour; // The Cars statemachine to control the cars behaviour.
    private PlayerInputs _Inputs; // Handles the players input to control the car.
    private CarParticleHandler _ParticleHandler; //Handles particles of the car;
    private CarAudioHandler _CarAudioHandler; //Handles SFX of the car;
    private GetVertexPositionsOfBoxCollider _ColliderVertices;
   
    [Header("Hover Settings")]
    [SerializeField] private float _BaseHoverHeight;
    [SerializeField] private float _BaseMaxGroundDistence;
    [SerializeField] private float _BaseRaycastYOffset;
    [SerializeField] private float _BaseHoverForce;
    [SerializeField] private float _BaseHoverGravity;
    [SerializeField] private float _BaseFallGravity;
    [SerializeField] private LayerMask _BaseGroundMask;
    [SerializeField] private PIDController _BaseHoverPID;

    private float _HoverHeight; //The height the ship maintains when hovering
    private float _MaxGroundDistence;  //The distance the ship can be above the ground before it is "falling"
    private float _RaycastYOffset; // Offset That Shoots the raycast above the transformpoint of the car
    private float _HoverForce; //The force of the ship's hovering
    private float _HoverGravity;//The gravity applied to the ship while it is on the Grounded
    private float _FallGravity; //The gravity applied to the ship while it is airborn
    private LayerMask _GroundMask; //A layer mask to determine what layer the ground is on
    private PIDController _HoverPID; //A PID controller to smooth the ship's hovering

    [Header("Behaviour Settings")]
    [SerializeField] private float _BaseAcceleration;
    [SerializeField] private float _BaseStraightFriction;
    [SerializeField] private float _BaseSideWaysFriction;
    [SerializeField] private float _BaseMaxStraigthVelocity;
    [SerializeField] private float _BaseYawTurnRate;
    [SerializeField] private float _BaseBankingRate;

    private float _Acceleration;
    private float _StraightFriction;
    private float _SideWaysFriction;
    private float _MaxStraigthVelocity;
    private float _YawTurnRate;
    private float _BankingRate;



    [Header("Forces")]
    private Vector3 _Velocity;
    private Vector3 _AngularVelocity;


    //public methods 

    //Gets
    public bool IsAI { get { return _IsAI; } }

    public Transform ModelTransform { get { return _ModelTransform; } }
    public Transform CameraTransform { get { return _CameraTransform; } }
    public Transform OrientationLookAtTransform { get { return _OrientationLookAtTransform; } }
    public Transform SplineProjection { get { return _SplineProjection; } }

    public CarHoverHandler CarHoverHandler { get { return _HoverHandler; } }
    public CarBehaviour Behaviour { get { return _Behaviour; } }
    public PlayerInputs Inputs { get { return _Inputs; } }
    public CarParticleHandler ParticleHandler { get { return _ParticleHandler; } }
    public CarAudioHandler CarAudioHandler { get { return _CarAudioHandler; } }
    public GetVertexPositionsOfBoxCollider ColliderVertices { get { return _ColliderVertices; } }


    public float BaseHoverHeight { get { return _BaseHoverHeight; } }
    public float BaseMaxGroundDistence { get { return _BaseMaxGroundDistence; } }
    public float BaseRaycastYOffset { get { return _BaseRaycastYOffset; } }
    public float BaseHoverForce { get { return _BaseHoverForce; } }
    public float BaseHoverGravity { get { return _BaseHoverGravity; } }
    public float BaseFallGravity { get { return _BaseFallGravity; } }
    public LayerMask BaseGroundMask { get { return _BaseGroundMask; } }
    public PIDController BaseHoverPID { get { return _BaseHoverPID; } }

    public float BaseAcceleration { get { return _BaseAcceleration; } }
    public float BaseStraightFriction { get { return _BaseStraightFriction; } }
    public float BaseSideWaysFriction { get { return _BaseSideWaysFriction; } }
    public float BaseMaxStraigthVelocity { get { return _BaseMaxStraigthVelocity; } }
    public float BaseYawTurnRate { get { return _BaseYawTurnRate; } }
    public float BaseBankingRate { get { return _BankingRate; } }

    public float HoverHeight { get { return _HoverHeight; } }
    public float MaxGroundDistence { get { return _MaxGroundDistence; } }
    public float RaycastYOffset { get { return _RaycastYOffset; } }
    public float HoverForce { get { return _HoverForce; } }
    public float HoverGravity { get { return _HoverGravity; } }
    public float FallGravity { get { return _FallGravity; } }
    public LayerMask GroundMask { get { return _GroundMask; } }
    public PIDController HoverPID { get { return _HoverPID; } }

    public float Acceleration { get { return _Acceleration; } }
    public float StraightFriction { get { return _StraightFriction; } }
    public float SideWaysFriction { get { return _SideWaysFriction; } }
    public float MaxStraigthVelocity { get { return _MaxStraigthVelocity; } }
    public float YawTurnRate { get { return _YawTurnRate; } }
    public float BankingRate { get { return _BankingRate; } }

//Sets
    public void SetModelPosition(Vector3 newposition)
    {
        _ModelTransform.position = newposition;
    }
    public void SetModelRotation(Quaternion newrotaton)
    {
        _ModelTransform.rotation = newrotaton;
    }
    public void SetModelLocalRotation(Quaternion newlocalrotaton)
    {
        _ModelTransform.localRotation = newlocalrotaton;
    }
    public void SetLookAtPosition(Vector3 newposition)
    {
        _OrientationLookAtTransform.position = newposition;
    }
    public void SetUpOrientationRotation(Quaternion newrotaton)
    {
        _OrientationLookAtTransform.rotation = newrotaton;
    }

    //Hover
    public void SetHoverHeight(float newHoverHeight)
    {
        if (newHoverHeight >= 0)
            _HoverHeight = newHoverHeight;
    }
    public void SetMaxGroundDistence(float newMaxGroundDistence)
    {
        if (newMaxGroundDistence >= 0)
            _MaxGroundDistence = newMaxGroundDistence;
    }
    public void SetRaycastYOffset(float newRaycastYOffset)
    {
        if (newRaycastYOffset >= 0)
            _RaycastYOffset = newRaycastYOffset;
    }
    public void SetHoverForce(float newHoverForce)
    {
        if (newHoverForce >= 0)
            _HoverForce = newHoverForce;
    }
    public void SetHoverGravity(float newHoverGravity)
    {
        if (newHoverGravity >= 0)
            _HoverGravity = newHoverGravity;
    }
    public void SetFallGravity(float newFallGravity)
    {
        if (newFallGravity >= 0)
            _FallGravity = newFallGravity;
    }
    public void SetGroundMask(LayerMask newGroundMask)
    {
        _GroundMask = newGroundMask;
    }
    public void SetHoverPID(PIDController newHoverPID)
    {
        _HoverPID = newHoverPID;
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
    public void SetMaxStraigthVelocity(float newMaxStraigthVelocity)
    {
        if (newMaxStraigthVelocity >= 0)
            _MaxStraigthVelocity = newMaxStraigthVelocity;
    }
    public void SetYawTurnRate(float newYawTurnRate)
    {
        if (newYawTurnRate >= 0)
            _YawTurnRate = newYawTurnRate;
    }
    public void SettBankingRate(float newtBankingRate)
    {
        if (newtBankingRate >= 0)
            _BankingRate = newtBankingRate;
    }

    private void Awake()
    {
        //GetCarObjects        
        _ModelTransform = transform.GetChild(0);
        _CameraTransform = transform.GetChild(1);
        _OrientationLookAtTransform = transform.GetChild(2);
        _SplineProjection = transform.GetChild(3);

        //GetCarComponents
        _HoverHandler = GetComponent<CarHoverHandler>();
        _Behaviour = GetComponent<CarBehaviour>();
        _Inputs = GetComponent<PlayerInputs>();
        _ParticleHandler = GetComponent<CarParticleHandler>();
        _CarAudioHandler = GetComponent<CarAudioHandler>();
        _ColliderVertices = GetComponent<GetVertexPositionsOfBoxCollider>();

        if (_IsAI)
        {
            _CameraTransform.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        SetStatsToBaseStats();
    }
    private void SetStatsToBaseStats()
    {
        SetHoverHeight(_BaseHoverHeight);
        SetMaxGroundDistence(_BaseMaxGroundDistence);
        SetRaycastYOffset(_BaseRaycastYOffset);
        SetHoverForce(_BaseHoverForce);
        SetHoverGravity(_BaseHoverGravity);
        SetFallGravity(_BaseFallGravity);
        SetGroundMask(_BaseGroundMask);
        SetHoverPID(_BaseHoverPID);

        SetAcceleration(_BaseAcceleration);
        SetStraightFriction(_BaseStraightFriction);
        SetSideWaysFriction(_BaseSideWaysFriction);
        SetMaxStraigthVelocity(_BaseMaxStraigthVelocity);
        SetYawTurnRate(_BaseYawTurnRate);
        SettBankingRate(_BaseBankingRate);
    }
    private void Update()
    {
        if (_SetStats)
        {
            SetStatsToBaseStats();
        }
    }

    private void FixedUpdate()
    {
        //_Velocity = GetBehaviour.GetProjectedWorldVelocity + _HoverHandler.GetHoverVelocity;
        //_AngularVelocity = GetBehaviour.GetAngularVelocity;

        //transform.Translate(_Velocity * Time.fixedDeltaTime, Space.World);
        //transform.Rotate(_AngularVelocity * Time.fixedDeltaTime, Space.World);
    }
}

