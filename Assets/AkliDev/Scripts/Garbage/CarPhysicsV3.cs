using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPhysicsV3 : MonoBehaviour {

    private CarManagerOld _Manager;

    private Vector3[] _SurviceNormals, _PreSurviceNormals;
    private Vector3 _CombinedSurviceNormal, _InterpolatedNormal;

    private float[] _CompressionRatios;
    private float _CombinedCompressionRatio, _HighestCompressionRatio, _PreHighestCompressionRatio;

    private Vector3 _SuspensionGravityVelocity, _SuspensionForceDirection, _GravityForceDirection;

    private bool _Grounded, _EngineIsOn;

    private Vector3[] _VertexPositions;

    private Mesh _Mesh;
    Vector3[] _Normals;
    int[] _Triangles;


    public CarManagerOld GetManager { get { return _Manager; } }
    public Vector3[] GetSurviceNormals { get { return _SurviceNormals; } }
    public Vector3 GetCombinedSurviceNormal { get { return _CombinedSurviceNormal; } }
    public Vector3 GetInterpolatedNormal { get { return _InterpolatedNormal; } }
    public bool GetGrounded { get { return _Grounded; } }

    private void Awake()
    {
        _Manager = GetComponent<CarManagerOld>();
    }
    void Start()
    {     
        _CompressionRatios = new float[4];
        _SurviceNormals = new Vector3[_CompressionRatios.Length];
        _PreSurviceNormals = new Vector3[_SurviceNormals.Length];
        _VertexPositions = new Vector3[8];
        _EngineIsOn = true;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            _EngineIsOn ^= true;
        }
    }
    private void FixedUpdate()
    {
        _VertexPositions = GlobalCarCalculations.CalculateVertexPositonsOfBoxCollider(GetManager.GetBoxCollider, 0);
        Suspension();
        Gravity();
        GetManager.TranslateTransform(_SuspensionGravityVelocity, Space.World);
        
    }
    private void Gravity()
    {
        _SuspensionGravityVelocity -= _GravityForceDirection * Time.fixedDeltaTime;
    }
   
    private void Suspension()
    {
        RaycastInformarionAtDesirablePosition();
       
        _Grounded = GlobalCarCalculations.GroundCheck(_CompressionRatios);

        if (_Grounded)
        {
            _CombinedSurviceNormal = GlobalCarCalculations.ReturnAverageSurviceNormal(_SurviceNormals);
        }

        RotateObjectToNormal(_InterpolatedNormal);

        _GravityForceDirection = GlobalCarCalculations.CalculateGravity(_InterpolatedNormal, GetManager.GetGravityMultiplier);
        
        if (_EngineIsOn)
        {
            _SuspensionForceDirection = GlobalCarCalculations.CalculateSuspensionForceDirection(_InterpolatedNormal, GetManager.GetSuspensionForceMultipleir);
        }
        else
        {
            _SuspensionForceDirection = _GravityForceDirection + (_GravityForceDirection * 0.01f);
        }

        _HighestCompressionRatio = GlobalCarCalculations.ReturnHighestCompressionRatio(_CompressionRatios);
        _CombinedCompressionRatio = GlobalCarCalculations.ReturnAverageCompressionRatio(_CompressionRatios);

        if (_HighestCompressionRatio == 1 && Vector3.Dot(_SuspensionGravityVelocity, _InterpolatedNormal) < 0)
        {
            _SuspensionGravityVelocity = Vector3.zero;
        }

        ElaborateSuspensionForce(_CombinedCompressionRatio); //Interchangeable with "_HighestCompressionRatio" and "_CombinedCompressionRatio"

        if (_Grounded && _EngineIsOn)
        {
            FrictionDuringSuspension();
        }

        _PreHighestCompressionRatio = _HighestCompressionRatio;
    }
    
    private void RaycastInformarionAtDesirablePosition()
    {
        float xOffset = 0.2f;
        float yOffset = transform.lossyScale.y * GetManager.GetRaycastYoffset;
        float zOffset = 0.2f;

        RaycastHit[] hits = new RaycastHit[_CompressionRatios.Length];
        
        if (Physics.Raycast(_VertexPositions[2] + ((-transform.right * xOffset) + (transform.up * yOffset) + (-transform.forward * zOffset)), -transform.up, out hits[0], (GetManager.GetRaycastDistence + yOffset)))
        {
            _CompressionRatios[0] = GlobalCarCalculations.CalculateCompressionRatio(hits[0].distance - yOffset, GetManager.GetRaycastDistence);
            _SurviceNormals[0] = hits[0].normal;
            _PreSurviceNormals[0] = _SurviceNormals[0];
        }
        else
        {
            _CompressionRatios[0] = 0;
            _SurviceNormals[0] = Vector3.zero;
        }

        if (Physics.Raycast(_VertexPositions[3] + ((transform.right * xOffset) + (transform.up * yOffset) + (-transform.forward * zOffset)), -transform.up, out hits[1], (GetManager.GetRaycastDistence + yOffset)))
        {
            _CompressionRatios[1] = GlobalCarCalculations.CalculateCompressionRatio(hits[1].distance - yOffset, GetManager.GetRaycastDistence);
            _SurviceNormals[1] = hits[1].normal;
            _PreSurviceNormals[1] = _SurviceNormals[1];
        }
        else
        {
            _CompressionRatios[1] = 0;
            _SurviceNormals[1] = Vector3.zero;
        }

        if (Physics.Raycast(_VertexPositions[6] + ((-transform.right * xOffset) + (transform.up * yOffset) + (transform.forward * zOffset)), -transform.up, out hits[2], (GetManager.GetRaycastDistence + yOffset)))
        {
            _CompressionRatios[2] = GlobalCarCalculations.CalculateCompressionRatio(hits[2].distance - yOffset, GetManager.GetRaycastDistence);
            _SurviceNormals[2] = hits[2].normal;
            _PreSurviceNormals[2] = _SurviceNormals[2];
        }
        else
        {
            _CompressionRatios[2] = 0;
            _SurviceNormals[2] = Vector3.zero;
        }

        if (Physics.Raycast(_VertexPositions[7] + ((transform.right * xOffset) + (transform.up * yOffset) + (transform.forward * zOffset)), -transform.up, out hits[3], (GetManager.GetRaycastDistence + yOffset)))
        {
            _CompressionRatios[3] = GlobalCarCalculations.CalculateCompressionRatio(hits[3].distance - yOffset, GetManager.GetRaycastDistence);
            _SurviceNormals[3] = hits[3].normal;
            _PreSurviceNormals[3] = _SurviceNormals[3];
        }
        else
        {
            _CompressionRatios[3] = 0;
            _SurviceNormals[3] = Vector3.zero;
        }

        ///
        Vector3 _RaycastPosition = ((_VertexPositions[2] + _VertexPositions[3] + _VertexPositions[6] + _VertexPositions[7]) * 0.25f);

        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(_RaycastPosition + (transform.up * yOffset), -transform.up, out hit, (GetManager.GetRaycastDistence + yOffset)))
        {

            if (_Mesh != hit.collider.gameObject.GetComponent<MeshCollider>().sharedMesh)
            {
                _Mesh = hit.collider.gameObject.GetComponent<MeshCollider>().sharedMesh;
                _Normals = _Mesh.normals;
                _Triangles = _Mesh.triangles;
            }

            Vector3 n0 = _Normals[_Triangles[hit.triangleIndex * 3 + 0]];
            Vector3 n1 = _Normals[_Triangles[hit.triangleIndex * 3 + 1]];
            Vector3 n2 = _Normals[_Triangles[hit.triangleIndex * 3 + 2]];
            Vector3 baryCenter = hit.barycentricCoordinate;
            Vector3 interpolatedNormal = n0 * baryCenter.x + n1 * baryCenter.y + n2 * baryCenter.z;
            interpolatedNormal = interpolatedNormal.normalized;
            Transform hitTransform = hit.collider.transform;
            interpolatedNormal = hitTransform.TransformDirection(interpolatedNormal);


            _InterpolatedNormal = interpolatedNormal;
        }
    }
    
    private void RotateObjectToNormal(Vector3 combinedSurviceNormal)
    {
        transform.rotation = Quaternion.FromToRotation(transform.up, (_InterpolatedNormal)) * transform.rotation;
        GetManager.SetModelRotation( Quaternion.Slerp(GetManager.GetModelTransform.rotation, transform.rotation, 20 * Time.fixedDeltaTime));
        GetManager.SetUpOrientationRotation( Quaternion.Slerp(GetManager.GetOrientationLookAtTransform.rotation, transform.rotation, 7 * Time.fixedDeltaTime));    
    }

    private void ElaborateSuspensionForce(float combinedCompressionRatio)
    {
        _SuspensionGravityVelocity += _SuspensionForceDirection * combinedCompressionRatio * Time.fixedDeltaTime;
    }
    private void FrictionDuringSuspension()
    {
        _SuspensionGravityVelocity -= GetManager.GetSuspensionFrictionMultiplier * _SuspensionGravityVelocity.normalized * Time.fixedDeltaTime;
    }
    
}
