using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools
{
    // tool for checking equality between two vectors (hack work around for lerping camera to a position and regain control)
    public static bool V3Equal(Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(a - b) < 0.1; // need to check with reduced margin of accuracy
    }
}
