//https://answers.unity.com/questions/287081/box-collider-vertexes-in-world-space.html
using UnityEngine;

[ExecuteInEditMode]
public class GetVertexPositionsOfBoxCollider : MonoBehaviour
{

    private BoxCollider _BoxCollider;
    [SerializeField] public Vector3[] _VertexPositions;

    void Start()
    {
        _BoxCollider = GetComponent<BoxCollider>();
        _VertexPositions = new Vector3[8];
    }
    void Update()
    {
        _VertexPositions = CalculateVertexPositonsOfBoxCollider(0);
    }
    public Vector3[] CalculateVertexPositonsOfBoxCollider(float offset)
    {
        if (_BoxCollider)
        {
            Vector3[] vertexPositions = new Vector3[8]; ;
            Vector3 boxColliderCenter = _BoxCollider.center;

            Vector3 boxColliderExtents = new Vector3(_BoxCollider.extents.x - offset, _BoxCollider.extents.y - offset, _BoxCollider.extents.z - offset);
            for (int i = 0; i < vertexPositions.Length; i++)
            {

                Vector3 ext = boxColliderExtents;
                ext.Scale(new Vector3((i & 1) == 0 ? 1 : -1, (i & 2) == 0 ? 1 : -1, (i & 4) == 0 ? 1 : -1));

                Vector3 vertPositionLocal = boxColliderCenter + ext;

                vertexPositions[i] = _BoxCollider.transform.TransformPoint(vertPositionLocal);

            }
            return vertexPositions;
        }
        return null;
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < _VertexPositions.Length; i++)
        {
            Gizmos.DrawSphere(_VertexPositions[i], 0.1f);
        }
    }
}

