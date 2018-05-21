using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marth
{
    public static float VectorDeterminant2D(Vector2 a, Vector2 b)
    {
        return (a.x * b.y) - (a.y * b.x);
    }

    public static Vector3 GetDirectionDistance(Vector2 from, Vector2 to)
    {
        return to - from;
    }

    public static float ZeroSign(float f)
    {
        if (f > 0)
        {
            return 1;
        }
        else if
        (f == 0)
        {
            return 0;
        }
        else if (f < 0)
        {
            return -1;
        }
        else
        {
            return Mathf.Infinity;
        }
        
    }
   





}
