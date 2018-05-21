using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalCarCalculations
{
    public static Vector3[] CalculateVertexPositonsOfBoxCollider(BoxCollider boxcollider, float offset)
    {
        if (boxcollider)
        {
            Vector3[] vertexPositions = new Vector3[8]; ;
            Vector3 boxColliderCenter = boxcollider.center;

            Vector3 boxColliderExtents = new Vector3(boxcollider.extents.x - offset, boxcollider.extents.y - offset, boxcollider.extents.z - offset);
            for (int i = 0; i < vertexPositions.Length; i++)
            {

                Vector3 ext = boxColliderExtents;
                ext.Scale(new Vector3((i & 1) == 0 ? 1 : -1, (i & 2) == 0 ? 1 : -1, (i & 4) == 0 ? 1 : -1));

                Vector3 vertPositionLocal = boxColliderCenter + ext;

                vertexPositions[i] = boxcollider.transform.TransformPoint(vertPositionLocal);

            }
            return vertexPositions;
        }
        return null;
    }
    public static bool GroundCheck(float[] compresionRatios)
    {
        bool grounded = false;
        for (int i = 0; i < compresionRatios.Length; i++)
        {
            if (compresionRatios[i] > 0)
            {
                grounded = true;
            }
        }
        return grounded;
    }
    public static Vector3 ReturnAverageSurviceNormal(Vector3[] surviceNormal)
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

    public static float CalculateCompressionRatio(float hitDistance, float RayCastDistence)
    {
        float absoluteValeu = 1;
        if (hitDistance > 0)
        {
            float compressionPercentege = (hitDistance / RayCastDistence);
            return absoluteValeu - compressionPercentege;
        }
        return 1;
    }

    public static float ReturnHighestCompressionRatio(float[] compressionRatios)
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

    public static float ReturnAverageCompressionRatio(float[] compressionRatios)
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

    public static Vector3 CalculateGravity(Vector3 usedNormal, float gravityMultiplier)
    {
        Vector3 gravityDirection = usedNormal * gravityMultiplier;
        return gravityDirection;
    }

    public static Vector3 CalculateSuspensionForceDirection(Vector3 combinedNormal, float suspensionForceMultiplier)
    {
        Vector3 suspensionForceDirection = combinedNormal * suspensionForceMultiplier;
        return suspensionForceDirection;
    }
}
