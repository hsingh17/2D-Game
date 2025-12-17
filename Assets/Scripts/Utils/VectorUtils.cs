using System;
using UnityEngine;

public class VectorUtils
{
    public static Vector3 ComponentAbsoluteValue(Vector3 v)
    {
        return new(Math.Abs(v.x), Math.Abs(v.y), Math.Abs(v.z));
    }

    public static Vector2 ComponentAbsoluteValue(Vector2 v)
    {
        return new(Math.Abs(v.x), Math.Abs(v.y));
    }
}
