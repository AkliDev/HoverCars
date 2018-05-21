//This Script handles all the hovering calculations of the car.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarHoverHandler : MonoBehaviour
{
    private CarManager _Manager;

    private Mesh _Mesh; //The Mesh that the car is currently driving on.
    private Vector3[] _Normals; // All vertex normals of the mesh that the car is currently driving on;
    private int[] _Triangles; // All Tri's of the mesh that the car is currently driving on;
    private Vector3 _FaceGroundNormal; // Face normal of the of the tri the car is currently driving on
    private Vector3 _PreFaceGroundNormal; //Face normal of the of the tri the car was driving on on the previous fixed update;
    private Vector3 _InterpolatedGroundNormal; // Interpolated normal of the 3 vertecies of the tri the car is currently driving on
    private Vector3 _PreInterpolatedGroundNormal; // Interpolated normal of the 3 vertecies of the tri the car was driving on on the previous fixed update;
    private float _HoverVelocity; // the force that is aplied to the car to get it to the ideal hover height
    private float _PreHoverVelocity; // The hover velocity of the car on the previous fixed time step.
    private float _PreHeight; // The height of the car above the track on the previous fixed time step.
    private bool _IsGrounded;

    public CarManager Manager { get { return _Manager; } }
    public Mesh Mesh { get { return _Mesh; } }
    public Vector3[] Normals { get { return _Normals; } }
    public int[] Triangles { get { return _Triangles; } }
    public Vector3 FacedGroundNormal { get { return _FaceGroundNormal; } }
    public Vector3 PreFacedGroundNormal { get { return _PreFaceGroundNormal; } }
    public Vector3 InterpolatedGroundNormal { get { return _InterpolatedGroundNormal; } }
    public Vector3 PreInterpolatedGroundNormal { get { return _PreInterpolatedGroundNormal; } }
    public float HoverVelocity { get { return _HoverVelocity; } }
    public float PreHoverVelocity { get { return _PreHoverVelocity; } }
    public float PreHeight { get { return _PreHeight; } }
    public bool Grounded { get { return _IsGrounded; } }

    public void SetMesh(Mesh newMesh)
    {
        _Mesh = newMesh;
    }
    public void SetNormals(Vector3[] newNormalArray)
    {
        _Normals = newNormalArray;
    }
    public void SetTriangles(int[] newTrianglesArray)
    {
        _Triangles = newTrianglesArray;
    }
    public void SetFaceGroundNormal(Vector3 newFaceGroundNormal)
    {
        _FaceGroundNormal = newFaceGroundNormal;
    }
    public void SetPreFaceGroundNormal(Vector3 newPreFaceGroundNormal)
    {
        _PreFaceGroundNormal = newPreFaceGroundNormal;
    }
    public void SetInterpolatedGroundNormal(Vector3 newInterpolatedGroundNormal)
    {
        _InterpolatedGroundNormal = newInterpolatedGroundNormal;
    }
    public void SetPreInterpolatedGroundNormal(Vector3 newPreInterpolatedGroundNormal)
    {
        _PreInterpolatedGroundNormal = newPreInterpolatedGroundNormal;
    }
    public void SetHoverVelocoty(float newVelocity)
    {
        _HoverVelocity = newVelocity;
    }
    public void SetPreHoverVelocoty(float newPreVelocity)
    {
        _PreHoverVelocity = newPreVelocity;
    }
    public void SetPreHeight(float newPreHeight)
    {
        _PreHeight = newPreHeight;
    }
    public void SetIsGrounded(bool newIsGrounded)
    {
        _IsGrounded = newIsGrounded;
    }

    private void Awake()
    {
        _Manager = GetComponent<CarManager>();
    }

    void FixedUpdate()
    {
        CalculatHover();
        RotateObjectToNormal();

        transform.Translate(InterpolatedGroundNormal * HoverVelocity * Time.deltaTime, Space.World);

        SetPreFaceGroundNormal(FacedGroundNormal);
        SetPreInterpolatedGroundNormal(InterpolatedGroundNormal);
        SetPreHoverVelocoty(HoverVelocity);
    }

    void CalculatHover()
    {
        Vector3 faceGroundNormal = new Vector3();
        Vector3 interpolatedGroundNormal = new Vector3();

        float force = 0;
        float gravity = 0;

        Ray ray = new Ray(transform.position + (_Manager.RaycastYOffset * transform.up), -transform.up);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, _Manager.MaxGroundDistence + _Manager.RaycastYOffset, _Manager.GroundMask))
        {
            if (Mesh != hitInfo.collider.gameObject.GetComponent<MeshCollider>().sharedMesh)
            {
                SetMesh(hitInfo.collider.gameObject.GetComponent<MeshCollider>().sharedMesh);
                SetNormals(Mesh.normals);
                SetTriangles(Mesh.triangles);
            }

            Transform hitTransform = hitInfo.collider.transform;
            Vector3 n0 = _Normals[_Triangles[hitInfo.triangleIndex * 3 + 0]];
            Vector3 n1 = _Normals[_Triangles[hitInfo.triangleIndex * 3 + 1]];
            Vector3 n2 = _Normals[_Triangles[hitInfo.triangleIndex * 3 + 2]];
            Vector3 baryCenter = hitInfo.barycentricCoordinate;
            interpolatedGroundNormal = n0 * baryCenter.x + n1 * baryCenter.y + n2 * baryCenter.z;
            interpolatedGroundNormal = interpolatedGroundNormal.normalized;
            interpolatedGroundNormal = hitTransform.TransformDirection(interpolatedGroundNormal);
            faceGroundNormal = hitInfo.normal;

            float height = hitInfo.distance - _Manager.RaycastYOffset;
            float forcePercent = _Manager.HoverPID.Seek(_Manager.HoverHeight, height);
            force = _Manager.HoverForce * forcePercent;
            gravity = _Manager.HoverGravity * height;
            SetIsGrounded(true);

            if (height <= 0 && Mathf.Sign(height) != Mathf.Sign(PreHeight) && HoverVelocity < 0)
            {
                SetHoverVelocoty(0);
            }

            SetPreHeight(height);
        }
        else
        {
            interpolatedGroundNormal = PreInterpolatedGroundNormal;
            gravity = _Manager.FallGravity;
            SetIsGrounded(false);
        }

        SetFaceGroundNormal(faceGroundNormal);
        SetInterpolatedGroundNormal(interpolatedGroundNormal);
        SetHoverVelocoty(HoverVelocity + (force * Time.deltaTime));
        SetHoverVelocoty(HoverVelocity - (gravity * Time.deltaTime));
    }

    private void RotateObjectToNormal()
    {
        transform.rotation = Quaternion.FromToRotation(transform.up, InterpolatedGroundNormal) * transform.rotation;
        Manager.SetModelRotation(Quaternion.Slerp(Manager.ModelTransform.rotation, transform.rotation, 4* Time.fixedDeltaTime));
        Manager.SetUpOrientationRotation(Quaternion.Slerp(Manager.OrientationLookAtTransform.rotation, transform.rotation, 7 * Time.fixedDeltaTime));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.up * 5);
        Gizmos.color = Color.white;
        Gizmos.DrawRay(transform.position, InterpolatedGroundNormal * 4);
    }
}
