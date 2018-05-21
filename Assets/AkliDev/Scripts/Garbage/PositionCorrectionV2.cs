using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionCorrectionV2 : MonoBehaviour
{
    [SerializeField]
    private LayerMask _LayerMask;
    private float _DistanceFromGround;
    private CarPhysics _Physics;
    private KANSAIDORIFTO _KANSAIDORIFTO;



    private void Start()
    {
        _Physics = GetComponent<CarPhysics>();
        _KANSAIDORIFTO = GetComponent<KANSAIDORIFTO>();


    }
    private void FixedUpdate()
    {
        RaycastHit downhit;

        Gizmos.DrawRay(transform.position + (transform.up * transform.lossyScale.y), -_Physics._CombinedSurviceNormal * (_Physics._RaycastDistence + (_Physics._RaycastDistence * 0.5f) + transform.lossyScale.y));

        Physics.Raycast(transform.position + (transform.up * transform.lossyScale.y), -_Physics._CombinedSurviceNormal, out downhit, (_Physics._RaycastDistence + (_Physics._RaycastDistence * 0.5f) + transform.lossyScale.y), _LayerMask);

        float distanceFromGround = downhit.distance - transform.lossyScale.y;
        Vector3 downHitPosition = downhit.point;
        Vector3 downHitNormal = downhit.normal;

        Gizmos.color = Color.red;

        RaycastHit hit;
        float raycastDistance = Vector3.Distance(transform.position, transform.position + _KANSAIDORIFTO._UsedVelocity);

        Vector3 positionOnNextFrame = transform.position + _KANSAIDORIFTO._UsedVelocity;
        Vector3 currentDirection = _KANSAIDORIFTO._UsedVelocity;
        currentDirection = currentDirection.normalized;





        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(downHitPosition + (0.001f * downHitNormal), currentDirection * raycastDistance);

        if (Physics.Raycast(downHitPosition + (0.001f * downHitNormal), currentDirection, out hit, raycastDistance, _LayerMask))
        {

            float hitDistance = hit.distance;
            Vector3 hitPosition = hit.point;
            Vector3 hitNormal = hit.normal;






            float newRaycastDistance = raycastDistance - hitDistance;

            int imount = 1;
            Gizmos.color = Color.green;




            for (int i = 0; i < imount; i++)
            {



                Gizmos.DrawRay(hitPosition + (0.001f * hitNormal), Vector3.ProjectOnPlane(currentDirection, hit.normal) * newRaycastDistance);
                if (Physics.Raycast(hitPosition + (0.001f * hitNormal), Vector3.ProjectOnPlane(currentDirection, hit.normal), out hit, newRaycastDistance, _LayerMask))


                {
                    imount++;
                    hitDistance = hit.distance;
                    hitPosition = hit.point;
                    hitNormal = hit.normal;
                    newRaycastDistance = newRaycastDistance - hitDistance;
                    currentDirection = Vector3.ProjectOnPlane(currentDirection, hit.normal);
                }
                else
                {
                    break;
                }

            }
            _Physics._CombinedSurviceNormal = hit.normal;
            transform.position = hitPosition + (hit.normal * distanceFromGround * 2);

        }
    }




    //void OnDrawGizmosSelected()
    //{
    //    RaycastHit downhit;

    //    Gizmos.DrawRay(transform.position + (transform.up * transform.lossyScale.y), -_Physics._CombinedSurviceNormal * (_Physics._RaycastDistence + (_Physics._RaycastDistence * 0.5f) + transform.lossyScale.y));

    //    Physics.Raycast(transform.position + (transform.up * transform.lossyScale.y), -_Physics._CombinedSurviceNormal, out downhit, (_Physics._RaycastDistence + (_Physics._RaycastDistence *0.5f) + transform.lossyScale.y), _LayerMask);

    //    float distanceFromGround = downhit.distance - transform.lossyScale.y;
    //    Vector3 downHitPosition = downhit.point;
    //    Vector3 downHitNormal = downhit.normal;

    //    Gizmos.color = Color.red;

    //    RaycastHit hit;
    //    //float raycastDistance = Vector3.Distance(transform.position, transform.position + _KANSAIDORIFTO._UsedVelocity);
    //    float raycastDistance = 1000;

    //    Vector3 positionOnNextFrame = transform.position + _KANSAIDORIFTO._UsedVelocity;
    //    Vector3 currentDirection = _KANSAIDORIFTO._UsedVelocity;
    //    currentDirection = currentDirection.normalized;





    //    Gizmos.color = Color.magenta;
    //    Gizmos.DrawRay(downHitPosition + (0.001f * downHitNormal), currentDirection * raycastDistance);

    //    if (Physics.Raycast(downHitPosition + (0.001f * downHitNormal), currentDirection, out hit, raycastDistance, _LayerMask))
    //    {

    //        float hitDistance = hit.distance;
    //        Vector3 hitPosition = hit.point;
    //        Vector3 hitNormal = hit.normal;






    //        float newRaycastDistance = raycastDistance - hitDistance;

    //        int imount = 1;
    //        Gizmos.color = Color.green;




    //        for (int i = 0; i < imount; i++)
    //        {
    //            Vector3 precurrentdirection = currentDirection;

    //            if (Vector3.Dot(currentDirection, precurrentdirection) == 1)
    //            {
    //                currentDirection = -currentDirection;
    //            }

    //                Gizmos.DrawRay(hitPosition + (0.002f * hitNormal), Vector3.ProjectOnPlane(currentDirection, hit.normal) * newRaycastDistance);
    //                if (Physics.Raycast(hitPosition + (0.002f * hitNormal), Vector3.ProjectOnPlane(currentDirection, hit.normal), out hit, newRaycastDistance, _LayerMask))


    //                {
    //                    imount++;
    //                    hitDistance = hit.distance;
    //                    hitPosition = hit.point;
    //                    hitNormal = hit.normal;
    //                    newRaycastDistance = newRaycastDistance - hitDistance;

    //                    currentDirection = Vector3.ProjectOnPlane(currentDirection, hit.normal);


    //                }
    //                else
    //                {
    //                    break;
    //                }
                

    //        }
           

    //    }
    //}



}
