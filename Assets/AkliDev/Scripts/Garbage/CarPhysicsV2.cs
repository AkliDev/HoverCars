using UnityEngine;

public class CarPhysicsV2 : MonoBehaviour
{
    private BoxCollider _BoxCollider;

    private GetVertexPositionsOfBoxCollider _VertexPositionsOfBoxCollider;
    public Vector3[] _VertexPositions;

    private Vector3 _RaycastPosition;

    [SerializeField]
    public float _RaycastDistence;

    [SerializeField]
    public Vector3 _Normal, _PreNormal, _SurfaceNormal;

    [SerializeField]
    private float _CompressionRatio, _PreCompressionRatio;

    [SerializeField]
    private float _SuspensionForceMultipleir, _SuspensionFrictionMultiplier, _GravityMultiplier;
    [SerializeField]
    private bool _Grounded, _EngineIsOn;
    [SerializeField]
    private Vector3 _SuspensionGravityVelocity, _GravityForceDirection, _SuspensionForceDirection;
    [SerializeField]


    public Transform _Model;


    [SerializeField]
    private Mesh _Mesh;




    void Start()
    {
        _BoxCollider = GetComponent<BoxCollider>();
        _VertexPositionsOfBoxCollider = GetComponent<GetVertexPositionsOfBoxCollider>();

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _EngineIsOn ^= true;
        }

    }
    private void FixedUpdate()
    {
        Suspension();
        Gravity();
        ElaborateMovement();
    }
    private void Gravity()
    {
        _SuspensionGravityVelocity -= _GravityForceDirection * Time.fixedDeltaTime;
    }
    private void ElaborateMovement()
    {
        transform.Translate(_SuspensionGravityVelocity * Time.fixedDeltaTime, Space.World);
    }
    private void Suspension()
    {
        RaycastInterpolatedNormal();

        GroundCheck();

        RotateObjectToNormal(_SurfaceNormal);

        _GravityForceDirection = CalculateGravityDirection(_SurfaceNormal);

        if (_EngineIsOn)
        {
            _SuspensionForceDirection = CalculateSuspensionForceDirection(_SurfaceNormal);
        }
        else
        {
            _SuspensionForceDirection = _GravityForceDirection + (_GravityForceDirection * 0.01f);
        }

        if (_CompressionRatio == 1 && _CompressionRatio != _PreCompressionRatio)
        {
            _SuspensionGravityVelocity = Vector3.zero;
        }

        ElaborateSuspensionForce(_CompressionRatio);

        if (_Grounded && _EngineIsOn)
        {
            FrictionDuringSuspension();
        }

        _PreCompressionRatio = _CompressionRatio;
    }

    private void RaycastInterpolatedNormal()
    {
        _VertexPositions = _VertexPositionsOfBoxCollider._VertexPositions;

        float yOffset = transform.lossyScale.y;

        _RaycastPosition = ((_VertexPositions[2] + _VertexPositions[3] + _VertexPositions[6] + _VertexPositions[7]) * 0.25f);

        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(_RaycastPosition + (transform.up * yOffset), -transform.up, out hit, (_RaycastDistence + yOffset)))
        {

            if (_Mesh != hit.collider.gameObject.GetComponent<MeshCollider>().sharedMesh)
            {
                _Mesh = hit.collider.gameObject.GetComponent<MeshCollider>().sharedMesh;              
            }
            Vector3[] normals = _Mesh.normals;
            int[] triangles = _Mesh.triangles;
            Vector3 n0 = normals[triangles[hit.triangleIndex * 3 + 0]];
            Vector3 n1 = normals[triangles[hit.triangleIndex * 3 + 1]];
            Vector3 n2 = normals[triangles[hit.triangleIndex * 3 + 2]];
            Vector3 baryCenter = hit.barycentricCoordinate;
            Vector3 interpolatedNormal = n0 * baryCenter.x + n1 * baryCenter.y + n2 * baryCenter.z;
            interpolatedNormal = interpolatedNormal.normalized;
            Transform hitTransform = hit.collider.transform;
            interpolatedNormal = hitTransform.TransformDirection(interpolatedNormal);

            _CompressionRatio = CalculateCompressionRatio(hit.distance - yOffset);
            _Normal = interpolatedNormal;
            _PreNormal = _SurfaceNormal;
            _SurfaceNormal = hit.normal;
           
        }
        else
        {
            _CompressionRatio = 0;
        }
    }
    private void GroundCheck()
    {
        if (_CompressionRatio > 0)
        {
            _Grounded = true;
        }
        else
        {
            _Grounded = false;
        }

    }

    private void RotateObjectToNormal(Vector3 hitNormal)
    {
        transform.rotation = Quaternion.FromToRotation(transform.up, (_SurfaceNormal)) * transform.rotation;
        _Model.rotation = Quaternion.FromToRotation(transform.up, (_Normal)) * transform.rotation;


    }
    private float CalculateCompressionRatio(float hitDistance)
    {
        float absoluteValeu = 1;
        if (hitDistance > 0)
        {
            float compressionPercentege = (hitDistance / _RaycastDistence);
            return absoluteValeu - compressionPercentege;
        }
        return 1;
    }
    private Vector3 CalculateGravityDirection(Vector3 hitNormal)
    {
        Vector3 gravityDirection = hitNormal * _GravityMultiplier;
        return gravityDirection;
    }
    private Vector3 CalculateSuspensionForceDirection(Vector3 hitNormal)
    {
        Vector3 suspensionForceDirection = hitNormal * _SuspensionForceMultipleir;
        return suspensionForceDirection;
    }
    private void ElaborateSuspensionForce(float compressionRatio)
    {
        _SuspensionGravityVelocity += _SuspensionForceDirection * compressionRatio * Time.fixedDeltaTime;
    }
    private void FrictionDuringSuspension()
    {
        _SuspensionGravityVelocity -= _SuspensionFrictionMultiplier * _SuspensionGravityVelocity.normalized * Time.fixedDeltaTime;
    }

   
    
}
