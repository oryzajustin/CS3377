using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools
{
    public static bool V3Equal(Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(a - b) < 0.0001;
    }
}
