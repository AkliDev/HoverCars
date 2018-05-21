using UnityEngine;

public interface IState
{
    void OnEnter();
    void Update();
    void FixedUpdate();
    void OnExit();
}

public enum AnimationState
{

}

public struct AxisProjection
{
    public Vector3 forward, right;

    public AxisProjection(Vector3 Forward, Vector3 Right)
    {
        forward = Forward;
        right = Right;
    }
}

public class CarBehaviour : MonoBehaviour
{
    private CarManager _CarManager;

    private IState _State;
    private Animator _Animator;
    private AnimationState _AnimationState;
    private AxisProjection _TransformProjection;
    private AxisProjection _ProjectedSplineProjection;
    private AxisProjection _FrictionAxis;
    private Vector3 _AccelerationAxis;
    private Vector3 _ProjectedWorldVelocity;
    private Vector3 _PreProjectedWorldVelocity;
    private Vector3 _LocalVelocity;
    private Vector3 _PreLocalVelocity;
    private Vector3 _AngularVelocity;
    private Vector3 _UsedMoveVelocity;
    private float _SpeedPerFrame;
    private bool _IsCollidong;


    [SerializeField] private LayerMask _WallMask;
    [SerializeField] private Collider _WallCollider;

    //Getters
    public CarManager CarManager { get { return _CarManager; } }
    public AnimationState AnimationState { get { return _AnimationState; } }
    public AxisProjection TransformProjection { get { return _TransformProjection; } }
    public AxisProjection ProjectedSplineProjection { get { return _ProjectedSplineProjection; } }
    public AxisProjection FrictionAxis { get { return _FrictionAxis; } }
    public Vector3 AccelerationAxis { get { return _AccelerationAxis; } }
    public Vector3 ProjectedWorldVelocity { get { return _ProjectedWorldVelocity; } }
    public Vector3 PreProjectedWorldVelocity { get { return _PreProjectedWorldVelocity; } }
    public Vector3 LocalVelocity { get { return _LocalVelocity; } }
    public Vector3 PreLocalVelocity { get { return _PreLocalVelocity; } }
    public Vector3 AngularVelocity { get { return _AngularVelocity; } }
    public Vector3 UsedMoveVelocity { get { return _UsedMoveVelocity; } }
    public float SpeedPerFrame { get { return _SpeedPerFrame; } }
    public bool IsCollidong { get { return _IsCollidong; } }

    //Setters
    public void SetAnimation(AnimationState newAninemationState) { _AnimationState = newAninemationState; }
    public void SetTransformProjection(AxisProjection newTransformProjection) { _TransformProjection = newTransformProjection; }
    public void SetProjectedSplineProjection(AxisProjection newProjectedSplineProjection) { _ProjectedSplineProjection = newProjectedSplineProjection; }
    public void SetFrictionAxis(AxisProjection newFrictionAxis) { _FrictionAxis = newFrictionAxis; }
    public void SetAccelerationAxis(Vector3 newAccelerationAxis) { _AccelerationAxis = newAccelerationAxis; }
    public void SetProjectedWorldVelocity(Vector3 newProjectedWorldVelocity) { _ProjectedWorldVelocity = newProjectedWorldVelocity; }
    public void SetPreProjectedWorldVelocity(Vector3 newPreProjectedWorldVelocity) { _PreProjectedWorldVelocity = newPreProjectedWorldVelocity; }
    public void SetLocalVelocity(Vector3 newLocalVelocity) { _LocalVelocity = newLocalVelocity; }
    public void SetPreLocalVelocity(Vector3 newPreLocalVelocity) { _PreLocalVelocity = newPreLocalVelocity; }
    public void SetAngularVelocity(Vector3 newAngularVelocity) { _AngularVelocity = newAngularVelocity; }
    public void SetUsedMoveVelocity(Vector3 newUsedMoveVelocity) { _UsedMoveVelocity = newUsedMoveVelocity; }
    public void SetSpeedPerFrame(float newSpeed) { _SpeedPerFrame = newSpeed; }
    public void SetIsCollidong(bool newIsColliding) { _IsCollidong = newIsColliding; }

    ///
    private void Awake()
    {
        _CarManager = GetComponent<CarManager>();
    }

    void Start()
    {
        if (CarManager.IsAI)
        {
            _State = new Auto(this);
        }
        else
        {
            _State = new NormalDrive(this);
        }
    }

    void Update()
    {
        _State.Update();

        try
        {
            if (CarManager.CarAudioHandler.Sounds[1] != null)
            {
                float speedpercentage = 1 - (1 - LocalVelocity.z / CarManager.MaxStraigthVelocity);
                CarManager.CarAudioHandler.Sounds[1].volume = Mathf.Lerp(CarManager.CarAudioHandler.EngineMinVol, CarManager.CarAudioHandler.EngineMaxVol, speedpercentage);
                CarManager.CarAudioHandler.Sounds[1].pitch = Mathf.Lerp(CarManager.CarAudioHandler.EngineMinPitch, CarManager.CarAudioHandler.EngineMaxPitch, speedpercentage);
            }
        }
        catch
        {

        }
    }

    void FixedUpdate()
    {
        SetProjections();
        _State.FixedUpdate();
        transform.Rotate(AngularVelocity * Time.deltaTime, Space.World);
        TranslateCar();
        Collision();
        SetPreVelocity();
    }
    private void SetProjections()
    {
        SetTransformProjection(new AxisProjection(Vector3.ProjectOnPlane(transform.forward, CarManager.CarHoverHandler.InterpolatedGroundNormal).normalized,
                                                  Vector3.ProjectOnPlane(transform.right, CarManager.CarHoverHandler.InterpolatedGroundNormal).normalized));

        SetProjectedSplineProjection(new AxisProjection(Vector3.ProjectOnPlane(CarManager.SplineProjection.forward, CarManager.CarHoverHandler.InterpolatedGroundNormal).normalized,
                                                        Vector3.ProjectOnPlane(CarManager.SplineProjection.right, CarManager.CarHoverHandler.InterpolatedGroundNormal).normalized));
    }


    private void SetPreVelocity()
    {
        SetPreProjectedWorldVelocity(ProjectedWorldVelocity);
        SetPreLocalVelocity(LocalVelocity);
    }

    public void SwitchState(IState state)
    {
        _State.OnExit();
        _State = state;
        _State.OnEnter();
        //_Animator.SetInteger("State", (int)_AnimationState);
    }

    private void TranslateCar()
    {
        Vector3[] vertexPositions = _CarManager.ColliderVertices._VertexPositions;
        RaycastHit hit = new RaycastHit();

        float smallestDistance = Mathf.Infinity; // smallest distance from vertex positions
        Vector3 usedVertex = new Vector3();

        float raycastOffset = 4;
        for (int i = 0; i < vertexPositions.Length; i++)
        {
            if (Physics.Raycast(vertexPositions[i] - ProjectedWorldVelocity.normalized * raycastOffset, ProjectedWorldVelocity.normalized, out hit, (ProjectedWorldVelocity.magnitude * Time.fixedDeltaTime) + raycastOffset, _WallMask))
            {
                if (hit.distance < smallestDistance)
                {
                    usedVertex = vertexPositions[i];
                    smallestDistance = hit.distance - raycastOffset;
                }
            }
        }

        if (Physics.Raycast(usedVertex - ProjectedWorldVelocity.normalized * raycastOffset, ProjectedWorldVelocity.normalized, out hit, (ProjectedWorldVelocity.magnitude * Time.fixedDeltaTime) + raycastOffset, _WallMask))
        {
            Vector3 velocity = ProjectedWorldVelocity;
            SetProjectedWorldVelocity(ProjectedWorldVelocity.normalized * smallestDistance * 0.08f);
            SetProjectedWorldVelocity(( Vector3.Reflect(velocity * 0.8f, (hit.normal).normalized)));
            transform.Translate(ProjectedWorldVelocity * Time.fixedDeltaTime, Space.World);
        }
        else
        {
            transform.Translate(ProjectedWorldVelocity * Time.fixedDeltaTime, Space.World);
        }
    }

    private void Collision()
    {

        Vector3 velocity = ProjectedWorldVelocity;
        Vector3 axis = new Vector3();
        float scalar = 0;

        var thisCollider = GetComponent<Collider>();

        Vector3 otherPosition = _WallCollider.gameObject.transform.position;
        Quaternion otherRotation = _WallCollider.gameObject.transform.rotation;

        Physics.ComputePenetration(
                    thisCollider, transform.position, transform.rotation,
                    _WallCollider, otherPosition, otherRotation,
                    out axis, out scalar
                    );

        if (scalar > 0)
        {
            transform.position += axis * scalar;
            Vector3 forwardFriction, sideWaysFriction;
        }

       // print(scalar);
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.cyan;
    //    Gizmos.DrawRay(transform.position, transform.forward * 5);

    //    Gizmos.DrawRay(transform.position, _ProjectedWorldVelocity.normalized * 5);
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawRay(transform.position, Vector3.Cross(TransformProjection.forward, TransformProjection.right).normalized * 2);
    //    Gizmos.color = Color.white;
    //    Gizmos.DrawRay(transform.position, ProjectedSplineProjection.forward * 5);
    //    Gizmos.DrawRay(transform.position, ProjectedSplineProjection.right * 5);

    //    Gizmos.DrawRay(transform.position, _FrictionAxis.forward * 5);
    //    Gizmos.DrawRay(transform.position, _FrictionAxis.right * 5);

    //    Gizmos.color = Color.red;
    //    Gizmos.DrawRay(transform.position, ProjectedWorldVelocity * Time.fixedDeltaTime);

    //    float selfIntersection = 0; //how much the translation vector is intersection with the cars own collider

    //    for (int i = 0; i < _CarManager.ColliderVertices._VertexPositions.Length; i++)
    //    {
    //        if (Vector2.Dot(ProjectedWorldVelocity.normalized, _CarManager.ColliderVertices._VertexPositions[i] - transform.position) >= selfIntersection)
    //        {
    //            selfIntersection = (_CarManager.ColliderVertices._VertexPositions[i] - transform.position).magnitude;

    //        }
    //        Gizmos.color = Color.green;
    //        Gizmos.DrawRay(transform.position, (_CarManager.ColliderVertices._VertexPositions[i] - transform.position));
    //    }

    //    Gizmos.DrawRay(transform.position, ProjectedWorldVelocity.normalized * selfIntersection);

    //    Gizmos.color = Color.red;

    //    _CarManager = GetComponent<CarManager>();
    //    Vector3[] vertexPositions = _CarManager.ColliderVertices._VertexPositions;
    //    float raycastOffset = 4;
    //    for (int i = 0; i < vertexPositions.Length; i++)
    //    {
    //        Gizmos.DrawRay(vertexPositions[i] - (ProjectedWorldVelocity.normalized * raycastOffset), (ProjectedWorldVelocity * Time.fixedDeltaTime) + (ProjectedWorldVelocity.normalized * raycastOffset));
    //    }

    //    Gizmos.DrawRay(transform.position - (ProjectedWorldVelocity.normalized * raycastOffset), (ProjectedWorldVelocity * Time.fixedDeltaTime));

    //    Gizmos.DrawRay(transform.position, (_CarManager.SplineProjection.position - transform.position));


    //}
}

public class NormalDrive : IState
{
    CarBehaviour _Car;

    public NormalDrive(CarBehaviour controller)
    {
        _Car = controller;
    }

    public void OnEnter()
    {
        _Car.CarManager.SetYawTurnRate(_Car.CarManager.BaseYawTurnRate);
        _Car.CarManager.SetSideWaysFriction(_Car.CarManager.BaseSideWaysFriction);
    }

    public void Update()
    {
        if (_Car.CarManager.Inputs._DriftButtonDown)
        {
            _Car.SwitchState(new Drift(_Car));
            //if (_Car.LocalVelocity.z > _Car.CarManager.MaxStraigthVelocity * 0.5f)
            //{
            //    if (_Car.CarManager.Inputs._SteerAxis > 0)
            //    {
            //        _Car.SwitchState(new DriftRight(_Car));
            //    }
            //}      
        }

        try
        {
            _Car.CarManager.CarAudioHandler.SetEngineMaxVol(Mathf.Lerp(_Car.CarManager.CarAudioHandler.EngineMaxVol, 1.2f, Time.deltaTime));
            _Car.CarManager.CarAudioHandler.SetEngineMaxPitch(Mathf.Lerp(_Car.CarManager.CarAudioHandler.EngineMaxPitch, 0.8f, 2 * Time.deltaTime));
        }
        catch { }
    }

    public void FixedUpdate()
    {
        SetYawVelocity();
        SetFrictionAxis();
        CalculateSpeedPerFrame();
        SetVelocity();
    }

    public void OnExit()
    {

    }

    private void SetFrictionAxis()
    {
        if (!_Car.IsCollidong)
        {
            _Car.SetAccelerationAxis(_Car.TransformProjection.forward);
            _Car.SetFrictionAxis(_Car.TransformProjection);
        }
    }

    private void CalculateSpeedPerFrame()
    {
        float speedPerFrame = 0;
        speedPerFrame += _Car.CarManager.Acceleration * _Car.CarManager.Inputs._AccelAxis;
        speedPerFrame -= _Car.CarManager.Acceleration * _Car.CarManager.Inputs._BrakeAxis;
        _Car.SetSpeedPerFrame(speedPerFrame);
    }

    private void SetVelocity()
    {
        ProjectWorldVelocityOnPlane();
        ApplySpeedPerFrame();
        ProjectWorldVelocityOnPlane();
        ProjectLocalVelocity();
        ApplyFriction();
        ProjectLocalVelocity();
    }

    private void ProjectWorldVelocityOnPlane()
    {
        float projectedWorldVelocityMagnitude = _Car.ProjectedWorldVelocity.magnitude;
        Vector3 TempProjectedWorldVelocity = Vector3.ProjectOnPlane(_Car.ProjectedWorldVelocity, _Car.CarManager.CarHoverHandler.InterpolatedGroundNormal).normalized;
        TempProjectedWorldVelocity = TempProjectedWorldVelocity * projectedWorldVelocityMagnitude;

        TempProjectedWorldVelocity = new Vector3(TempProjectedWorldVelocity.x, TempProjectedWorldVelocity.y, TempProjectedWorldVelocity.z);
        _Car.SetProjectedWorldVelocity(TempProjectedWorldVelocity);
    }
    private void ApplySpeedPerFrame()
    {
        if (_Car.LocalVelocity.z < _Car.CarManager.MaxStraigthVelocity && _Car.LocalVelocity.z > -(_Car.CarManager.MaxStraigthVelocity * 0.25f))
        {
            _Car.SetProjectedWorldVelocity(_Car.ProjectedWorldVelocity + (_Car.AccelerationAxis * _Car.SpeedPerFrame * Time.fixedDeltaTime));
        }
    }
    private void ProjectLocalVelocity()
    {
        Vector3 localVelocity = Vector3.zero;
        localVelocity.x = Vector3.Dot(_Car.ProjectedWorldVelocity, _Car.FrictionAxis.right);
        localVelocity.y = Vector3.Dot(_Car.ProjectedWorldVelocity, Vector3.Cross(_Car.FrictionAxis.right, _Car.FrictionAxis.forward));
        localVelocity.z = Vector3.Dot(_Car.ProjectedWorldVelocity, _Car.FrictionAxis.forward);
        _Car.SetLocalVelocity(localVelocity);
    }

    private void ApplyFriction()
    {
        if (_Car.LocalVelocity.z != 0)
        {
            Vector3 TempProjectedWorldVelocity = _Car.ProjectedWorldVelocity;

            TempProjectedWorldVelocity = TempProjectedWorldVelocity - (_Car.CarManager.StraightFriction * _Car.FrictionAxis.forward * Mathf.Sign(_Car.LocalVelocity.z) * Time.fixedDeltaTime);

            Vector3 tempLocalVelocity = Vector3.zero;
            tempLocalVelocity.x = Vector3.Dot(TempProjectedWorldVelocity, _Car.FrictionAxis.right);
            tempLocalVelocity.y = Vector3.Dot(TempProjectedWorldVelocity, Vector3.Cross(_Car.FrictionAxis.right, _Car.FrictionAxis.forward));
            tempLocalVelocity.z = Vector3.Dot(TempProjectedWorldVelocity, _Car.FrictionAxis.forward);

            if (Mathf.Sign(_Car.LocalVelocity.z) != Mathf.Sign(tempLocalVelocity.z))
            {
                _Car.SetProjectedWorldVelocity(_Car.ProjectedWorldVelocity - (_Car.LocalVelocity.z * _Car.FrictionAxis.forward));
            }
            else
            {
                _Car.SetProjectedWorldVelocity(_Car.ProjectedWorldVelocity - (_Car.CarManager.StraightFriction * _Car.FrictionAxis.forward * Mathf.Sign(_Car.LocalVelocity.z) * Time.fixedDeltaTime));
            }
            ProjectLocalVelocity();
        }

        if (_Car.LocalVelocity.x != 0)
        {
            Vector3 TempProjectedWorldVelocity = _Car.ProjectedWorldVelocity;

            TempProjectedWorldVelocity = TempProjectedWorldVelocity - (_Car.CarManager.SideWaysFriction * _Car.FrictionAxis.right * Mathf.Sign(_Car.LocalVelocity.x) * Time.fixedDeltaTime);

            Vector3 tempLocalVelocity = Vector3.zero;
            tempLocalVelocity.x = Vector3.Dot(TempProjectedWorldVelocity, _Car.FrictionAxis.right);
            tempLocalVelocity.y = Vector3.Dot(TempProjectedWorldVelocity, Vector3.Cross(_Car.FrictionAxis.right, _Car.FrictionAxis.forward));
            tempLocalVelocity.z = Vector3.Dot(TempProjectedWorldVelocity, _Car.FrictionAxis.forward);

            if (Mathf.Sign(_Car.LocalVelocity.x) != Mathf.Sign(tempLocalVelocity.x))
            {
                _Car.SetProjectedWorldVelocity(_Car.ProjectedWorldVelocity - (_Car.LocalVelocity.x * _Car.FrictionAxis.right));
            }
            else
            {
                _Car.SetProjectedWorldVelocity(_Car.ProjectedWorldVelocity - (_Car.CarManager.SideWaysFriction * _Car.FrictionAxis.right * Mathf.Sign(_Car.LocalVelocity.x) * Time.fixedDeltaTime));
            }
            ProjectLocalVelocity();
        }
    }

    private void SetYawVelocity()
    {
        Vector3 YawVelocity = new Vector3();

        if (_Car.ProjectedWorldVelocity.magnitude != 0)
        {
            YawVelocity = _Car.transform.up * _Car.CarManager.Inputs._SteerAxis * _Car.CarManager.YawTurnRate;
            _Car.CarManager.ModelTransform.RotateAround(-_Car.transform.forward, _Car.CarManager.Inputs._SteerAxis * _Car.CarManager.BankingRate * Time.fixedDeltaTime);
        }
        _Car.SetAngularVelocity(YawVelocity);
    }
}

public class Drift : IState
{
    CarBehaviour _Car;

    public Drift(CarBehaviour controller)
    {
        _Car = controller;
    }
    public void OnEnter()
    {
        _Car.CarManager.SetYawTurnRate(150);
        _Car.CarManager.SetSideWaysFriction(80000000);
    }
    public void Update()
    {
        if (_Car.CarManager.Inputs._DriftButtonUp)
        {
            _Car.SwitchState(new NormalDrive(_Car));
        }
        try
        {
            _Car.CarManager.CarAudioHandler.SetEngineMaxVol(Mathf.Lerp(_Car.CarManager.CarAudioHandler.EngineMaxVol, 1, Time.deltaTime));
            _Car.CarManager.CarAudioHandler.SetEngineMaxPitch(Mathf.Lerp(_Car.CarManager.CarAudioHandler.EngineMaxPitch, 0.6f, 2 * Time.deltaTime));
        }
        catch { }
    }

    public void FixedUpdate()
    {
        SetYawVelocity();
        SetFrictionAxis();
        CalculateSpeedPerFrame();
        SetVelocity();
    }

    public void OnExit()
    {
        _Car.CarManager.ParticleHandler.SetTilt(0);
        _Car.CarManager.SetSideWaysFriction(150000);
    }

    private void SetFrictionAxis()
    {
        if (!_Car.IsCollidong)
        {
            _Car.SetAccelerationAxis(_Car.TransformProjection.forward);
            _Car.SetFrictionAxis(_Car.ProjectedSplineProjection);
        }
    }

    private void CalculateSpeedPerFrame()
    {
        float speedPerFrame = 0;
        speedPerFrame += _Car.CarManager.Acceleration * _Car.CarManager.Inputs._AccelAxis;
        speedPerFrame -= _Car.CarManager.Acceleration * _Car.CarManager.Inputs._BrakeAxis;
        _Car.SetSpeedPerFrame(speedPerFrame);
    }

    private void SetVelocity()
    {
        ProjectWorldVelocityOnPlane();
        ApplySpeedPerFrame();
        ProjectWorldVelocityOnPlane();
        ProjectLocalVelocity();
        ApplyFriction();
        ProjectLocalVelocity();
    }

    private void ProjectWorldVelocityOnPlane()
    {
        float projectedWorldVelocityMagnitude = _Car.ProjectedWorldVelocity.magnitude;
        Vector3 TempProjectedWorldVelocity = Vector3.ProjectOnPlane(_Car.ProjectedWorldVelocity, _Car.CarManager.CarHoverHandler.InterpolatedGroundNormal).normalized;
        TempProjectedWorldVelocity = TempProjectedWorldVelocity * projectedWorldVelocityMagnitude;

        TempProjectedWorldVelocity = new Vector3(TempProjectedWorldVelocity.x, TempProjectedWorldVelocity.y, TempProjectedWorldVelocity.z);
        _Car.SetProjectedWorldVelocity(TempProjectedWorldVelocity);
    }
    private void ApplySpeedPerFrame()
    {
        if (_Car.LocalVelocity.z < _Car.CarManager.MaxStraigthVelocity && _Car.LocalVelocity.z > -(_Car.CarManager.MaxStraigthVelocity * 0.25f))
        {
            _Car.SetProjectedWorldVelocity(_Car.ProjectedWorldVelocity + (_Car.AccelerationAxis * _Car.SpeedPerFrame * Time.fixedDeltaTime));
        }
    }
    private void ProjectLocalVelocity()
    {
        Vector3 localVelocity = Vector3.zero;
        localVelocity.x = Vector3.Dot(_Car.ProjectedWorldVelocity, _Car.FrictionAxis.right);
        localVelocity.y = Vector3.Dot(_Car.ProjectedWorldVelocity, Vector3.Cross(_Car.FrictionAxis.right, _Car.FrictionAxis.forward));
        localVelocity.z = Vector3.Dot(_Car.ProjectedWorldVelocity, _Car.FrictionAxis.forward);
        _Car.SetLocalVelocity(localVelocity);
    }

    private void ApplyFriction()
    {
        if (_Car.LocalVelocity.z != 0)
        {
            Vector3 TempProjectedWorldVelocity = _Car.ProjectedWorldVelocity;

            TempProjectedWorldVelocity = TempProjectedWorldVelocity - (_Car.CarManager.StraightFriction * _Car.FrictionAxis.forward * Mathf.Sign(_Car.LocalVelocity.z) * Time.fixedDeltaTime);

            Vector3 tempLocalVelocity = Vector3.zero;
            tempLocalVelocity.x = Vector3.Dot(TempProjectedWorldVelocity, _Car.FrictionAxis.right);
            tempLocalVelocity.y = Vector3.Dot(TempProjectedWorldVelocity, Vector3.Cross(_Car.FrictionAxis.right, _Car.FrictionAxis.forward));
            tempLocalVelocity.z = Vector3.Dot(TempProjectedWorldVelocity, _Car.FrictionAxis.forward);

            if (Mathf.Sign(_Car.LocalVelocity.z) != Mathf.Sign(tempLocalVelocity.z))
            {
                _Car.SetProjectedWorldVelocity(_Car.ProjectedWorldVelocity - (_Car.LocalVelocity.z * _Car.FrictionAxis.forward));
            }
            else
            {
                _Car.SetProjectedWorldVelocity(_Car.ProjectedWorldVelocity - (_Car.CarManager.StraightFriction * _Car.FrictionAxis.forward * Mathf.Sign(_Car.LocalVelocity.z) * Time.fixedDeltaTime));
            }
            ProjectLocalVelocity();
        }

        if (_Car.LocalVelocity.x != 0)
        {
            Vector3 TempProjectedWorldVelocity = _Car.ProjectedWorldVelocity;

            TempProjectedWorldVelocity = TempProjectedWorldVelocity - (_Car.CarManager.SideWaysFriction * _Car.FrictionAxis.right * Mathf.Sign(_Car.LocalVelocity.x) * Time.fixedDeltaTime);

            Vector3 tempLocalVelocity = Vector3.zero;
            tempLocalVelocity.x = Vector3.Dot(TempProjectedWorldVelocity, _Car.FrictionAxis.right);
            tempLocalVelocity.y = Vector3.Dot(TempProjectedWorldVelocity, Vector3.Cross(_Car.FrictionAxis.right, _Car.FrictionAxis.forward));
            tempLocalVelocity.z = Vector3.Dot(TempProjectedWorldVelocity, _Car.FrictionAxis.forward);

            if (Mathf.Sign(_Car.LocalVelocity.x) != Mathf.Sign(tempLocalVelocity.x))
            {
                _Car.SetProjectedWorldVelocity(_Car.ProjectedWorldVelocity - (_Car.LocalVelocity.x * _Car.FrictionAxis.right));
            }
            else
            {
                _Car.SetProjectedWorldVelocity(_Car.ProjectedWorldVelocity - (_Car.CarManager.SideWaysFriction * _Car.FrictionAxis.right * Mathf.Sign(_Car.LocalVelocity.x) * Time.fixedDeltaTime));
            }
            ProjectLocalVelocity();
        }
    }

    private void SetYawVelocity()
    {
        Vector3 YawVelocity = new Vector3();
        float tilt = Vector3.Dot(_Car.ProjectedWorldVelocity, _Car.transform.right);
        if (_Car.ProjectedWorldVelocity.magnitude != 0)
        {
            YawVelocity = _Car.transform.up * _Car.CarManager.Inputs._SteerAxis * _Car.CarManager.YawTurnRate;
            //_Car.CarManager.ModelTransform.RotateAround(Vector3.Cross(_Car.transform.up, _Car.ProjectedWorldVelocity).normalized, _Car.CarManager.BankingRate * -0.0015f * _Car.LocalVelocity.z * (1f - Mathf.Abs(Vector3.Dot(_Car.transform.forward.normalized, _Car.ProjectedWorldVelocity.normalized) * 1.8f)) * Time.fixedDeltaTime);
            //_Car.CarManager.ModelTransform.RotateAround(Vector3.Cross(_Car.transform.up, _Car.ProjectedWorldVelocity).normalized, _Car.CarManager.BankingRate * -0.001f * _Car.LocalVelocity.z * Time.fixedDeltaTime);

            _Car.CarManager.ModelTransform.RotateAround(_Car.transform.forward, _Car.CarManager.BankingRate * 0.00075f * tilt * Time.fixedDeltaTime);
            _Car.CarManager.ModelTransform.RotateAround(-_Car.transform.forward, _Car.CarManager.Inputs._SteerAxis * _Car.CarManager.BankingRate * Time.fixedDeltaTime);
        }
        float tiltAngle = Vector3.SignedAngle(_Car.transform.up, _Car.CarManager.ModelTransform.up, _Car.transform.forward);
        _Car.CarManager.ParticleHandler.SetTilt(tiltAngle);

        if (_Car.CarManager.ParticleHandler.IsSparking)
        {
            _Car.CarManager.SetModelPosition(_Car.transform.position + Random.insideUnitSphere * 0.03f);
        }
        else
        {
            _Car.CarManager.SetModelPosition(_Car.transform.position);
        }

        _Car.SetAngularVelocity(YawVelocity);
    }
}

public class Auto : IState
{

    CarBehaviour _Car;

    public Auto(CarBehaviour controller)
    {
        _Car = controller;
    }
    public void OnEnter()
    {

    }
    public void Update()
    {
        _Car.CarManager.SetYawTurnRate(150);
        _Car.CarManager.SetSideWaysFriction(9999999999999999999);

        try
        {
            _Car.CarManager.CarAudioHandler.SetEngineMaxVol(Mathf.Lerp(_Car.CarManager.CarAudioHandler.EngineMaxVol, 0.2f, Time.deltaTime));
            _Car.CarManager.CarAudioHandler.SetEngineMaxPitch(Mathf.Lerp(_Car.CarManager.CarAudioHandler.EngineMaxPitch, 0.6f, 2 * Time.deltaTime));
        }
        catch { }
    }

    public void FixedUpdate()
    {
        SetYawVelocity();
        _Car.SetAccelerationAxis(_Car.TransformProjection.forward);
        _Car.SetFrictionAxis(_Car.ProjectedSplineProjection);
        CalculateSpeedPerFrame();
        SetVelocity();
    }

    public void OnExit()
    {
        _Car.CarManager.ParticleHandler.SetTilt(0);
        _Car.CarManager.SetSideWaysFriction(150000);
    }

    private void CalculateSpeedPerFrame()
    {
        float speedPerFrame = 0;
        speedPerFrame += _Car.CarManager.Acceleration * 1;
        _Car.SetSpeedPerFrame(speedPerFrame);
    }

    private void SetVelocity()
    {
        ProjectWorldVelocityOnPlane();
        ApplySpeedPerFrame();
        ProjectWorldVelocityOnPlane();
        ProjectLocalVelocity();
        ApplyFriction();
        ProjectLocalVelocity();
    }

    private void ProjectWorldVelocityOnPlane()
    {
        float projectedWorldVelocityMagnitude = _Car.ProjectedWorldVelocity.magnitude;
        Vector3 TempProjectedWorldVelocity = Vector3.ProjectOnPlane(_Car.ProjectedWorldVelocity, _Car.CarManager.CarHoverHandler.InterpolatedGroundNormal).normalized;
        TempProjectedWorldVelocity = TempProjectedWorldVelocity * projectedWorldVelocityMagnitude;

        TempProjectedWorldVelocity = new Vector3(TempProjectedWorldVelocity.x, TempProjectedWorldVelocity.y, TempProjectedWorldVelocity.z);
        _Car.SetProjectedWorldVelocity(TempProjectedWorldVelocity);
    }
    private void ApplySpeedPerFrame()
    {
        if (_Car.LocalVelocity.z < _Car.CarManager.MaxStraigthVelocity && _Car.LocalVelocity.z > -(_Car.CarManager.MaxStraigthVelocity * 0.25f))
        {
            _Car.SetProjectedWorldVelocity(_Car.ProjectedWorldVelocity + (_Car.AccelerationAxis * _Car.SpeedPerFrame * Time.fixedDeltaTime));
        }
    }
    private void ProjectLocalVelocity()
    {
        Vector3 localVelocity = Vector3.zero;
        localVelocity.x = Vector3.Dot(_Car.ProjectedWorldVelocity, _Car.FrictionAxis.right);
        localVelocity.y = Vector3.Dot(_Car.ProjectedWorldVelocity, Vector3.Cross(_Car.FrictionAxis.right, _Car.FrictionAxis.forward));
        localVelocity.z = Vector3.Dot(_Car.ProjectedWorldVelocity, _Car.FrictionAxis.forward);
        _Car.SetLocalVelocity(localVelocity);
    }

    private void ApplyFriction()
    {
        if (_Car.LocalVelocity.z != 0)
        {
            Vector3 TempProjectedWorldVelocity = _Car.ProjectedWorldVelocity;

            TempProjectedWorldVelocity = TempProjectedWorldVelocity - (_Car.CarManager.StraightFriction * _Car.FrictionAxis.forward * Mathf.Sign(_Car.LocalVelocity.z) * Time.fixedDeltaTime);

            Vector3 tempLocalVelocity = Vector3.zero;
            tempLocalVelocity.x = Vector3.Dot(TempProjectedWorldVelocity, _Car.FrictionAxis.right);
            tempLocalVelocity.y = Vector3.Dot(TempProjectedWorldVelocity, Vector3.Cross(_Car.FrictionAxis.right, _Car.FrictionAxis.forward));
            tempLocalVelocity.z = Vector3.Dot(TempProjectedWorldVelocity, _Car.FrictionAxis.forward);

            if (Mathf.Sign(_Car.LocalVelocity.z) != Mathf.Sign(tempLocalVelocity.z))
            {
                _Car.SetProjectedWorldVelocity(_Car.ProjectedWorldVelocity - (_Car.LocalVelocity.z * _Car.FrictionAxis.forward));
            }
            else
            {
                _Car.SetProjectedWorldVelocity(_Car.ProjectedWorldVelocity - (_Car.CarManager.StraightFriction * _Car.FrictionAxis.forward * Mathf.Sign(_Car.LocalVelocity.z) * Time.fixedDeltaTime));
            }
            ProjectLocalVelocity();
        }

        if (_Car.LocalVelocity.x != 0)
        {
            Vector3 TempProjectedWorldVelocity = _Car.ProjectedWorldVelocity;

            TempProjectedWorldVelocity = TempProjectedWorldVelocity - (_Car.CarManager.SideWaysFriction * _Car.FrictionAxis.right * Mathf.Sign(_Car.LocalVelocity.x) * Time.fixedDeltaTime);

            Vector3 tempLocalVelocity = Vector3.zero;
            tempLocalVelocity.x = Vector3.Dot(TempProjectedWorldVelocity, _Car.FrictionAxis.right);
            tempLocalVelocity.y = Vector3.Dot(TempProjectedWorldVelocity, Vector3.Cross(_Car.FrictionAxis.right, _Car.FrictionAxis.forward));
            tempLocalVelocity.z = Vector3.Dot(TempProjectedWorldVelocity, _Car.FrictionAxis.forward);

            if (Mathf.Sign(_Car.LocalVelocity.x) != Mathf.Sign(tempLocalVelocity.x))
            {
                _Car.SetProjectedWorldVelocity(_Car.ProjectedWorldVelocity - (_Car.LocalVelocity.x * _Car.FrictionAxis.right));
            }
            else
            {
                _Car.SetProjectedWorldVelocity(_Car.ProjectedWorldVelocity - (_Car.CarManager.SideWaysFriction * _Car.FrictionAxis.right * Mathf.Sign(_Car.LocalVelocity.x) * Time.fixedDeltaTime));
            }
            ProjectLocalVelocity();
        }
    }

    private void SetYawVelocity()
    {
        Vector3 strifeVector = new Vector3();
        Ray ray = new Ray(_Car.transform.position + (_Car.CarManager.RaycastYOffset * _Car.transform.up), -_Car.transform.up);
        RaycastHit hitInfo;
        Vector3 hitPoint = new Vector3();
        if (Physics.Raycast(ray, out hitInfo, _Car.CarManager.MaxGroundDistence + _Car.CarManager.RaycastYOffset, _Car.CarManager.GroundMask))
        {
            hitPoint = hitInfo.point;
        }

        strifeVector = _Car.CarManager.SplineProjection.position - _Car.transform.position;
        _Car.transform.rotation = Quaternion.FromToRotation(_Car.transform.forward, _Car.ProjectedSplineProjection.forward) * _Car.transform.rotation;
        _Car.transform.position = Vector3.Lerp(_Car.transform.position, _Car.CarManager.SplineProjection.position, Time.fixedDeltaTime);
    }
}

public class State : IState
{
    CarBehaviour _Car;

    public State(CarBehaviour controller)
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







