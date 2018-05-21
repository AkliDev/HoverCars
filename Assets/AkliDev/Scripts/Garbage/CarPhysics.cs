using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPhysics : MonoBehaviour
{
    private BoxCollider _BoxCollider;

    private GetVertexPositionsOfBoxCollider _VertexPositionsOfBoxCollider;
    public Vector3[] _VertexPositions;

    [SerializeField]
    public float _RaycastDistence,_RaycastYoffset;

    [SerializeField]
    public Vector3[] _SurviceNormals, _PreSurviceNormals;
    [SerializeField]
    public Vector3 _CombinedSurviceNormal, _InterpolatedNormal;

    [SerializeField]
    private float[] _CompressionRatios;
    [SerializeField]
    private float _CombinedCompressionRatio, _HighestCompressionRatio, _PreHighestCompressionRatio;

    [SerializeField]
    private float _SuspensionForceMultipleir, _SuspensionFrictionMultiplier, _GravityMultiplier;
    [SerializeField]
    private bool _Grounded, _EngineIsOn;
    [SerializeField]
    private Vector3 _SuspensionGravityVelocity, _GravityForceDirection, _SuspensionForceDirection;


    private Mesh _Mesh;
    Vector3[] _Normals;
    int[] _Triangles;


    public Transform _Model,_UpOrientation;
    void Start()
    {
        _BoxCollider = GetComponent<BoxCollider>();

        _VertexPositionsOfBoxCollider = GetComponent<GetVertexPositionsOfBoxCollider>();
        _CompressionRatios = new float[4];
        _SurviceNormals = new Vector3[_CompressionRatios.Length];
        _PreSurviceNormals = new Vector3[_SurviceNormals.Length];
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
        transform.Translate(_SuspensionGravityVelocity *Time.fixedDeltaTime, Space.World);
    }
    private void Suspension()
    {
        RaycastInformarionAtDesirablePosition();
        RaycastInterpolatedNormal();

        GroundCheck();
        if (_Grounded)
        {
            _CombinedSurviceNormal = ReturnCombinedSurviceNormal(_SurviceNormals, _PreSurviceNormals);
        }

        RotateObjectToNormal(_InterpolatedNormal);

        _GravityForceDirection = CalculateGravityDirection(_InterpolatedNormal);
        if (_EngineIsOn)
        {
            //_SuspensionForceDirection = _GravityForceDirection + (_GravityForceDirection * 0.02f);
            _SuspensionForceDirection = CalculateSuspensionForceDirection(_InterpolatedNormal);
        }
        else
        {
            _SuspensionForceDirection = _GravityForceDirection + (_GravityForceDirection * 0.01f);
        }

        _HighestCompressionRatio = ReturnHighestCompressionRatio(_CompressionRatios);
        _CombinedCompressionRatio = ReturnCombinedCompressionRatio(_CompressionRatios);

        //if (_HighestCompressionRatio == 1 && _HighestCompressionRatio != _PreHighestCompressionRatio)
        //{
        //    _SuspensionGravityVelocity = Vector3.zero;
        //}

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
    void OnDrawGizmosSelected()
    {
        _VertexPositions = _VertexPositionsOfBoxCollider._VertexPositions;

        float xOffset = 0.2f;
        float yOffset = transform.lossyScale.y * _RaycastYoffset;
        float zOffset = 0.2f;

        Vector3 _AveregeRaycastPosition = ((_VertexPositions[2] + _VertexPositions[3] + _VertexPositions[6] + _VertexPositions[7]) * 0.25f);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(_VertexPositions[2] + ((-transform.right * xOffset) + (transform.up * yOffset) + (-transform.forward * zOffset)), -transform.up * (_RaycastDistence + yOffset));
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(_VertexPositions[3] + ((transform.right * xOffset) + (transform.up * yOffset) + (-transform.forward * zOffset)), -transform.up * (_RaycastDistence + yOffset));
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(_VertexPositions[6] + ((-transform.right * xOffset) + (transform.up * yOffset) + (transform.forward * zOffset)), -transform.up * (_RaycastDistence + yOffset));
        Gizmos.color = Color.green;
        Gizmos.DrawRay(_VertexPositions[7] + ((transform.right * xOffset) + (transform.up * yOffset) + (transform.forward * zOffset)), -transform.up * (_RaycastDistence + yOffset));
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(_AveregeRaycastPosition + (transform.up * yOffset), -transform.up* (_RaycastDistence + yOffset));
    }
    private void RaycastInformarionAtDesirablePosition()
    {
        _VertexPositions = _VertexPositionsOfBoxCollider._VertexPositions;

        float xOffset = 0.2f;
        float yOffset = transform.lossyScale.y * _RaycastYoffset;
        float zOffset = 0.2f;

        RaycastHit[] hits = new RaycastHit[_CompressionRatios.Length];

      
        if (Physics.Raycast(_VertexPositions[2] + ((-transform.right * xOffset) + (transform.up * yOffset) + (-transform.forward * zOffset)), -transform.up, out hits[0], (_RaycastDistence + yOffset)))
        {
            _CompressionRatios[0] = CalculateCompressionRatio(hits[0].distance - yOffset);
            _SurviceNormals[0] = hits[0].normal;
            _PreSurviceNormals[0] = _SurviceNormals[0];
        }
        else
        {
            _CompressionRatios[0] = 0;
            _SurviceNormals[0] = Vector3.zero;
        }

      
        if (Physics.Raycast(_VertexPositions[3] + ((transform.right * xOffset) + (transform.up * yOffset) + (-transform.forward * zOffset)), -transform.up, out hits[1], (_RaycastDistence + yOffset)))
        {
            _CompressionRatios[1] = CalculateCompressionRatio(hits[1].distance - yOffset);
            _SurviceNormals[1] = hits[1].normal;
            _PreSurviceNormals[1] = _SurviceNormals[1];
        }
        else
        {
            _CompressionRatios[1] = 0;
            _SurviceNormals[1] = Vector3.zero;
        }

      
        if (Physics.Raycast(_VertexPositions[6] + ((-transform.right * xOffset) + (transform.up * yOffset) + (transform.forward * zOffset)), -transform.up, out hits[2], (_RaycastDistence + yOffset)))
        {
            _CompressionRatios[2] = CalculateCompressionRatio(hits[2].distance - yOffset);
            _SurviceNormals[2] = hits[2].normal;
            _PreSurviceNormals[2] = _SurviceNormals[2];
        }
        else
        {
            _CompressionRatios[2] = 0;
            _SurviceNormals[2] = Vector3.zero;
        }

      
        if (Physics.Raycast(_VertexPositions[7] + ((transform.right * xOffset) + (transform.up * yOffset) + (transform.forward * zOffset)), -transform.up, out hits[3], (_RaycastDistence + yOffset)))
        {
            _CompressionRatios[3] = CalculateCompressionRatio(hits[3].distance - yOffset);
            _SurviceNormals[3] = hits[3].normal;
            _PreSurviceNormals[3] = _SurviceNormals[3];
        }
        else
        {
            _CompressionRatios[3] = 0;
            _SurviceNormals[3] = Vector3.zero;
        }
    }
    private void RaycastInterpolatedNormal()
    {
        _VertexPositions = _VertexPositionsOfBoxCollider._VertexPositions;

        float yOffset = transform.lossyScale.y * _RaycastYoffset; ;

        Vector3 _RaycastPosition = ((_VertexPositions[2] + _VertexPositions[3] + _VertexPositions[6] + _VertexPositions[7]) * 0.25f);

        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(_RaycastPosition + (transform.up * yOffset), -transform.up, out hit, (_RaycastDistence + yOffset)))
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
   
    private void GroundCheck()
    {
        if (_CompressionRatios[0] > 0)
        { _Grounded = true; }
        else if (_CompressionRatios[1] > 0)
        { _Grounded = true; }
        else if (_CompressionRatios[2] > 0)
        { _Grounded = true; }
        else if (_CompressionRatios[3] > 0)
        { _Grounded = true; }
        else
        { _Grounded = false; }
    }
    private Vector3 ReturnCombinedSurviceNormal(Vector3[] surviceNormal, Vector3[] preSurviceNormals)
    {
        Vector3 combinedSurviceNormal = new Vector3();
        int divideAmount = 0;

        for (int i = 0; i < surviceNormal.Length; i++)
        {
            if (surviceNormal[i] != Vector3.zero)
            {
                combinedSurviceNormal += surviceNormal[i];
                divideAmount++;
            }
        }
        if (combinedSurviceNormal != Vector3.zero)
        {
            combinedSurviceNormal = combinedSurviceNormal / divideAmount;
        }
        return combinedSurviceNormal;
    }
    private void RotateObjectToNormal(Vector3 combinedSurviceNormal) 
    {
        transform.rotation = Quaternion.FromToRotation(transform.up, (_InterpolatedNormal)) * transform.rotation;
        _Model.rotation = Quaternion.Slerp(_Model.rotation, transform.rotation, 20 * Time.fixedDeltaTime);
        _UpOrientation.rotation = Quaternion.Slerp (_UpOrientation.rotation, transform.rotation, 7 * Time.fixedDeltaTime);
        //_Model.rotation = transform.rotation;
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
    private float ReturnHighestCompressionRatio(float[] compressionRatios)
    {
        float highestCompressionRatio = 0;

        for (int i = 0; i < compressionRatios.Length; i++)
        {
            if (compressionRatios[i] >= highestCompressionRatio)
            {
                highestCompressionRatio = compressionRatios[i];
            }
        }
        return highestCompressionRatio;
    }
    private float ReturnCombinedCompressionRatio(float[] compressionRatios)
    {
        float combinedCompressionRatio = 0;
        int divideAmount = 0;

        for (int i = 0; i < compressionRatios.Length; i++)
        {
           if (compressionRatios[i] > 0)
           {
                combinedCompressionRatio += compressionRatios[i];
                divideAmount++;
            }

        }
        if (combinedCompressionRatio > 0)
        {
            combinedCompressionRatio = combinedCompressionRatio / divideAmount;
        }
        return combinedCompressionRatio;
    }
    private Vector3 CalculateGravityDirection(Vector3 combinedNormal )
    {
        Vector3 gravityDirection = combinedNormal * _GravityMultiplier;
        return gravityDirection;
    }
    private Vector3 CalculateSuspensionForceDirection(Vector3 combinedNormal)
    {
        Vector3 suspensionForceDirection = combinedNormal * _SuspensionForceMultipleir;
        return suspensionForceDirection;
    }
    private void ElaborateSuspensionForce(float combinedCompressionRatio)
    {
        _SuspensionGravityVelocity += _SuspensionForceDirection * combinedCompressionRatio * Time.fixedDeltaTime;
    }
    private void FrictionDuringSuspension()
    {
        _SuspensionGravityVelocity -= _SuspensionFrictionMultiplier * _SuspensionGravityVelocity.normalized * Time.fixedDeltaTime;
    }
}
