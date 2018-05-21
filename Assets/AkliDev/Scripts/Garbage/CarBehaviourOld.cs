using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CarBehaviourOld : MonoBehaviour
{
    private CarManagerOld _Manager;

    private IState _State;
    private AnimationState _AnimationState;
    private Animator _Animator;

    private Vector3 _ProjectedForward;
    private Vector3 _ProjectedRight;

    private Vector3 _ProjectedWorldVelocity;
    private Vector3 _PreProjectedWorldVelocity;
    private Vector3 _LocalVelocity;
    private Vector3 _PreLocalVelocity;
    private Vector3 _AngularVelocity;
    private Vector3 _UsedMoveVelocity;

    private float _SpeedPerFrame;

    //Getters
    public CarManagerOld GetCarManager { get { return _Manager; } }

    public AnimationState GetAnimationState { get { return _AnimationState; } }

    public Vector3 GetProjectedForward { get { return _ProjectedForward; } }
    public Vector3 GetProjectedRight { get { return _ProjectedRight; } }
    public Vector3 GetProjectedWorldVelocity { get { return _ProjectedWorldVelocity; } }
    public Vector3 GetPreProjectedWorldVelocity { get { return _PreProjectedWorldVelocity; } }
    public Vector3 GetLocalVelocity { get { return _LocalVelocity; } }
    public Vector3 GetPreLocalVelocity { get { return _PreLocalVelocity; } }
    public Vector3 GetAngularVelocity { get { return _AngularVelocity; } }
    public Vector3 GetUsedMoveVelocity { get { return _UsedMoveVelocity; } }

    public float GetSpeedPerFrame { get { return _SpeedPerFrame; } }




    //Setters
    public void SetAnimation(AnimationState newAninemationState) { _AnimationState = newAninemationState; }

    private void SetForwardProjection(Vector3 newForwardProjection) { _ProjectedForward = newForwardProjection; }
    private void SetRightProjection(Vector3 newRightProjection) { _ProjectedRight = newRightProjection; }

    public void SetProjectedWorldVelocity(Vector3 newProjectedWorldVelocity) { _ProjectedWorldVelocity = newProjectedWorldVelocity; }
    public void SetPreProjectedWorldVelocity(Vector3 newPreProjectedWorldVelocity) { _PreProjectedWorldVelocity = newPreProjectedWorldVelocity; }
    public void SetLocalVelocity(Vector3 newLocalVelocity) { _LocalVelocity = newLocalVelocity; }
    public void SetPreLocalVelocity(Vector3 newPreLocalVelocity) { _PreLocalVelocity = newPreLocalVelocity; }
    public void SetAngularVelocity(Vector3 newAngularVelocity) { _AngularVelocity = newAngularVelocity; }
    public void SetUsedMoveVelocity(Vector3 newUsedMoveVelocity) { _UsedMoveVelocity = newUsedMoveVelocity; }

    public void SetSpeedPerFrame(float newSpeed) { _SpeedPerFrame = newSpeed; }

    ///
    private void Awake()
    {
        _Manager = GetComponent<CarManagerOld>();
    }
    void Start()
    {
        _State = new NormalDriveOld(this);
    }
    void Update()
    {
        _State.Update();
    }
    void FixedUpdate()
    {
        SetForwardProjection(Vector3.ProjectOnPlane(transform.forward, _Manager.GetPhysics.GetInterpolatedNormal).normalized);
        SetRightProjection(Vector3.ProjectOnPlane(transform.right, _Manager.GetPhysics.GetInterpolatedNormal).normalized);

        _State.FixedUpdate();
      
        GetCarManager.RotateTransform(GetAngularVelocity, Space.World);
        GetCarManager.TranslateTransform(GetProjectedWorldVelocity, Space.World);

        CheckEndFriction();
    }
    public void SwitchState(IState state)
    {
        _State.OnExit();
        _State = state;
        _State.OnEnter();
        _Animator.SetInteger("State", (int)_AnimationState);
    }

    private void CheckEndFriction()
    {
        if (Mathf.Sign(GetPreLocalVelocity.z) != Mathf.Sign(GetLocalVelocity.z) && GetPreLocalVelocity.z != 0 && GetLocalVelocity.z != 0)
        {
            SetProjectedWorldVelocity(GetProjectedWorldVelocity - (GetLocalVelocity.z * GetProjectedForward));
        }

        if (Mathf.Sign(GetPreLocalVelocity.x) != Mathf.Sign(GetLocalVelocity.x) && GetPreLocalVelocity.x != 0 && GetLocalVelocity.x != 0)
        {
            SetProjectedWorldVelocity(GetProjectedWorldVelocity - (GetLocalVelocity.x * GetProjectedRight));
        }


        SetPreProjectedWorldVelocity(GetProjectedWorldVelocity);
        SetPreLocalVelocity(GetLocalVelocity);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, _ProjectedWorldVelocity.normalized * 5);
    }

}



public class NormalDriveOld : IState
{
    CarBehaviourOld _Car;

    public NormalDriveOld(CarBehaviourOld controller)
    {
        _Car = controller;
    }
    public void OnEnter()
    {

    }
    public void Update()
    {

    }
    public void FixedUpdate()
    {
        SetYawVelocity();
        Accelerate();
        SetVelocity();       
    }
    public void OnExit()
    {

    }

    private void Accelerate()
    {
        
        float speedPerFrame = 0;
        speedPerFrame += _Car.GetCarManager.GetAcceleration * _Car.GetCarManager.GetInputs._AccelAxis;
        speedPerFrame -= _Car.GetCarManager.GetAcceleration * _Car.GetCarManager.GetInputs._BrakeAxis;
        _Car.SetSpeedPerFrame(speedPerFrame);
    }
    private void SetVelocity()
    {
        float projectedWorldVelocityMagnitude = _Car.GetProjectedWorldVelocity.magnitude;
        _Car.SetProjectedWorldVelocity(Vector3.ProjectOnPlane(_Car.GetProjectedWorldVelocity, _Car.GetCarManager.GetPhysics.GetInterpolatedNormal).normalized);
        _Car.SetProjectedWorldVelocity(_Car.GetProjectedWorldVelocity * projectedWorldVelocityMagnitude);

        if (_Car.GetLocalVelocity.z < _Car.GetCarManager.GetMaxStrigthVelocity && _Car.GetLocalVelocity.z > -(_Car.GetCarManager.GetMaxStrigthVelocity * 0.25f))
        {
            _Car.SetProjectedWorldVelocity(_Car.GetProjectedWorldVelocity + (_Car.GetProjectedForward * _Car.GetSpeedPerFrame * Time.fixedDeltaTime));
        }

        projectedWorldVelocityMagnitude = _Car.GetProjectedWorldVelocity.magnitude;
        _Car.SetProjectedWorldVelocity(Vector3.ProjectOnPlane(_Car.GetProjectedWorldVelocity, _Car.GetCarManager.GetPhysics.GetInterpolatedNormal).normalized);
        _Car.SetProjectedWorldVelocity(_Car.GetProjectedWorldVelocity * projectedWorldVelocityMagnitude);



        Vector3 localVelocity = Vector3.zero;
        
        localVelocity.x = Vector3.Dot(_Car.GetProjectedWorldVelocity, _Car.GetProjectedRight);
        localVelocity.y = Vector3.Dot(_Car.GetProjectedWorldVelocity, Vector3.Cross(_Car.GetProjectedRight, _Car.GetProjectedForward));
        localVelocity.z = Vector3.Dot(_Car.GetProjectedWorldVelocity, _Car.GetProjectedForward);
        _Car.SetLocalVelocity(localVelocity);

        if (localVelocity.x != 0)
        {
            _Car.SetProjectedWorldVelocity(_Car.GetProjectedWorldVelocity - (_Car.GetCarManager.GetSideWaysFriction * _Car.GetProjectedRight * Mathf.Sign(localVelocity.x) * Time.fixedDeltaTime));
        }
        if (localVelocity.z != 0)
        {
            _Car.SetProjectedWorldVelocity(_Car.GetProjectedWorldVelocity - (_Car.GetCarManager.GetStraightFriction * _Car.GetProjectedForward * Mathf.Sign(localVelocity.z) * Time.fixedDeltaTime));
        }

     

    }

   
    private void SetYawVelocity()
    {
        Vector3 YawVelocity = _Car.transform.up * _Car.GetCarManager.GetInputs._SteerAxis * _Car.GetCarManager.GetYawTurnRate;
        _Car.SetAngularVelocity(YawVelocity);
    }
}

public class DriftLeftOld : IState
{
    CarBehaviourOld _Car;

    public DriftLeftOld(CarBehaviourOld controller)
    {
        _Car = controller;
    }
    public void OnEnter()
    {

    }
    public void Update()
    {

    }
    public void FixedUpdate()
    {

    }
    public void OnExit()
    {

    }
}

public class DriftRightOld : IState
{
    CarBehaviourOld _Car;

    public DriftRightOld(CarBehaviourOld controller)
    {
        _Car = controller;
    }
    public void OnEnter()
    {

    }
    public void Update()
    {

    }
    public void FixedUpdate()
    {

    }
    public void OnExit()
    {

    }

}

public class StateOld : IState
{
    CarBehaviourOld _Car;

    public StateOld(CarBehaviourOld controller)
    {
        _Car = controller;
    }
    public void OnEnter()
    {

    }
    public void Update()
    {

    }
    public void FixedUpdate()
    {

    }
    public void OnExit()
    {

    }

}







