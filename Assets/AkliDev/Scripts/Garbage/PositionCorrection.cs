using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionCorrection : MonoBehaviour
{
    [SerializeField]
    private LayerMask _LayerMask;
    private float _DistanceFromGround;
    private CarPhysics _Physics;

    public Transform _TheNormRotation,TheHitNorm;

    [SerializeField]
    private Mesh _HitMesh;

    private Vector3[] _Verts;


    [SerializeField]
    private int _HitTri;

    [SerializeField]
    private int _TriVertIndex1, _TriVertIndex2, _TriVertIndex3;
    [SerializeField]
    float _Angle,_CrossAngle, FaceAngle, _Yuler;
   


    private void Start()
    {
        _Physics = GetComponent<CarPhysics>();
        _HitMesh = new Mesh();
    }

    private void Update()
    {
        TheHitNorm.position = transform.position;
        _TheNormRotation.position = transform.position;
        //_TheNormRotation.eulerAngles = new Vector3(transform.localEulerAngles.x, _TheNormRotation.localEulerAngles.y,transform.localEulerAngles.z);
       _TheNormRotation.rotation = Quaternion.FromToRotation(_TheNormRotation.up, (transform.up)) * _TheNormRotation.rotation;
        _Yuler = transform.localEulerAngles.y;
    }

    float SignedAngleBetween(Vector3 a, Vector3 b, Vector3 n)
    {
       
        float angle = Vector3.Angle(a, b);
        float sign = Mathf.Sign(Vector3.Dot(n, Vector3.Cross(a, b)));

       
        float signed_angle = angle * sign;

       
        return signed_angle;
    }
    void OnDrawGizmosSelected()
    {
       
        Gizmos.color = Color.red;

        //_TheNormRotation.position = transform.position;
        //_TheNormRotation.up = transform.up;
        
 

       RaycastHit hit; 
       float raycastDistance = 100;

        Vector3 positionOnNextFrame = transform.position + (transform.forward * raycastDistance);
        Vector3 currentDirection =   positionOnNextFrame - transform.position;
        currentDirection = currentDirection.normalized;

        _Angle = Vector3.Angle( _TheNormRotation.forward , currentDirection);

        _Angle = SignedAngleBetween(_TheNormRotation.forward, currentDirection,transform.up);
        

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, _TheNormRotation.forward * raycastDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, _TheNormRotation.right * raycastDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, _TheNormRotation.up * raycastDistance);

       
        Gizmos.color = Color.magenta;
        //Gizmos.DrawRay(transform.position, currentDirection * raycastDistance);     

        if (Physics.Raycast(transform.position, currentDirection, out hit, raycastDistance, _LayerMask))
        {


            if (_HitMesh != hit.collider.gameObject.GetComponent<MeshCollider>().sharedMesh)
            {
                _HitMesh = hit.collider.gameObject.GetComponent<MeshCollider>().sharedMesh;
                _Verts = _HitMesh.vertices;
            }

            _HitTri = hit.triangleIndex;

            int index = hit.triangleIndex * 3;

            _TriVertIndex1 = _HitMesh.triangles[index];
            _TriVertIndex2 = _HitMesh.triangles[index + 1];
            _TriVertIndex3 = _HitMesh.triangles[index + 2];


            Gizmos.color = Color.red;
            Gizmos.DrawSphere(hit.transform.TransformPoint(_Verts[_TriVertIndex1]), 0.5f);
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(hit.transform.TransformPoint(_Verts[_TriVertIndex2]), 0.5f);
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(hit.transform.TransformPoint(_Verts[_TriVertIndex3]), 0.5f);

            Vector3 Rdir = new Vector3();
        

            if (_HitTri % 2 == 0)
            {
                Rdir = hit.transform.TransformPoint(_Verts[_TriVertIndex2]) - hit.transform.TransformPoint(_Verts[_TriVertIndex1]);
            }
            else
            {
                Rdir = hit.transform.TransformPoint(_Verts[_TriVertIndex3]) - hit.transform.TransformPoint(_Verts[_TriVertIndex1]);
            }

            Rdir = Rdir.normalized;








            float hitDistance = hit.distance;
            Vector3 hitPosition = hit.point;
            Vector3 hitNormal = hit.normal;

            TheHitNorm.up = hitNormal;
           



            float newRaycastDistance = raycastDistance - hitDistance;

            int imount = 1;
            Gizmos.color = Color.green;
           // Gizmos.DrawRay(transform.position, Rdir * raycastDistance);


            FaceAngle = SignedAngleBetween(Quaternion.AngleAxis(90, hitNormal) * Rdir, TheHitNorm.forward, hitNormal);
           //Gizmos.DrawRay(hitPosition + (0.001f * hitNormal), TheHitNorm.forward * newRaycastDistance);
            for (int i = 0; i < imount; i++)
            {

               
                Gizmos.color = Color.magenta;
                //Gizmos.DrawRay(hitPosition + (0.001f * hitNormal), Quaternion.AngleAxis(FaceAngle, hitNormal) * Quaternion.AngleAxis(_Angle, hitNormal) * Vector3.Cross(hitNormal, Rdir) * newRaycastDistance);
                //if (Physics.Raycast(hitPosition + (0.001f * hitNormal), Quaternion.AngleAxis(FaceAngle, hitNormal) * Quaternion.AngleAxis(_Angle, hitNormal) *  Vector3.Cross(hitNormal, Rdir), out hit, newRaycastDistance, _LayerMask))
                Gizmos.DrawRay(hitPosition + (0.001f * hitNormal), Quaternion.AngleAxis(_Angle, hitNormal) * Vector3.Cross(hitNormal, Rdir) * newRaycastDistance);
                if (Physics.Raycast(hitPosition + (0.001f * hitNormal), Quaternion.AngleAxis(_Angle, hitNormal) * Vector3.Cross(hitNormal, Rdir), out hit, newRaycastDistance, _LayerMask))


                {
                    imount++;

                    if (_HitMesh != hit.collider.gameObject.GetComponent<MeshCollider>().sharedMesh)
                    {
                        _HitMesh = hit.collider.gameObject.GetComponent<MeshCollider>().sharedMesh;
                    }


                    _Verts = _HitMesh.vertices;

                    _HitTri = hit.triangleIndex;

                    index = hit.triangleIndex * 3;

                    _TriVertIndex1 = _HitMesh.triangles[index];
                    _TriVertIndex2 = _HitMesh.triangles[index + 1];
                    _TriVertIndex3 = _HitMesh.triangles[index + 2];





                    if (_HitTri % 2 == 0)
                    {
                        Rdir = hit.transform.TransformPoint(_Verts[_TriVertIndex2]) - hit.transform.TransformPoint(_Verts[_TriVertIndex1]);
                    }
                    else
                    {
                        Rdir = hit.transform.TransformPoint(_Verts[_TriVertIndex3]) - hit.transform.TransformPoint(_Verts[_TriVertIndex1]);
                    }




                    Rdir = Rdir.normalized;


                    hitDistance = hit.distance;
                    hitPosition = hit.point;
                    hitNormal = hit.normal;
                    newRaycastDistance = newRaycastDistance - hitDistance;
                    
                }
                else
                {
                    break;
                }

            }
        }
    }


    private void CheckDistanceFromGround()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -transform.up, out hit, _Physics._RaycastDistence, _LayerMask))
        {
            _DistanceFromGround = hit.distance;
        }
    }
}
